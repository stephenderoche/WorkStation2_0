using System;

using System.Net;

using System.Windows;
using System.Windows.Forms;
using System.Drawing;
    


namespace CurrentOrders.Client.View
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmLogic : System.Windows.Forms.Form
	{
     
        private string[] _symbol;
        private string[] _quantity;
        private string[] _country;
        private int? Ex;
      
        private System.Windows.Forms.Button cmdOK;
        private ComboBox comboBox1;
        private System.Windows.Forms.ListBox LstSecurities;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public frmLogic(string[] symbol, string[] quantity, string[] country)
		{
			//
			// Required for Windows Form Designer support
			//
            this._symbol = symbol;
            this._quantity = quantity;
            this._country = country;
			InitializeComponent();
           
            populateSecurityBox(symbol);
            GetITgList();


			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

       

        

        private void populateSecurityBox(string[] symbol)
        {

            //cmbSecurities.Items.Add("d");
            foreach (string s in symbol)
            {

                LstSecurities.Items.Add(s);

            }

        }

        private void GetITgList()
        {

            IWebProxy webProxy;
            webProxy = GlobalProxySelection.Select;
            webProxy.Credentials = CredentialCache.DefaultCredentials;
            
            //set up web service object
            LogicWebService.ITGLogicWebService ws;
            ws = new LogicWebService.ITGLogicWebService();
            ws.Proxy = webProxy;
            ws.Url = "https://logic.itginc.com/services/CalculationService_v3_0?wsdl";
            ws.PreAuthenticate = true;
            ws.Timeout = 60000;
            ws.Credentials = new NetworkCredential("Linedatatest", "Linedata3");
            


            //set up the input arguments for the web service "calculate" method
            LogicWebService.CalcParameters calcParams = new LogicWebService.CalcParameters();



            calcParams.riskParameters = new LogicWebService.RiskParameters();
            calcParams.riskParameters.benchmarkId = "sp500";
            calcParams.riskParameters.riskModelId = "USA.2.D";

            calcParams.tradelist = new LogicWebService.Tradelist();

            calcParams.tradelist.securitiesArray = new LogicWebService.Security[1];

            LogicWebService.Security security = new LogicWebService.Security();
           
            security.dayDate = System.DateTime.Now;
            calcParams.tradelist.name = "Test";
       
            ws.setTradelist(calcParams.tradelist);

          

            string[] test = ws.getTradelistNames();

            foreach (string s in test)
            {

                comboBox1.Items.Add(s);

            }
          
            comboBox1.SelectedIndex = 0;
        }


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.cmdOK = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.LstSecurities = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(24, 184);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 7;
            this.cmdOK.Text = "Update list";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(24, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(182, 21);
            this.comboBox1.TabIndex = 17;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // LstSecurities
            // 
            this.LstSecurities.FormattingEnabled = true;
            this.LstSecurities.Location = new System.Drawing.Point(24, 44);
            this.LstSecurities.Name = "LstSecurities";
            this.LstSecurities.Size = new System.Drawing.Size(182, 134);
            this.LstSecurities.TabIndex = 18;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(131, 184);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Cancel";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.Location = new System.Drawing.Point(212, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(10, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmLogic
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(229, 219);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LstSecurities);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.cmdOK);
            this.Name = "frmLogic";
            this.Text = "ITG Logic Web Service Example";
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
        static void Main()
        {
           // Application.Run(new frmLogic(_symbol, _quantity));
        }

		private void cmdOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				//set up webProxy object; these are default settings that will
				//work in most Windows environments; more custom code would be
				//necessary for more complex environments.
				IWebProxy webProxy;
				webProxy = GlobalProxySelection.Select;
				webProxy.Credentials = CredentialCache.DefaultCredentials;

				//set up web service object
				LogicWebService.ITGLogicWebService ws;
				ws = new LogicWebService.ITGLogicWebService();
				ws.Proxy = webProxy;
				ws.Url = "https://logic.itginc.com/services/CalculationService_v3_0?wsdl";
				ws.PreAuthenticate = true;
				ws.Timeout = 60000;
				ws.Credentials = new NetworkCredential( "Linedatatest", "Linedata3" );
                

				//set up the input arguments for the web service "calculate" method
				LogicWebService.CalcParameters calcParams = new LogicWebService.CalcParameters();
				calcParams.riskParameters = new LogicWebService.RiskParameters();
				calcParams.riskParameters.benchmarkId = "sp500";
				calcParams.riskParameters.riskModelId = "USA.3.D";

               
                

				calcParams.tradelist = new LogicWebService.Tradelist();

                calcParams.tradelist.securitiesArray = new LogicWebService.Security[_symbol.Length];
                
                for (int i = 0; i < _symbol.Length; i++)
                {
                    LogicWebService.Security security = new LogicWebService.Security();
                    security.stockId = _symbol[i];
                    security.country = _country[i];
                    security.amount = Convert.ToDouble(_quantity[i]);
                    security.dayDate = System.DateTime.Now;
                    calcParams.tradelist.securitiesArray[i] = security;
                  
                    
                    calcParams.dayDate = security.dayDate;
                }


                calcParams.tradelist.name = comboBox1.Text;

                calcParams.sourceApp = "a";

             
               

            
               
                ////make the call to web services
                
                    LogicWebService.CalcResults calcResults = ws.calculate(calcParams);
            
              

                //int ex = if (calcResults.tradelistResults.exceptions.Count();

               // int? ex1 = Convert.ToInt32(calcResults.tradelistResults.exceptions.Count());

			    int? ex1 = 0;
           
                //TODO:this int got rid of
             // var ex1 = (int?) calcResults.tradelistResults.exceptions.Count() > 0 ?calcResults.tradelistResults.exceptions.Count():0;

                    //var ex1 = 0;    
         

                for (int i = 0; i < ex1; i++)
                {

                    LogicWebService.CalcException secExceptions = (LogicWebService.CalcException)calcResults.tradelistResults.exceptions.GetValue(i);

                    System.Windows.MessageBox.Show("There was a problem with Security " + secExceptions.securityException.stockId.ToString() + ": " + secExceptions.message.ToString());
                }


                ////get the results and show in the text boxes
                //LogicWebService.SecurityResults secResults = (LogicWebService.SecurityResults)calcResults.tradelistResults.securityResultsArray.GetValue(0);

                if (_symbol.Length > ex1)
                {
                    System.Windows.MessageBox.Show("List Updated");
                    this.Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("No Valid Securities selected.");
                    this.Close();
                }

			}
			catch( Exception ex )
			{
                string a = string.Format("{0}",ex.Message);
                if(a != "Value cannot be null.\r\nParameter name: source")
			        System.Windows.MessageBox.Show( ex.Message );
                else
                    System.Windows.MessageBox.Show("List Updated");
                    this.Close();
			}

}

      


        /// <summary>
        /// This holds the LongViewAddInbase.
        /// </summary>
       

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            IWebProxy webProxy;
            webProxy = GlobalProxySelection.Select;
            webProxy.Credentials = CredentialCache.DefaultCredentials;

            //set up web service object
            LogicWebService.ITGLogicWebService ws;
            ws = new LogicWebService.ITGLogicWebService();
            ws.Proxy = webProxy;
            ws.Url = "https://logic.itginc.com/services/CalculationService_v3_0?wsdl";
            ws.PreAuthenticate = true;
            ws.Timeout = 60000;
            ws.Credentials = new NetworkCredential("Linedatatest", "Linedata3");

            ws.deleteTradelist(comboBox1.Text);

            comboBox1.Items.Clear();

            GetITgList();
        }

       
	}
}
