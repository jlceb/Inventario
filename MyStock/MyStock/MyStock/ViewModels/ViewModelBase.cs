using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MyStock.ViewModels
{
    public abstract class ViewModelBase: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
