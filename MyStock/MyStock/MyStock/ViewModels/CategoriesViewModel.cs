using MyStock.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MyStock.ViewModels
{
    public class CategoriesViewModel
    {
        public ObservableCollection<Category> Categories { get; set; }

        public CategoriesViewModel()
        {
            LoadCategories();
        }

        void LoadCategories()
        {

        }
    }
}
