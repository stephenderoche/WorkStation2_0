using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using System.Xml.Serialization;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace AccountTreeCashViewer
{
    public class Parent : INotifyPropertyChanged {
        string _short_name;
        public string Short_name
        {
            get { return _short_name; }
            set {
                if (Short_name == value)
                    return;
                _short_name = value;
                OnPropertyChanged("Short_name");
            }
        }

        int _account_id;
        public int Account_id
        {
            get { return _account_id; }
            set {
                if (ReferenceEquals(Account_id, value))
                    return;
                _account_id = value;
                OnPropertyChanged("Account_id");
            }
        }

        string _image_path;
        public string ImagePath
        {
            get
            {
               return _image_path;
            }
            set
            {
                if (ImagePath == value)
                    return;
                _image_path = value;
                OnPropertyChanged("Image_path");
            }
        }

      

      

        public override string ToString() {
            return Short_name;
        }

        protected void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Childlist : Parent 
    {
        public ObservableCollection<Child> Child { get; set; }
    }

    public class Child : Parent
    {

    }
}