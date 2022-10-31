using Pustovoy.Lopushok.Domain.Entities;
using Pustovoy.Lopushok.Infrastucture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pustovoy.Lopushok.Presentation
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private bool IsChanged = false;
        private Product Item;

        public ProductWindow(Product item)
        {
            InitializeComponent();
            Item = item;

            SetComboBoxProductType();
            SetListBoxMaterials();
            SetTextBoxes();
        }

        private void SetTextBoxes()
        {
            TB_Title.Text = Item.Title;
            //TB_ProductTypeId.Text = Convert.ToString(Item.ProductTypeId);
            TB_Description.Text = Item.Description;
            TB_ArticleNumber.Text = Item.ArticleNumber;
            TB_ImagePath.Text = Item.Image;
            TB_PersonCount.Text = Convert.ToString(Item.ProductionPersonCount);
            TB_WorkshopNumber.Text = Convert.ToString(Item.ProductionWorkshopNumber);
            TB_MinCostForAgent.Text = Convert.ToString(Item.MinCostForAgent);
        }

        private void SetComboBoxProductType()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                List<ProductType> productTypes = context.ProductTypes.ToList();
                ProductType selectedProductType = context.ProductTypes.Where(p => p.Id == Item.ProductTypeId).First();
                int selectedIndex = productTypes.FindIndex(p => p.Id == selectedProductType.Id);

                foreach (var productType in productTypes)
                    CB_ProductTypes.Items.Add(productType.Title);

                CB_ProductTypes.SelectedIndex = selectedIndex;
            }
        }

        private void SetListBoxMaterials()
        {
            List<string> MaterialItems = new List<string>();
            List<Material> materials = new ApplicationDbContext().Materials.ToList();

            foreach (Material material in materials)
                MaterialItems.Add(material.Title);

            LB_Materials.ItemsSource = MaterialItems;
        }

        private Product SetProduct()
        {
            Product product = new Product();

            var productTypeValue = CB_ProductTypes.Items[CB_ProductTypes.SelectedIndex].ToString();
            product.ProductTypeId = new ApplicationDbContext().ProductTypes.Where(p => p.Title.Equals(productTypeValue)).First().Id;

            product.Title = TB_Title.Text;
            product.ArticleNumber = TB_ArticleNumber.Text;
            product.MinCostForAgent = Convert.ToDecimal(TB_MinCostForAgent.Text);

            product.Description = (TB_Description.Text == "") ? null : TB_Description.Text;
            product.ProductionPersonCount = (TB_PersonCount.Text == "") ? null : Convert.ToInt32(TB_PersonCount.Text);
            product.ProductionWorkshopNumber = (TB_WorkshopNumber.Text == "") ? null : Convert.ToInt32(TB_WorkshopNumber.Text);
            product.Image = (TB_ImagePath.Text == "") ? null : TB_ImagePath.Text;

            return product;
        }

        private void AddProduct(Product product)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        private void EditProduct(Product item)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Product product = context.Products
                    .Where(p => p.Id == Item.Id)
                    .First();

                product.Title = item.Title;
                product.ProductTypeId = item.ProductTypeId;
                product.ArticleNumber = item.ArticleNumber;
                product.MinCostForAgent = item.MinCostForAgent;
                product.Description = item.Description;
                product.ProductionPersonCount = item.ProductionPersonCount;
                product.ProductionWorkshopNumber = item.ProductionWorkshopNumber;
                product.Image = item.Image;

                context.SaveChanges();
            }
        }

        // clicks event methods
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = IsChanged;
            this.Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (TB_Title.Text == "" || TB_ArticleNumber.Text == "" || TB_MinCostForAgent.Text == "" || CB_ProductTypes.SelectedIndex == -1)
            {
                MessageBox.Show("Invalid input");
                return;
            }

            EditProduct(SetProduct());
            IsChanged = true;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (TB_Title.Text == "" || TB_ArticleNumber.Text == "" || TB_MinCostForAgent.Text == "" || CB_ProductTypes.SelectedIndex == -1)
            {
                MessageBox.Show("Invalid input");
                return;
            }

            AddProduct(SetProduct());
            IsChanged = true;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                // Delete all ProductMaterials items with Product.Id
                var productMaterials = context.ProductMaterials
                    .Where(p => p.ProductId == Item.Id)
                    .ToList();

                if (productMaterials.Count > 0)
                {
                    foreach (var pm in productMaterials)
                        context.ProductMaterials.Remove(pm);

                    context.SaveChanges();
                }

                // Delete product item
                context.Products.Remove(Item);
                context.SaveChanges();
            }

            IsChanged = true;
        }
    }
}
