namespace Linedata.Framework.Client.TestWebBrowserWidget.ViewModel
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Linedata.Framework.WidgetFrame.MvvmFoundation;
    using Linedata.Framework.Client.TestWebBrowserWidget.Plugin;
    using Linedata.Framework.Client.TestWebBrowserWidget.Helpers;

    public class WebBrowserViewModel : ObservableObject
    {
        private string urlForTitle;
        
        private ICommand processWebPageCommand;
        private ICommand backCommand;
        private ICommand forwardCommand;

        private IDictionary<string, string> titles;

        private Brush applicationBackgroundBrush;
        private Brush highlightBackgroundBrush;
        private Brush controlBackgroundBrush;
        private Brush controlForegroundBrush;
        private Brush borderBrush;
        private Brush controlBrush;

        private int back;
        private int forward;
        private bool isLoading;
        private bool? isBackOrForwardAction;

        public WebBrowserViewModel(WebBrowserParameters parameters)
        {
            this.Parameters = parameters;
            this.back = 0;
            this.forward = 0;
            this.isBackOrForwardAction = null;
            this.SetLoadingState(false);
            this.titles = new Dictionary<string, string>();
            this.RaisePropertyChanged("BrowserVisibility");
        }

        public WebBrowserParameters Parameters
        {
            get;
            set;
        }

        public string UrlForTitle
        {
            get
            {
                    return this.urlForTitle;
            }

            set
            {
                this.urlForTitle = value;
                this.RaisePropertyChanged("UrlForTitle");
            }
        }

        public Visibility BrowserVisibility
        {
            get
            {
                if (!WebBrowserMvvmHelper.VerifyUrlFormat(this.Parameters.URL))
                {
                    return Visibility.Hidden;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        public ICommand ProcessWebPageCommand
        {
            get
            {
                if (this.processWebPageCommand == null)
                {
                    this.processWebPageCommand = new RelayCommand(this.ProcessWebPage, this.CanProcessWebPage);
                }

                return this.processWebPageCommand;
            }
        }

        public ICommand BackCommand
        {
            get
            {
                if (this.backCommand == null)
                {
                    this.backCommand = new RelayCommand(this.GoBack, this.CanGoBack);
                }

                return this.backCommand;
            }
        }

        public ICommand ForwardCommand
        {
            get
            {
                if (this.forwardCommand == null)
                {
                    this.forwardCommand = new RelayCommand(this.GoForward, this.CanGoForward);
                }

                return this.forwardCommand;
            }
        }

        public int Back
        {
            get
            {
                return this.back;
            }

            set
            {
                this.back = value;
                this.RaisePropertyChanged("Back");
            }
        }

        public int Forward
        {
            get
            {
                return this.forward;
            }

            set
            {
                this.forward = value;
                this.RaisePropertyChanged("Forward");
            }
        }

        public Visibility LoadingVisibility
        {
            get 
            {
                if (this.isLoading)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
        }

        public Brush ApplicationBackgroundBrush
        {
            get
            {
                return this.applicationBackgroundBrush;
            }

            set
            {
                this.applicationBackgroundBrush = value;
                this.RaisePropertyChanged("ApplicationBackgroundBrush");
            }
        }

        public Brush HighlightBackgroundBrush
        {
            get
            {
                return this.highlightBackgroundBrush;
            }

            set
            {
                this.highlightBackgroundBrush = value;
                this.RaisePropertyChanged("HighlightBackgroundBrush");
            }
        }

        public Brush ControlBackgroundBrush
        {
            get
            {
                return this.controlBackgroundBrush;
            }

            set
            {
                this.controlBackgroundBrush = value;
                this.RaisePropertyChanged("ControlBackgroundBrush");
            }
        }

        public Brush ControlForegroundBrush
        {
            get
            {
                return this.controlForegroundBrush;
            }

            set
            {
                this.controlForegroundBrush = value;
                this.RaisePropertyChanged("ControlForegroundBrush");
            }
        }

        public Brush BorderBrush
        {
            get
            {
                return this.borderBrush;
            }

            set
            {
                this.borderBrush = value;
                this.RaisePropertyChanged("BorderBrush");
            }
        }

        public Brush ControlBrush 
        {
            get 
            {
                return this.controlBrush;
            }
            
            set
            {
                this.controlBrush = value;
                this.RaisePropertyChanged("ControlBrush");
            }
        }

        public void SetLoadingState(bool loading)
        {
            this.isLoading = loading;
            this.RaisePropertyChanged("LoadingVisibility");
        }

        public void UpdateTitle()
        {
            if (null != this.titles)
            {
                if (!string.IsNullOrEmpty(this.urlForTitle))
                {
                    if ((this.isBackOrForwardAction.HasValue && this.isBackOrForwardAction.Value) || !this.isBackOrForwardAction.HasValue)
                    {
                        if (this.titles.ContainsKey(this.urlForTitle))
                        {
                            this.Parameters.Title = this.titles[this.urlForTitle];
                        }
                        else
                        {
                            this.titles.Add(this.urlForTitle, this.Parameters.Title);
                        }
                    }
                    else
                    {
                        if (this.titles.ContainsKey(this.urlForTitle))
                        {
                            this.titles.Remove(this.urlForTitle);
                        }

                        this.titles.Add(this.urlForTitle, this.Parameters.Title);
                    }
                }
            }

            this.isBackOrForwardAction = null;
        }

        private void ProcessWebPage()
        {
            this.isBackOrForwardAction = false;
           
            this.Parameters.URL = WebBrowserMvvmHelper.NormalizeUrl(this.UrlForTitle);
          
            this.RaisePropertyChanged("BrowserVisibility");
        }

        private bool CanProcessWebPage()
        {
            return WebBrowserMvvmHelper.VerifyUrlFormat(this.UrlForTitle);
        }

        private bool CanGoForward()
        {
            return !this.isLoading;
        }

        private void GoForward()
        {
            this.isBackOrForwardAction = true;
            this.Forward = this.forward + 1;
        }

        private bool CanGoBack()
        {
            return !this.isLoading;
        }

        private void GoBack()
        {
            this.isBackOrForwardAction = true;
            this.Back = this.back + 1;
        }
    }
}
