namespace Linedata.Framework.Client.TestWebBrowserWidget.Helpers
{
    using System;
    using System.Windows;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using MessageBox = Linedata.Framework.WidgetFrame.MessageBox.MessageBox;

    public static class WebBrowserMvvmHelper
    {
        public static readonly DependencyProperty SourceMvvmProperty =
        DependencyProperty.RegisterAttached("SourceMvvm", typeof(string), typeof(WebBrowserMvvmHelper), new UIPropertyMetadata(null, SourceMvvmPropertyChanged));

        public static readonly DependencyProperty BackProperty =
        DependencyProperty.RegisterAttached("Back", typeof(int), typeof(WebBrowserMvvmHelper), new UIPropertyMetadata(0, BackPropertyChanged));

        public static readonly DependencyProperty ForwardProperty =
        DependencyProperty.RegisterAttached("Forward", typeof(int), typeof(WebBrowserMvvmHelper), new UIPropertyMetadata(0, ForwardPropertyChanged));

        public static string GetSourceMvvm(DependencyObject obj)
        {
            return (string)obj.GetValue(SourceMvvmProperty);
        }
        
        public static string GetBack(DependencyObject obj)
        {
            return (string)obj.GetValue(BackProperty);
        }

        public static string GetForward(DependencyObject obj)
        {
            return (string)obj.GetValue(ForwardProperty);
        }

        public static void SetSourceMvvm(DependencyObject obj, string value)
        {
            obj.SetValue(SourceMvvmProperty, value);
        }

        public static void SetBack(DependencyObject obj, string value)
        {
            obj.SetValue(BackProperty, value);
        }

        public static void SetForward(DependencyObject obj, string value)
        {
            obj.SetValue(ForwardProperty, value);
        }

        public static void SourceMvvmPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.WebBrowser browser = o as System.Windows.Controls.WebBrowser;
            if (browser != null)
            {
                string uri = e.NewValue as string;
                if (!string.IsNullOrEmpty(uri))
                {
                    try
                    {
                        browser.Source = uri != null ? new Uri(uri) : null;
                    }
                    catch (UriFormatException ex)
                    {
                        MessageBox.Show(ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public static void BackPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.WebBrowser browser = o as System.Windows.Controls.WebBrowser;
            if (null != browser && browser.CanGoBack)
            {
                browser.GoBack();
            }
        }

        public static void ForwardPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            System.Windows.Controls.WebBrowser browser = o as System.Windows.Controls.WebBrowser;
            if (null != browser && browser.CanGoForward)
            {
                browser.GoForward();
            }
        }

        //public static bool VerifyUrlFormat(string url)
        //{
        // if (string.IsNullOrEmpty(url))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        CompareInfo cmpUrl = CultureInfo.InvariantCulture.CompareInfo;

        //      /*  if (!cmpUrl.IsPrefix(url, "http://") && !cmpUrl.IsPrefix(url, "https://"))
        //        {
        //            url = "http://" + url;
        //        }*/


        //        string regExUrlFormat = 

        //        // Regular expression from http://www.geekpedia.com/KB65_How-to-validate-an-URL-using-RegEx-in-Csharp.html 
        //        "(([a-zA-Z][0-9a-zA-Z+\\-\\.]*:)?/{0,2}[0-9a-zA-Z;/?:@&=+$\\.\\-_!~*'()%]+)?(#[0-9a-zA-Z;/?:@&=+$\\.\\-_!~*'()%]+)?";

        //        //Regular expression from http://www.osix.net/modules/article/?id=586 
        //         /*"^(https?://)"
        //        + "?(([0-9a-zA-Z_!~*'().&=+$%-]+: )?[0-9a-zA-Z_!~*'().&=+$%-]+@)?"
        //        + @"(([0-9]{1,3}\.){3}[0-9]{1,3}"
        //        + "|"
        //        + @"([0-9a-zA-Z_!~*'()-]+\.)*"
        //        + @"([0-9a-zA-Z][0-9a-zA-Z-]{0,61})?[0-9a-zA-Z]\."
        //        + "[a-zA-Z]{2,6})"
        //        + "(:[0-9]{1,4})?"
        //        + "((/?)|"
        //        + "(/[0-9a-zA-Z_!~*'().;?:@&=+$,%#-]+)+/?)$";*/
                               
        //        Regex regExUrlValidator = new Regex(regExUrlFormat);

        //        return regExUrlValidator.IsMatch(url);
        //    }
        //}

        public static bool VerifyUrlFormat(string url)
        {
            bool retval = true;

            if (string.IsNullOrEmpty(url))
            {
                retval = false;
            }
            else
            {
                string normalizedURL = NormalizeUrl(url);

                int lengthToLastPos = normalizedURL.Length - 1;

                int newLengthToLastPos = normalizedURL.IndexOf('?');
                if (newLengthToLastPos != -1)
                {
                    lengthToLastPos = newLengthToLastPos - 1;
                }

                if ((normalizedURL.Length >= 4 && normalizedURL.Substring(0, 4).ToLower().CompareTo("http") == 0) || (normalizedURL.Length >= 3 && normalizedURL.Substring(0, 3).ToLower().CompareTo("ftp") == 0))
                {
                    //if (normalizedURL.Substring(0, lengthToLastPos).IndexOf('.') == -1)
                    //{
                    //    retval = false;
                    //}
                    //else
                    //{
                    string regExExcludeFormat = "['\\s<>{}\\|\\[\\]]";
                    Regex regExExcludeValidator = new Regex(regExExcludeFormat);
                    if (regExExcludeValidator.IsMatch(normalizedURL.Substring(0, lengthToLastPos)))
                    {
                        retval = false;
                    }
                    else
                    {
                        string regExUrlFormat =
                            //// Regular expression from http://www.geekpedia.com/KB65_How-to-validate-an-URL-using-RegEx-in-Csharp.html 
                            "(([a-zA-Z][0-9a-zA-Z+\\-\\.]*:)?/{0,2}[0-9a-zA-Z;/?:@&=+$\\.\\-_!~*'()%]+)?(#[0-9a-zA-Z;/?:@&=+$\\.\\-_!~*'()%]+)?";

                        //// Regular expression from http://www.osix.net/modules/article/?id=586
                        /*
                                               "^(https?://)"
                                                + "?(([0-9a-zA-Z_!~*'().&=+$%-]+: )?[0-9a-zA-Z_!~*'().&=+$%-]+@)?"
                                                + @"(([0-9]{1,3}\.){3}[0-9]{1,3}"
                                                + "|"
                                                + @"([0-9a-zA-Z_!~*'()-]+\.)*"
                                                + @"([0-9a-zA-Z][0-9a-zA-Z-]{0,61})?[0-9a-zA-Z]\."
                                                + "[a-z]{2,6})"
                                                + "(:[0-9]{1,4})?"
                                                + "((/?)|"
                                                + "(/[0-9a-zA-Z_!~*'().;?:@&=+$,%#-]+)+/?)$";
                         */

                        Regex regExUrlValidator = new Regex(regExUrlFormat);

                        retval = regExUrlValidator.IsMatch(normalizedURL);
                    }
                    //}
                }
                else if (normalizedURL.Length >= 2 && normalizedURL.Substring(0, 2).CompareTo("\\\\") == 0)
                {
                }
                else if (normalizedURL.Length >= 3 && normalizedURL.Substring(1, 2).CompareTo(":\\") == 0)
                {
                }
                else
                {
                    // unsupported format
                    retval = false;
                }
            }

            return retval;
        }



        public static string NormalizeUrl(string url)
        {
            string normalizedUrl;
            CompareInfo cmpUrl = CultureInfo.InvariantCulture.CompareInfo;

            if (!string.IsNullOrEmpty(url) && 
                !cmpUrl.IsPrefix(url, "file:") && 
                !cmpUrl.IsPrefix(url, "\\") && 
                !cmpUrl.IsPrefix(url, "ftp:") && 
                !cmpUrl.IsPrefix(url, "http:") && 
                !cmpUrl.IsPrefix(url, "https:") && 
                (url.Length > 1 && url[1] != ':'))
            {
                normalizedUrl = "http://" + url;
            }
            else
            {
                normalizedUrl = url;
            }

            return normalizedUrl;
        }
    }
}
