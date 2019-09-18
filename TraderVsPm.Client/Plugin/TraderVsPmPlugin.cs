namespace TraderVsPM.Client.Plugin
{
    using Linedata.Client.Widget.BaseWidget.Plugin;
    using Linedata.Framework.WidgetFrame.PluginBase;
    using System.ComponentModel.Composition;

  

    [Plugin(
      "Trader Vs. Pm",
      "Trading",
      "935EFA2F-F9A7-40E0-86C4-BF6EE527F649",
      "pack://application:,,,/TraderVsPmApp.Client;component/Images/Resize.png")]
    //public class TraderVsPmPlugin : IPlugin
    //{
    //    private readonly ITabDataSubscriber<TabData1EventArgs> subscriber;
    //    private TraderVsPmParameters pluginParameters;

    //    [ImportingConstructor]
    //    public TraderVsPmPlugin(ITabDataSubscriber<TabData1EventArgs> subscriber)
    //    {
    //        this.subscriber = subscriber;
    //        this.pluginParameters = new TraderVsPmParameters();
    //    }

    //    public string Category
    //    {
    //        get { return "Trading"; }
    //    }

    //    public IWidget CreateWidget()
    //    {
    //        return new TraderVsPmWidget(this.subscriber, this.pluginParameters);
    //    }

    //    public string Name
    //    {
    //        get { return "Trader Vs. Pm"; }
    //    }

    //    public IWidgetParameters Parameters
    //    {
    //        get;
    //        set;
    //    }

    //    public void ShowSettings()
    //    {
    //    }
    //}

    public class TraderVsPmPlugin : BasePlugin<TraderVsPmWidget, TraderVsPmParameters>
    {

    }
}
