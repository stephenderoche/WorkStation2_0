using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using FileHelpers;
using FileHelpers.DataLink;
using FileHelpers.RunTime;
using Linedata.Client;
using Linedata.Framework.Foundation;
using Linedata.Shared.Api.ServiceModel;
using Microsoft.Win32;
using SalesTools.Shared.Contracts;
using Xceed.Wpf.DataGrid;
using Xceed.Wpf.DataGrid.ValidationRules;
using DataRow = System.Data.DataRow;
using OrderImportPane.Client.Helper;
using Linedata.Framework.WidgetFrame.PluginBase;
using Linedata.Client.AddIn.HostView.AppAddIns;
using Linedata.Client.AddIn;

using Linedata.Client.AddIn.AppAddIns;
using Linedata.Client.AddIn.AppAddIns.LongView;

//using MyGrid = myGrid::Xceed.Wpf.DataGrid;
//using MyVal = myGrid::Xceed.Wpf.DataGrid.ValidationRules;

namespace OrderImportPane.Client
{
    /// <summary>
    /// Interaction logic for OrderImportControl.xaml
    /// </summary>
    public partial class OrderImportControl
    {
        [DllImport("User32.Dll", EntryPoint = "PostMessageA", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public DataTable borders;
        //public OrderImportPaneAddIn addin;

       
        readonly DataGridCollectionView collectionView;

        readonly LinedataApp addInBase;

        StatusWindow statusWindow;
        public bool statusUp;
        readonly DispatcherTimer dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);

        bool localPropAllorNone;

        // public OrderImportControl( ILinedataApp cli)
           public OrderImportControl()
        {
            addInBase = LinedataApp.Instance;

            try
            {
                Licenser.LicenseKey = "DGF13-TFBDU-97Y7J-L82A";

                ProposedOrdersDataSet.ProposedOrdersPasteDataTable table = new ProposedOrdersDataSet.ProposedOrdersPasteDataTable();

                borders = table;

                InitializeComponent();
                collectionView = new DataGridCollectionView(borders.DefaultView);
                //collectionView.ItemProperties["ProposedValue"].SortComparer = new ValueComparer();
                grid.ItemsSource = collectionView;
                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
                dispatcherTimer.Tick += dispatcherTimer_Tick;
            }
            catch (Exception ex)
            {
                throw ex;
           
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            AutoSizeColumns();
        }

        protected void OnExecutedPaste(object target, ExecutedRoutedEventArgs args)
        {
            CloseStatusWindow();
            PasteFromClip();
            args.Handled = true;
        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseStatusWindow()
        {
            if (statusWindow != null)
                if (statusWindow.IsVisible)
                {
                    try
                    {
                        statusWindow.Close();
                    }
                    // ReSharper disable EmptyGeneralCatchClause
                    catch
                    // ReSharper restore EmptyGeneralCatchClause
                    {
                    }
                }
        }

        private DelimitedClassBuilder ExamineGridForColumnOrder(string delimiter)
        {
            DelimitedClassBuilder classBuilder = new DelimitedClassBuilder("ProposedOrders", delimiter);
            foreach (Column column in grid.VisibleColumns.Where(column => column.Visible && column.FieldName != "ErrorString"))
            {
                classBuilder.AddField(column.FieldName, typeof(string));
                classBuilder.LastField.TrimMode = TrimMode.Both;
                classBuilder.LastField.FieldQuoted = true;
            }
            return classBuilder;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            DelimitedClassBuilder classBuilder = ExamineGridForColumnOrder(",");
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Filter =
                    "csv files (*.csv)|*.csv|Excel files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*",
                FilterIndex = 3
            };

            if (openFileDialog1.ShowDialog() != true)
            {
                return;
            }
            try
            {
                DataTable dt;
                if (Path.GetExtension(openFileDialog1.FileName).ToLower().Equals(".csv"))
                {
                    FileHelperEngine engine = new FileHelperEngine(classBuilder.CreateRecordClass());
                    dt = engine.ReadFileAsDT(openFileDialog1.FileName);
                }
                else if (Path.GetExtension(openFileDialog1.FileName).ToLower().Equals(".xls")
                    || Path.GetExtension(openFileDialog1.FileName).ToLower().Equals(".xlsx"))
                {
                    ExcelStorage storage = new ExcelStorage(classBuilder.CreateRecordClass(), openFileDialog1.FileName, 1, 1);

                    dt = storage.ExtractRecordsAsDT();
                }
                else
                {
                    return;
                }
                PasteDataTable(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
        }

        private void PasteDataTable(DataTable dt)
        {
            try
            {
                ProposedOrdersDataSet.ProposedOrdersPasteDataTable table = borders as ProposedOrdersDataSet.ProposedOrdersPasteDataTable;
                if (table == null)
                    return;

                foreach (DataRow dataRow in dt.Rows)
                {
                    ProposedOrdersDataSet.ProposedOrdersPasteRow row = table.NewProposedOrdersPasteRow();
                    row.PromptSymbol = dataRow["PromptSymbol"].ToString();
                    row.SideMnemonic = dataRow["SideMnemonic"].ToString();
                    row.ProposedValue = dataRow["ProposedValue"].ToString();
                    row.AccountShortName = dataRow["AccountShortName"].ToString();
                    //row.Broker = dataRow["Broker"].ToString();
                   // row.UserField1 = dataRow["UserField1"].ToString();
                   // row.UserField2 = dataRow["UserField2"].ToString();
                   // row.UserField8 = dataRow["UserField8"].ToString();
                    
               
                    table.AddProposedOrdersPasteRow(row);
                }
                dispatcherTimer.Start();
            }
            catch (Exception ex)
            {
                throw ex;
               // addInBase.MessageService.LogMessage(new ApplicationMessage(ex, ApplicationMessageType.ErrorNoPopup));
            }
        }

        private void PasteFromClip()
        {
            DelimitedClassBuilder classBuilder = ExamineGridForColumnOrder("\t");
            try
            {
                FileHelperEngine engine = new FileHelperEngine(classBuilder.CreateRecordClass());
                DataTable dt = engine.ReadStringAsDT(Clipboard.GetText());
                PasteDataTable(dt);
            }
            catch (Exception ex)
            {
                throw ex;
                //addInBase.MessageService.LogMessage(new ApplicationMessage(ex, ApplicationMessageType.ErrorNoPopup));
            }
        }

        private void Import_Orders_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CloseStatusWindow();
                grid.EndEdit();
                BackgroundWorker backgroundWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = false,
                    WorkerSupportsCancellation = false
                };
                backgroundWorker.DoWork += backgroundWorker_DoWork;
                //backgroundWorker_DoWork();
                backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

                statusWindow = new StatusWindow();
                statusWindow.Show();
                statusWindow.StatusText = "Importing Orders";
                statusUp = true;
                localPropAllorNone = AllOrNone;

                backgroundWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                throw ex;
                //addInBase.MessageService.LogMessage(new ApplicationMessage(ex, ApplicationMessageType.ErrorNoPopup));
                // MessageBox.Show("The data you pasted is in the wrong format");
               // return;
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                statusWindow.ImportComplete = true;
                //PostMessage(addin.LongViewApp.MainWindowHandle, 32874, IntPtr.Zero, (IntPtr)16);

               
            }
            catch (Exception ex)
            {
                throw ex;
                //addInBase.MessageService.LogMessage(new ApplicationMessage(ex, ApplicationMessageType.ErrorNoPopup));
                // MessageBox.Show("The data you pasted is in the wrong format");
               // return;
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
            //private void backgroundWorker_DoWork()
        {
            int errorCount = 0;
            int warningCount = 0;

            ProposedOrderList proposedOrderList = new ProposedOrderList();
            proposedOrderList.AddRange(from ProposedOrdersDataSet.ProposedOrdersPasteRow row in borders.Rows
                                       select new ProposedOrder
                                       {
                                           PromptSymbol = row.PromptSymbol,
                                           ProposedValue = row.ProposedValue,
                                           SideMnemonic = row.SideMnemonic,
                                           AccountShortName = row.AccountShortName,
                                           //Broker = "",//row.Broker,
                                           //UserField1 = "",// row.UserField1,
                                           //UserField2 = "",// row.UserField2,
                                           //UserField8 = "",// row.UserField8,
                                           id = row.id
                                       });

            try
            {
                ApplicationMessageList messageList;
                ApplicationMessageList messageListWarn = new ApplicationMessageList();
                SalesToolsServiceContract contract = TransportManagerProxyFactory<SalesToolsServiceContract>.GetReusableInstance("SalesTools");
                ProposedOrderErrorList errorList = contract.AddProposed(proposedOrderList, localPropAllorNone, out messageList);
               // addInBase.MessageService.LogMessagesFromBackgroundThread(messageList);
                foreach (ProposedOrderError proposedOrderError in errorList)
                {
                    DataView view = borders.AsDataView();
                    view.Sort = "id";
                    int j = view.Find(proposedOrderError.id);
                    view[j]["ErrorString"] = proposedOrderError.error;
                    if (!string.IsNullOrEmpty(proposedOrderError.error))
                    {
                        CompareInfo compareInfo = CultureInfo.CurrentCulture.CompareInfo;
                        if (compareInfo.IsPrefix(proposedOrderError.error, "WARNING", CompareOptions.IgnoreCase))
                        {
                            warningCount++;
                            messageListWarn.Add(new ApplicationMessage(proposedOrderError.error, ApplicationMessageType.Warning));
                        }
                        else
                        {
                            errorCount++;
                        }
                    }
                }

                // remove successfully imported rows
                if (errorCount == 0 || (errorCount > 0 && !localPropAllorNone))
                {
                    foreach (ProposedOrderError proposedOrderError in from proposedOrderError in errorList
                                                                      where proposedOrderError.proposed_order_id != 0
                                                                      select proposedOrderError)
                    {
                        DataView view = borders.AsDataView();
                        view.Sort = "id";
                        int j = view.Find(proposedOrderError.id);
                        statusWindow.Dispatcher.Invoke(new Action(() => view.Delete(j)));
                    }
                }
            //    addInBase.MessageService.LogMessagesFromBackgroundThread(messageListWarn);

              



                
            }
            catch (Exception ex)
            {
                ApplicationMessageList list = new ApplicationMessageList { new ApplicationMessage(ex, ApplicationMessageType.ErrorNoPopup) };
               // addInBase.MessageService.LogMessagesFromBackgroundThread(list);
                throw ex;
            }
            statusWindow.Dispatcher.BeginInvoke(new Action(() => statusWindow.StatusText =
                                                                 string.Format(
                                                                     "Order Import Complete\r\n\r\nTotal attempted {0}\r\nErrors {1}\r\nWarnings {2}\r\n\r\nTotal Imported {3}",
                                                                     proposedOrderList.Count,
                                                                     errorCount,
                                                                     warningCount,
                                                                     !localPropAllorNone
                                                                         ? proposedOrderList.Count - errorCount
                                                                         : ((errorCount == 0) ? proposedOrderList.Count : 0))));
        }

        public bool AllOrNone
        {
            get { return (bool)GetValue(AllOrNoneProperty); }
            set { SetValue(AllOrNoneProperty, value); }
        }

        public static readonly DependencyProperty AllOrNoneProperty =
                DependencyProperty.Register("AllOrNone", typeof(bool), typeof(OrderImportControl), new UIPropertyMetadata(false));

        private void Clear_Grid_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CloseStatusWindow();
                grid.EndEdit();
                borders.Clear();
            }
            catch (Exception ex)
            {
                throw ex;
               // addInBase.MessageService.LogMessage(new ApplicationMessage(ex, ApplicationMessageType.ErrorNoPopup));
                // MessageBox.Show("The data you pasted is in the wrong format");
               // return;
            }
        }

        private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                grid.EndEdit();
               // AutoSizeColumns();
            }
            catch (Exception ex)
            {
                throw ex;
                //addInBase.MessageService.LogMessage(new ApplicationMessage(ex, ApplicationMessageType.ErrorNoPopup));
                // MessageBox.Show("The data you pasted is in the wrong format");
               // return;
            }
        }

        private long AutoSizeInterlock;

        public void AutoSizeColumns()
        {
            if (!grid.Dispatcher.CheckAccess())
            {
                grid.Dispatcher.BeginInvoke((Action)(AutoSizeColumns));
                return;
            }
            if (Interlocked.CompareExchange(ref AutoSizeInterlock, 1, 0) == 0)
            {
                try
                {
                    // if its too small (or zero like during startup) do nothing
                    if (ActualWidth < 100)
                        return;

                    double widthUsed = 0;
                    Column errorColumn = null;
                    foreach (Column column in grid.Columns)
                    {
                        if (column.Visible)
                        {
                            if (column.FieldName != "ErrorString")
                            {
                                double fitWidth = column.GetFittedWidth();
                                if (fitWidth > 0)
                                {
                                    column.Width = fitWidth;
                                    widthUsed += fitWidth;
                                }
                            }
                            else
                            {
                                errorColumn = column;
                            }
                        }
                    }
                    if (errorColumn != null)
                    {
                        // fill the rest of the space with the errorColumn
                        double calc = ActualWidth - widthUsed - (SystemParameters.VerticalScrollBarWidth + ((Xceed.Wpf.DataGrid.Views.TableView)(grid.View)).DetailIndicatorWidth);
                        if (calc < 10)
                            calc = errorColumn.GetFittedWidth();
                        errorColumn.Width = calc;
                        errorColumn.MinWidth = errorColumn.GetFittedWidth();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                    //addInBase.MessageService.LogMessage(new ApplicationMessage(ex, ApplicationMessageType.ErrorNoPopup));
                    //return;
                }
                finally
                {
                    AutoSizeInterlock = 0;
                }
            }
        }

        private void OnExecutedDelete(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void OnExecutedSelectAll(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CloseStatusWindow();
            PasteFromClip();
            e.Handled = true;
        }
    }

    public class HasAnError : CellValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo culture, CellValidationContext context)
        {
            string val = value as string;

            if (!string.IsNullOrEmpty(val))
            {
                CompareInfo compareInfo = CultureInfo.CurrentCulture.CompareInfo;
                if (compareInfo.IsPrefix(val, "ERROR", CompareOptions.IgnoreCase))
                    return new ValidationResult(false, val);
            }
            return new ValidationResult(true, null);
        }
    }

    public class SideCellValid : CellValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo culture, CellValidationContext context)
        {
            if (value == null)
                return new ValidationResult(true, null);
            string[] sides = { "B", "BMV", "S", "SMV", "BC", "BCMV", "SS", "SSMV", "SSB", "SSBMV" };
            if (sides.Contains(value.ToString().ToUpper()))
            {
                context.Cell.ToolTip = null;
                return new ValidationResult(true, null);
            }
            context.Cell.ToolTip = "Invalid side";
            return new ValidationResult(false, "Invalid side");
        }
    }

    public class NullToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            if (string.IsNullOrEmpty(s))
                return false;
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class ValueComparer : IComparer
    {
        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <returns>
        /// Value
        ///                     Condition
        ///                     Less than zero
        ///                 <paramref name="x"/> is less than <paramref name="y"/>.
        ///                     Zero
        ///                 <paramref name="x"/> equals <paramref name="y"/>.
        ///                     Greater than zero
        ///                 <paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        /// <param name="x">The first object to compare.
        ///                 </param><param name="y">The second object to compare.
        ///                 </param><exception cref="T:System.ArgumentException">Neither <paramref name="x"/> nor <paramref name="y"/> implements the <see cref="T:System.IComparable"/> interface.
        ///                     -or-
        ///                 <paramref name="x"/> and <paramref name="y"/> are of different types and neither one can handle comparisons with the other.
        ///                 </exception><filterpriority>2</filterpriority>
        public int Compare(object x, object y)
        {
            if (x == null || y == null)
            {
                if (x != null || y != null)
                    return 1;
                return 0;
            }

            double valX;
            double valY;

            double.TryParse(x.ToString(), out valX);
            double.TryParse(y.ToString(), out valY);
            if (valX > valY)
                return 1;
            if (valX < valY)
                return -1;
            return 0;
        }
    }
}