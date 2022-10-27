using Microsoft.EntityFrameworkCore;
using Pustovoy.Lopushok.Domain.Entities;
using Pustovoy.Lopushok.Infrastucture.Persistence;
using Pustovoy.Lopushok.Presentation.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pustovoy.Lopushok.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<Product> _products;
        private string _searchText;
        private int _sortIndex;
        private int _filterIndex;

        public List<Product> Products
        {
            get => _products;
            set => Set(ref _products, value, nameof(Products));
        }
        public string SearchText
        {
            get => _searchText;
            set => Set(ref _searchText, value, nameof(SearchText));
        }
        public int SortIndex
        {
            get => _sortIndex;
            set => Set(ref _sortIndex, value, nameof(SortIndex));
        }
        public int FilterIndex
        {
            get => _filterIndex;
            set => Set(ref _filterIndex, value, nameof(FilterIndex));
        }

        public ICommand Search => new SearchCommand(this);

        public MainWindowViewModel()
        {
            _products = new List<Product>();
            _searchText = "";
            GetProducts();            
        }

        private void GetProducts()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Products = context.Products
                    .Include(p => p.ProductType)
                    .ToList();
            }
        }
    }
}
