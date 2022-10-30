using Microsoft.EntityFrameworkCore;
using Pustovoy.Lopushok.Domain.Entities;
using Pustovoy.Lopushok.Infrastucture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Pustovoy.Lopushok.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private List<Product> _products = new List<Product>();
        private List<string> _comboBoxSort = new List<string>();
        private List<string> _comboBoxFilter = new List<string>();
        private int _selectedIndex;

        public List<Product> Products
        {
            get => _products;
            set => Set(ref _products, value, nameof(Products));
        }
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => Set(ref _selectedIndex, value, nameof(SelectedIndex));
        }
        public List<string> ComboBoxSort
        {
            get => _comboBoxSort;
            set => Set(ref _comboBoxSort, value, nameof(ComboBoxSort));
        }
        public List<string> ComboBoxFilter
        {
            get => _comboBoxFilter;
            set => Set(ref _comboBoxFilter, value, nameof(ComboBoxFilter));
        }

        public MainWindowViewModel()
        {
            GetProducts();
            InitializeComboBoxes();
        }

        // called methods
        public void Search(string text)
        {
            if (text == "")
                GetProducts();
            else
                GetProductsWithText(text);
        }

        public void Sort(int index)
        {
            switch(index)
            {
                case 0:
                    Products = Products.OrderBy(p => p.Title).ToList();
                    break;
                case 1:
                    Products = Products.OrderByDescending(p => p.Title).ToList();
                    break;
                case 2:
                    Products = Products.OrderBy(p => p.ProductionWorkshopNumber).ToList();
                    break;
                case 3:
                    Products = Products.OrderByDescending(p => p.ProductionWorkshopNumber).ToList();
                    break;
                case 4:
                    Products = Products.OrderBy(p => p.MinCostForAgent).ToList();
                    break;
                case 5:
                    Products = Products.OrderByDescending(p => p.MinCostForAgent).ToList();
                    break;
            }
        }

        public void Filter(List<string> Types, int Index)
        {
            GetProducts();
            Products = Products.Where(p => p.ProductType.Title == Types[Index]).ToList();
        }

        public void DeleteItem()
        {
            var product = _products.ElementAt(_selectedIndex);

            DeleteProductMaterialsWithId(product.Id);

            DeleteProduct(product);

            GetProducts();
        }

        public void AddItem()
        {
            ProductWindow window = new ProductWindow(null);
            window.ShowDialog();
            GetProducts();
        }

        public void EditItem()
        {
            var product = _products.ElementAt(_selectedIndex);

            ProductWindow window = new ProductWindow(product);
            window.ShowDialog();
            GetProducts();
        }

        // local methods
        private void GetProducts()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Products = context.Products
                    .Include(pm => pm.ProductMaterials)
                    .ThenInclude(m => m.Material)
                    .Include(p => p.ProductType)
                    .ToList();
            }
        }

        private void GetProductsWithText(string text)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Products = context.Products
                    .Include(pm => pm.ProductMaterials)
                    .ThenInclude(m => m.Material)
                    .Include(p => p.ProductType)
                    .Where(t => t.Title.ToLower().Contains(text.ToLower()))
                    .ToList();
            }
        }

        private void InitializeComboBoxes()
        {
            ComboBoxSort.Add("Наименование >");
            ComboBoxSort.Add("Наименование <");
            ComboBoxSort.Add("Номер цеха >");
            ComboBoxSort.Add("Номер цеха <");
            ComboBoxSort.Add("Мин стоимость >");
            ComboBoxSort.Add("Мин стоимость <");

            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                foreach (var pt in context.ProductTypes.ToList())
                    ComboBoxFilter.Add(pt.Title);
            }
        }

        private void DeleteProductMaterialsWithId(int Id)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var productMaterials = context.ProductMaterials
                    .Where(p => p.ProductId == Id)
                    .ToList();

                if (productMaterials.Count == 0)
                    return;

                foreach (var pm in productMaterials)
                    context.ProductMaterials.Remove(pm);

                context.SaveChanges();
            }
        }

        private void DeleteProduct(Product product)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }
    }
}
