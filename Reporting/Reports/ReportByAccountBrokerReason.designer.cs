namespace Reporting.Client
{
    partial class ReportByAccountBrokerReason
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
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.EstTotalData = new DevExpress.XtraReports.UI.XRLabel();
            this.EstSupplimentalData = new DevExpress.XtraReports.UI.XRLabel();
            this.EstExecutionData = new DevExpress.XtraReports.UI.XRLabel();
            this.PercentBudgetData = new DevExpress.XtraReports.UI.XRLabel();
            this.BudgetData = new DevExpress.XtraReports.UI.XRLabel();
            this.TotalData = new DevExpress.XtraReports.UI.XRLabel();
            this.SupplimentalData = new DevExpress.XtraReports.UI.XRLabel();
            this.Execution = new DevExpress.XtraReports.UI.XRLabel();
            this.Reason = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.GroupAccount = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.Account = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.EstTotal = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.EstimateExecution = new DevExpress.XtraReports.UI.XRLabel();
            this.PercentBudget = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.Total = new DevExpress.XtraReports.UI.XRLabel();
            this.Supplimental = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupBroker = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.Broker = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.EstTotalData,
            this.EstSupplimentalData,
            this.EstExecutionData,
            this.PercentBudgetData,
            this.BudgetData,
            this.TotalData,
            this.SupplimentalData,
            this.Execution,
            this.Reason,
            this.xrLabel6});
            this.Detail.HeightF = 29.79169F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // EstTotalData
            // 
            this.EstTotalData.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.EstTotalData.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[se_get_commissions.estimated_total_commission]")});
            this.EstTotalData.LocationFloat = new DevExpress.Utils.PointFloat(808.2916F, 0F);
            this.EstTotalData.Name = "EstTotalData";
            this.EstTotalData.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.EstTotalData.SizeF = new System.Drawing.SizeF(80.20837F, 23F);
            this.EstTotalData.StylePriority.UseBorders = false;
            this.EstTotalData.Text = "TotalData";
            this.EstTotalData.TextFormatString = "{0:n2}";
            // 
            // EstSupplimentalData
            // 
            this.EstSupplimentalData.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.EstSupplimentalData.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[se_get_commissions.estimated_supp_commission]")});
            this.EstSupplimentalData.LocationFloat = new DevExpress.Utils.PointFloat(678.7917F, 0F);
            this.EstSupplimentalData.Name = "EstSupplimentalData";
            this.EstSupplimentalData.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.EstSupplimentalData.SizeF = new System.Drawing.SizeF(80.20837F, 23F);
            this.EstSupplimentalData.StylePriority.UseBorders = false;
            this.EstSupplimentalData.Text = "TotalData";
            this.EstSupplimentalData.TextFormatString = "{0:n2}";
            // 
            // EstExecutionData
            // 
            this.EstExecutionData.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.EstExecutionData.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[se_get_commissions.estimated_exec_commission]")});
            this.EstExecutionData.LocationFloat = new DevExpress.Utils.PointFloat(576.0417F, 0F);
            this.EstExecutionData.Name = "EstExecutionData";
            this.EstExecutionData.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.EstExecutionData.SizeF = new System.Drawing.SizeF(80.20837F, 23F);
            this.EstExecutionData.StylePriority.UseBorders = false;
            this.EstExecutionData.Text = "TotalData";
            this.EstExecutionData.TextFormatString = "{0:n2}";
            // 
            // PercentBudgetData
            // 
            this.PercentBudgetData.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.PercentBudgetData.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[se_get_commissions.PercentBudget]")});
            this.PercentBudgetData.LocationFloat = new DevExpress.Utils.PointFloat(486.4584F, 0F);
            this.PercentBudgetData.Name = "PercentBudgetData";
            this.PercentBudgetData.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.PercentBudgetData.SizeF = new System.Drawing.SizeF(80.20837F, 23F);
            this.PercentBudgetData.StylePriority.UseBorders = false;
            this.PercentBudgetData.Text = "TotalData";
            this.PercentBudgetData.TextFormatString = "{0:0.00%}";
            // 
            // BudgetData
            // 
            this.BudgetData.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.BudgetData.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[se_get_commissions.budget]")});
            this.BudgetData.LocationFloat = new DevExpress.Utils.PointFloat(406.2501F, 0F);
            this.BudgetData.Name = "BudgetData";
            this.BudgetData.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.BudgetData.SizeF = new System.Drawing.SizeF(90.62506F, 23F);
            this.BudgetData.StylePriority.UseBorders = false;
            this.BudgetData.Text = "TotalData";
            this.BudgetData.TextFormatString = "{0:n2}";
            // 
            // TotalData
            // 
            this.TotalData.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.TotalData.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[se_get_commissions.total_commission]")});
            this.TotalData.LocationFloat = new DevExpress.Utils.PointFloat(300.0001F, 0F);
            this.TotalData.Name = "TotalData";
            this.TotalData.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.TotalData.SizeF = new System.Drawing.SizeF(90.62506F, 23F);
            this.TotalData.StylePriority.UseBorders = false;
            this.TotalData.Text = "TotalData";
            this.TotalData.TextFormatString = "{0:n2}";
            // 
            // SupplimentalData
            // 
            this.SupplimentalData.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.SupplimentalData.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[se_get_commissions.supp_commission]")});
            this.SupplimentalData.LocationFloat = new DevExpress.Utils.PointFloat(196.875F, 0F);
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
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[se_get_commissions.exec_commission]")});
            this.Execution.LocationFloat = new DevExpress.Utils.PointFloat(102.7084F, 0F);
            this.Execution.Name = "Execution";
            this.Execution.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Execution.SizeF = new System.Drawing.SizeF(101.0417F, 23F);
            this.Execution.StylePriority.UseBorders = false;
            this.Execution.Text = "Execution";
            this.Execution.TextFormatString = "{0:n2}";
            // 
            // Reason
            // 
            this.Reason.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Reason.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[se_get_commissions.Commission Reason]")});
            this.Reason.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.Reason.Name = "Reason";
            this.Reason.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Reason.SizeF = new System.Drawing.SizeF(110.4167F, 23F);
            this.Reason.StylePriority.UseBorders = false;
            this.Reason.Text = "xrLabel1";
            // 
            // xrLabel6
            // 
            this.xrLabel6.BackColor = System.Drawing.Color.Silver;
            this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(911F, 29.79169F);
            this.xrLabel6.StylePriority.UseBackColor = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 23F;
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
            // GroupAccount
            // 
            this.GroupAccount.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.Account});
            this.GroupAccount.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("short_name", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending),
            new DevExpress.XtraReports.UI.GroupField("mnemonic", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupAccount.HeightF = 23F;
            this.GroupAccount.KeepTogether = true;
            this.GroupAccount.Level = 1;
            this.GroupAccount.Name = "GroupAccount";
            // 
            // Account
            // 
            this.Account.BackColor = System.Drawing.Color.LightSalmon;
            this.Account.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[se_get_commissions.short_name]")});
            this.Account.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.Account.Name = "Account";
            this.Account.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Account.SizeF = new System.Drawing.SizeF(911F, 23F);
            this.Account.StylePriority.UseBackColor = false;
            this.Account.Text = "Account";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.EstTotal,
            this.xrLabel5,
            this.EstimateExecution,
            this.PercentBudget,
            this.xrLabel4,
            this.Total,
            this.Supplimental,
            this.xrLabel2,
            this.xrLabel3,
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
            this.EstTotal.Text = "Est. Total";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(678.7917F, 98.00002F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(115.9582F, 19.79167F);
            this.xrLabel5.StylePriority.UseBorders = false;
            this.xrLabel5.Text = "Est. Supplimental";
            // 
            // EstimateExecution
            // 
            this.EstimateExecution.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.EstimateExecution.LocationFloat = new DevExpress.Utils.PointFloat(576.0417F, 98.00002F);
            this.EstimateExecution.Name = "EstimateExecution";
            this.EstimateExecution.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.EstimateExecution.SizeF = new System.Drawing.SizeF(87.5F, 19.79167F);
            this.EstimateExecution.StylePriority.UseBorders = false;
            this.EstimateExecution.Text = "Est. Execution";
            // 
            // PercentBudget
            // 
            this.PercentBudget.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.PercentBudget.LocationFloat = new DevExpress.Utils.PointFloat(486.4584F, 98.00002F);
            this.PercentBudget.Name = "PercentBudget";
            this.PercentBudget.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.PercentBudget.SizeF = new System.Drawing.SizeF(62.49997F, 19.79167F);
            this.PercentBudget.StylePriority.UseBorders = false;
            this.PercentBudget.Text = "% Budget";
            // 
            // xrLabel4
            // 
            this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(394.7917F, 98.00002F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(52.08331F, 19.79167F);
            this.xrLabel4.StylePriority.UseBorders = false;
            this.xrLabel4.Text = "Budget";
            // 
            // Total
            // 
            this.Total.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Total.LocationFloat = new DevExpress.Utils.PointFloat(300.0001F, 98.00002F);
            this.Total.Name = "Total";
            this.Total.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Total.SizeF = new System.Drawing.SizeF(79.16666F, 19.79167F);
            this.Total.StylePriority.UseBorders = false;
            this.Total.Text = "Total";
            // 
            // Supplimental
            // 
            this.Supplimental.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Supplimental.LocationFloat = new DevExpress.Utils.PointFloat(196.875F, 98.00002F);
            this.Supplimental.Name = "Supplimental";
            this.Supplimental.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Supplimental.SizeF = new System.Drawing.SizeF(79.16666F, 19.79167F);
            this.Supplimental.StylePriority.UseBorders = false;
            this.Supplimental.Text = "Supplimental";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(102.7084F, 98.00002F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(79.16666F, 19.79167F);
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.Text = "Execution";
            // 
            // xrLabel3
            // 
            this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 98.00002F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(79.16666F, 19.79167F);
            this.xrLabel3.StylePriority.UseBorders = false;
            this.xrLabel3.Text = "Reason";
            // 
            // xrLabel7
            // 
            this.xrLabel7.BackColor = System.Drawing.Color.Silver;
            this.xrLabel7.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(0F, 87.58335F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(911F, 30.20834F);
            this.xrLabel7.StylePriority.UseBackColor = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(228.1251F, 23F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(465.2499F, 22.91667F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.Text = "Commission Report: By Account/Broker/Reason";
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.BorderColor = System.Drawing.Color.Transparent;
            this.xrPictureBox1.BorderWidth = 0F;
            this.xrPictureBox1.ImageUrl = "C:\\Sales_Engineering\\Development - 751\\Reporting\\EmptyReport\\linedata_small.gif";
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(726.7084F, 23F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(141.6667F, 45.20833F);
            this.xrPictureBox1.StylePriority.UseBorderColor = false;
            this.xrPictureBox1.StylePriority.UseBorderWidth = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(911F, 23F);
            this.xrLabel1.StylePriority.UseBackColor = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Linedata Services";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // GroupBroker
            // 
            this.GroupBroker.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel9,
            this.Broker});
            this.GroupBroker.HeightF = 23F;
            this.GroupBroker.KeepTogether = true;
            this.GroupBroker.Name = "GroupBroker";
            // 
            // xrLabel9
            // 
            this.xrLabel9.BackColor = System.Drawing.Color.PeachPuff;
            this.xrLabel9.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(1.416651F, 0F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(11.45833F, 22.91666F);
            this.xrLabel9.StylePriority.UseBackColor = false;
            this.xrLabel9.StylePriority.UseBorders = false;
            // 
            // Broker
            // 
            this.Broker.BackColor = System.Drawing.Color.PeachPuff;
            this.Broker.Borders = ((DevExpress.XtraPrinting.BorderSide)(((DevExpress.XtraPrinting.BorderSide.Top | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.Broker.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[se_get_commissions.mnemonic]")});
            this.Broker.LocationFloat = new DevExpress.Utils.PointFloat(12.87498F, 0F);
            this.Broker.Name = "Broker";
            this.Broker.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Broker.SizeF = new System.Drawing.SizeF(900.9999F, 23F);
            this.Broker.StylePriority.UseBackColor = false;
            this.Broker.StylePriority.UseBorders = false;
            this.Broker.Text = "Broker";
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine1,
            this.xrLabel8});
            this.GroupFooter1.HeightF = 23F;
            this.GroupFooter1.Level = 1;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(295.8335F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(94.79175F, 2.083339F);
            // 
            // xrLabel8
            // 
            this.xrLabel8.BackColor = System.Drawing.Color.PeachPuff;
            this.xrLabel8.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "sumSum([se_get_commissions.total_commission])")});
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(300.0001F, 0F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(90.62506F, 23F);
            this.xrLabel8.StylePriority.UseBackColor = false;
            this.xrLabel8.StylePriority.UseBorders = false;
            xrSummary1.IgnoreNullValues = true;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel8.Summary = xrSummary1;
            this.xrLabel8.TextFormatString = "{0:n2}";
            // 
            // ReportByAccountBrokerReason
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupAccount,
            this.ReportHeader,
            this.GroupBroker,
            this.GroupFooter1});
            this.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.Margins = new System.Drawing.Printing.Margins(9, 16, 23, 10);
            this.PageHeight = 1500;
            this.PageWidth = 927;
            this.PaperKind = System.Drawing.Printing.PaperKind.LegalExtra;
            this.Version = "17.2";
            this.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel Reason;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupAccount;
        private DevExpress.XtraReports.UI.XRLabel Account;
        private DevExpress.XtraReports.UI.ReportHeaderBand ReportHeader;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel Execution;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupBroker;
        private DevExpress.XtraReports.UI.XRLabel Broker;
        private DevExpress.XtraReports.UI.XRLabel SupplimentalData;
        private DevExpress.XtraReports.UI.XRLabel Supplimental;
        private DevExpress.XtraReports.UI.XRLabel TotalData;
        private DevExpress.XtraReports.UI.XRLabel Total;
        private DevExpress.XtraReports.UI.XRLabel PercentBudgetData;
        private DevExpress.XtraReports.UI.XRLabel BudgetData;
        private DevExpress.XtraReports.UI.XRLabel PercentBudget;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel EstExecutionData;
        private DevExpress.XtraReports.UI.XRLabel EstimateExecution;
        private DevExpress.XtraReports.UI.XRLabel EstSupplimentalData;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel EstTotalData;
        private DevExpress.XtraReports.UI.XRLabel EstTotal;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
    }
}
