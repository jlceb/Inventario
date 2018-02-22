using MyStock.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MyStock.ViewModels
{
    public class ProductsViewModel : ViewModelBase
    {
        List<Product> listProducts;

        public ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get
            {
                return _products;
            }
            set
            {
                _products = value;
                this.Notify("Products");
            }
        }

        public ProductsViewModel(List<Product> lproducts)
        {
            this.listProducts = lproducts;
            Products = new ObservableCollection<Product>(listProducts.OrderBy(x => x.Description));
        }
    }
}
