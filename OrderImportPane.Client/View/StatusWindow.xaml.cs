using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrderImportPane.Client
{
    /// <summary>
    /// Interaction logic for StatusWindow.xaml
    /// </summary>
    public partial class StatusWindow
    {
        public bool ImportComplete;

        public string StatusText
        {
            get { return (string)GetValue(StatusTextProperty); }
            set { SetValue(StatusTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StatusText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusTextProperty =
            DependencyProperty.Register("StatusText", typeof(string), typeof(StatusWindow), new UIPropertyMetadata(string.Empty));

        public StatusWindow()
        {
            InitializeComponent();
        }

        private void statusWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseIfComplete();
        }

        private void statusWindow_StylusButtonDown(object sender, StylusButtonEventArgs e)
        {
            CloseIfComplete();
        }

        private void statusWindow_TextInput(object sender, TextCompositionEventArgs e)
        {
            CloseIfComplete();
        }

        private void CloseIfComplete()
        {
            if (ImportComplete)
                Close();
        }
    }
}