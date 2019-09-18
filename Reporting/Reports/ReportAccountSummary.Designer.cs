namespace Reporting.Client

{
    partial class ReportAccountSummary
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TotalData = new DevExpress.XtraReports.UI.XRLabel();
            this.SupplimentalData = new DevExpress.XtraReports.UI.XRLabel();
            this.Execution = new DevExpress.XtraReports.UI.XRLabel();
            this.Reason = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.EstTotal = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.EstimateExecution = new DevExpress.XtraReports.UI.XRLabel();
            this.PercentBudget = new DevExpress.XtraReports.UI.XRLabel();
            this.Total = new DevExpress.XtraReports.UI.XRLabel();
            this.Supplimental = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.Broker = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.sqlDataSource3 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel9,
            this.xrLabel4,
            this.xrLabel3,
            this.TotalData,
            this.SupplimentalData,
            this.Execution,
            this.Reason,
            this.xrLabel6});
            this.Detail.HeightF = 29.79169F;
            this.Detail.KeepTogether = true;
            this.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnWidth;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TotalData
            // 
            this.TotalData.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.TotalData.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[rpx_account_compare].[market_value]")});
            this.TotalData.LocationFloat = new DevExpress.Utils.PointFloat(421.8751F, 0F);
            this.TotalData.Name = "TotalData";
            this.TotalData.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TotalData.SizeF = new System.Drawing.SizeF(110.4167F, 23F);
            this.TotalData.StylePriority.UseBorders = false;
            this.TotalData.Text = "TotalData";
            this.TotalData.TextFormatString = "{0:n2}";
            // 
            // SupplimentalData
            // 
            this.SupplimentalData.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.SupplimentalData.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[rpx_account_compare].[quantity_booked]")});
            this.SupplimentalData.LocationFloat = new DevExpress.Utils.PointFloat(295.8335F, 0F);
            this.SupplimentalData.Name = "SupplimentalData";
            this.SupplimentalData.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.SupplimentalData.SizeF = new System.Drawing.SizeF(101.0417F, 23F);
            this.SupplimentalData.StylePriority.UseBorders = false;
            this.SupplimentalData.Text = "SupplimentalData";
            this.SupplimentalData.TextFormatString = "{0:n2}";
            // 
            // Execution
            // 
            this.Execution.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Execution.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[rpx_account_compare].[description]")});
            this.Execution.LocationFloat = new DevExpress.Utils.PointFloat(144.3751F, 0F);
            this.Execution.Name = "Execution";
            this.Execution.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Execution.SizeF = new System.Drawing.SizeF(140.6251F, 23F);
            this.Execution.StylePriority.UseBorders = false;
            this.Execution.Text = "Execution";
            this.Execution.TextFormatString = "{0:n2}";
            // 
            // Reason
            // 
            this.Reason.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Reason.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[rpx_account_compare].[symbol]")});
            this.Reason.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 0F);
            this.Reason.Name = "Reason";
            this.Reason.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Reason.SizeF = new System.Drawing.SizeF(123.9583F, 23F);
            this.Reason.StylePriority.UseBorders = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel6.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(902F, 29.79169F);
            this.xrLabel6.StylePriority.UseBackColor = false;
            this.xrLabel6.StylePriority.UseBorderColor = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 1.125002F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.BottomMargin.BorderWidth = 2F;
            this.BottomMargin.HeightF = 10.12503F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.StylePriority.UseBorders = false;
            this.BottomMargin.StylePriority.UseBorderWidth = false;
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.EstTotal,
            this.xrLabel5,
            this.EstimateExecution,
            this.PercentBudget,
            this.Total,
            this.Supplimental,
            this.xrLabel2,
            this.Broker,
            this.xrLabel7,
            this.xrLabel17,
            this.xrPictureBox1,
            this.xrLabel1});
            this.ReportHeader.HeightF = 117.7917F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // EstTotal
            // 
            this.EstTotal.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.EstTotal.LocationFloat = new DevExpress.Utils.PointFloat(809.2917F, 98.00002F);
            this.EstTotal.Name = "EstTotal";
            this.EstTotal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.EstTotal.SizeF = new System.Drawing.SizeF(92.70831F, 19.79167F);
            this.EstTotal.StylePriority.UseBorders = false;
            this.EstTotal.Text = "Delta";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(693.375F, 98.00002F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(82.62482F, 19.79167F);
            this.xrLabel5.StylePriority.UseBorders = false;
            this.xrLabel5.Text = "Model";
            // 
            // EstimateExecution
            // 
            this.EstimateExecution.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.EstimateExecution.LocationFloat = new DevExpress.Utils.PointFloat(600F, 98.00002F);
            this.EstimateExecution.Name = "EstimateExecution";
            this.EstimateExecution.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.EstimateExecution.SizeF = new System.Drawing.SizeF(53.125F, 19.79167F);
            this.EstimateExecution.StylePriority.UseBorders = false;
            this.EstimateExecution.Text = "Acct.";
            // 
            // PercentBudget
            // 
            this.PercentBudget.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.PercentBudget.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Underline);
            this.PercentBudget.LocationFloat = new DevExpress.Utils.PointFloat(600F, 78.20834F);
            this.PercentBudget.Name = "PercentBudget";
            this.PercentBudget.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.PercentBudget.SizeF = new System.Drawing.SizeF(259F, 19.79167F);
            this.PercentBudget.StylePriority.UseBorders = false;
            this.PercentBudget.StylePriority.UseFont = false;
            this.PercentBudget.StylePriority.UseTextAlignment = false;
            this.PercentBudget.Text = "% of Denominator";
            this.PercentBudget.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.PercentBudget.TextFormatString = "{0:0.00%}";
            // 
            // Total
            // 
            this.Total.AutoWidth = true;
            this.Total.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Total.CanShrink = true;
            this.Total.LocationFloat = new DevExpress.Utils.PointFloat(421.8751F, 98.00002F);
            this.Total.Multiline = true;
            this.Total.Name = "Total";
            this.Total.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Total.SizeF = new System.Drawing.SizeF(110.4166F, 19.79167F);
            this.Total.StylePriority.UseBorders = false;
            this.Total.StylePriority.UseTextAlignment = false;
            this.Total.Text = "Unit Market Value\t\t\t";
            this.Total.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // Supplimental
            // 
            this.Supplimental.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Supplimental.LocationFloat = new DevExpress.Utils.PointFloat(300.0001F, 98.00002F);
            this.Supplimental.Name = "Supplimental";
            this.Supplimental.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Supplimental.SizeF = new System.Drawing.SizeF(79.16666F, 19.79167F);
            this.Supplimental.StylePriority.UseBorders = false;
            this.Supplimental.Text = "Units\t";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(144.3751F, 98.00002F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(79.16666F, 19.79167F);
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.Text = "Description";
            // 
            // Broker
            // 
            this.Broker.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Broker.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 98.00002F);
            this.Broker.Name = "Broker";
            this.Broker.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Broker.SizeF = new System.Drawing.SizeF(79.16666F, 19.79167F);
            this.Broker.StylePriority.UseBorders = false;
            this.Broker.Text = "Symbol";
            // 
            // xrLabel7
            // 
            this.xrLabel7.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(0F, 78.20835F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(902F, 39.58334F);
            this.xrLabel7.StylePriority.UseBackColor = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel17
            // 
            this.xrLabel17.BorderColor = System.Drawing.Color.Transparent;
            this.xrLabel17.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(300.0001F, 23F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(363.5416F, 22.91667F);
            this.xrLabel17.StylePriority.UseBorderColor = false;
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = "Account vs. Model\t";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.BorderColor = System.Drawing.Color.Transparent;
            this.xrPictureBox1.ImageUrl = "C:\\Sales_Engineering\\Development - 751\\Reporting\\EmptyReport\\linedata_small.gif";
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(693.375F, 23F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(141.6667F, 45.20833F);
            this.xrPictureBox1.StylePriority.UseBorderColor = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(902F, 23F);
            this.xrLabel1.StylePriority.UseBackColor = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Linedata Services";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // sqlDataSource3
            // 
            this.sqlDataSource3.Name = "sqlDataSource3";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[rpx_account_compare].[percent_port]")});
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(600F, 0F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(53.125F, 23F);
            this.xrLabel3.StylePriority.UseBorders = false;
            this.xrLabel3.Text = "TotalData";
            this.xrLabel3.TextFormatString = "{0:p2}";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[rpx_account_compare].[model_percent]")});
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(706.9167F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(53.125F, 23F);
            this.xrLabel4.StylePriority.UseBorders = false;
            this.xrLabel4.Text = "TotalData";
            this.xrLabel4.TextFormatString = "{0:p2}";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[rpx_account_compare].[model_percent] - [rpx_account_compare].[percent_port]")});
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(809.2917F, 0F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(53.125F, 23F);
            this.xrLabel9.StylePriority.UseBorders = false;
            this.xrLabel9.Text = "TotalData";
            this.xrLabel9.TextFormatString = "{0:p2}";
            // 
            // ReportAccountSummary
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader});
            this.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.sqlDataSource3});
            this.DataMember = "se_get_commissions";
            this.DataSource = this.sqlDataSource3;
            this.Margins = new System.Drawing.Printing.Margins(9, 16, 1, 10);
            this.PageHeight = 1500;
            this.PageWidth = 927;
            this.PaperKind = System.Drawing.Printing.PaperKind.LegalExtra;
            this.ScriptsSource = "\r\n";
            this.Version = "17.2";
            this.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel Reason;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel Broker;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel Execution;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel SupplimentalData;
        private DevExpress.XtraReports.UI.XRLabel Supplimental;
        private DevExpress.XtraReports.UI.XRLabel TotalData;
        private DevExpress.XtraReports.UI.XRLabel Total;
        private DevExpress.XtraReports.UI.XRLabel PercentBudget;
        private DevExpress.XtraReports.UI.XRLabel EstimateExecution;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel EstTotal;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        
        private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
        private DevExpress.XtraReports.UI.DetailBand detailBand1;
        private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;


    }
}
