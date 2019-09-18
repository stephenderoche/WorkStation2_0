namespace Guages.Client.View
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Guages.Client.ViewModel;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using Linedata.Shared.Widget.Common;
    using DevExpress.Xpf.Grid;
    using System.Threading;
    using SalesSharedContracts;
    using Linedata.Framework.Foundation;
    using System.Data;
    using System;
    using Linedata.Shared.Api.ServiceModel;
    using Guages.Client;
    using System.IO;
    using DevExpress.Xpf.Bars;

    using DevExpress.Xpf.Editors;
    using DevExpress.Xpf.Core.ConditionalFormatting;
    using DevExpress.Xpf.Core.Serialization;
    using DevExpress.Xpf.Gauges;

    /// <summary>
    /// Interaction logic for TestSampleView.xaml
    /// </summary>
    public partial class GuagesView : UserControl
    {
      
      
 
        public GuagesViewModel view;
     
        System.Windows.Forms.Timer timer1;

        # region Parameters

        private bool _reverse;
        public bool Reverse
        {
            set
            {
                _reverse = value;
            }
            get { return _reverse; }
        }


        private int _messageCount;
        public int MessageCount
        {
            set
            {
                _messageCount = value;


            }
            get { return _messageCount; }
        }

        private int _low = 10;
        public int Low
        {
            set
            {
                _low = value;


            }
            get { return _low; }
        }


        private int _medium;
        public int Medium
        {
            set
            {
                _medium = value;


            }
            get { return _medium; }
        }

        private int _high = 50;
        public int High
        {
            set
            {
                _high = value;


            }
            get { return _high; }
        }

        private string _gaugetype = "Number of Messages";
        public string Gaugetype
        {
            set
            {
                _gaugetype = value;


            }
            get { return _gaugetype; }
        }

      


        #endregion Parameters


        public GuagesView(GuagesViewModel ViewModel)
        {
            
            InitializeComponent();
           
         
            this.DataContext = ViewModel;
         
            this.view = ViewModel;
           
        }

     
    
 

        # region Helperfunctions

      
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            this.DataContext = view;
            High = view.Parameters.High;
            Gaugetype = view.Parameters.GuageType;
            MessageCount = view.Parameters.Count;
            Reverse = view.Parameters.Reverse;

            setform();

            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 6000;//5 seconds
            timer1.Tick += new System.EventHandler(timer1_Tick);
            timer1.Start();
        }
    

        public void setform()
        {
            if(Reverse)
           {
               GuageRev.Visibility = System.Windows.Visibility.Visible;
               GuageNotRev.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                GuageRev.Visibility = System.Windows.Visibility.Hidden;
                GuageNotRev.Visibility = System.Windows.Visibility.Visible;
            }
        }
 
  

      

        # endregion Helperfunctions

        # region Methods

        private void needle_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            MessageCount = Convert.ToInt32(e.NewValue);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if(Reverse)
            {
                ScaleReverse.EndValue = High;

                if (High < 10)
                    ScaleReverse.MajorIntervalCount = High;
                else if (High > 10 & High < 20)
                    ScaleReverse.MajorIntervalCount = 2;
                else if (High > 20 & High < 30)
                    ScaleReverse.MajorIntervalCount = 3;
                else
                    ScaleReverse.MajorIntervalCount = 5;
            }
            else
            {
                Scale.EndValue = High;

                if (High < 10)
                    Scale.MajorIntervalCount = High;
                else if (High > 10 & High < 20)
                    Scale.MajorIntervalCount = 2;
                else if (High > 20 & High < 30)
                    Scale.MajorIntervalCount = 3;
                else
                    Scale.MajorIntervalCount = 5;
            }
          
        }

        #endregion methods



    }
}
