using Pustovoy.Lopushok.Domain.Entities;
using Pustovoy.Lopushok.Infrastructure.Persistence;
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
using System.Xml.Serialization;

namespace Pustovoy.Lopushok.Presentation
{
    /// <summary>
    /// Логика взаимодействия для ProductItemEdit.xaml
    /// </summary>
    public partial class ProductItemEdit : Window
    {
        private enum AddOrEditFlag { Add = 0, Edit = 1 };

        private AddOrEditFlag WindowPurpose = AddOrEditFlag.Add;
        private Product? Item = null;

        public ProductItemEdit(Product? item)
        {
            InitializeComponent();

            Item = item;

            if(Item == null)
            {
                Item = new Product();
                this.Title = "Добавить продукт";
                WindowPurpose = AddOrEditFlag.Add;
            }
            else
            {
                SetTextBoxes();
                this.Title = "Редактировать продукт";
                WindowPurpose = AddOrEditFlag.Edit;
            }
        }

        private void SetTextBoxes()
        {
            TB_Title.Text = Item.Title;
            TB_ProductTypeId.Text = Convert.ToString(Item.ProductTypeId);
            TB_Description.Text = Item.Description;
            TB_ArticleNumber.Text = Item.ArticleNumber;
            TB_ImagePath.Text = Item.Image;
            TB_PersonCount.Text = Convert.ToString(Item.ProductionPersonCount);
            TB_WorkshopNumber.Text = Convert.ToString(Item.ProductionWorkshopNumber);
            TB_MinCostForAgent.Text = Convert.ToString(Item.MinCostForAgent);
        }

        private void SetProduct()
        {
            Item.Title = TB_Title.Text;
            Item.ProductTypeId = Convert.ToInt32(TB_ProductTypeId.Text);
            Item.ArticleNumber = TB_ArticleNumber.Text;
            Item.MinCostForAgent = Convert.ToDecimal(TB_MinCostForAgent.Text);

            Item.Description = (TB_Description.Text == "") ? null : TB_Description.Text;
            Item.ProductionPersonCount = (TB_PersonCount.Text == "") ? null : Convert.ToInt32(TB_PersonCount.Text);
            Item.ProductionWorkshopNumber = (TB_WorkshopNumber.Text == "") ? null : Convert.ToInt32(TB_WorkshopNumber.Text);
            Item.Image = (TB_ImagePath.Text == "") ? null : TB_ImagePath.Text;
        }

        private void AddProduct()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Products.Add(Item);
                context.SaveChanges();
            }
        }

        private void EditProduct()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Product product = context.Products
                    .Where(p => p.Id == Item.Id)
                    .First();

                product.Title = Item.Title;
                product.ProductTypeId = Item.ProductTypeId;
                product.ArticleNumber = Item.ArticleNumber;
                product.MinCostForAgent = Item.MinCostForAgent;
                product.Description = Item.Description;
                product.ProductionPersonCount = Item.ProductionPersonCount;
                product.ProductionWorkshopNumber = Item.ProductionWorkshopNumber;
                product.Image = Item.Image;

                context.SaveChanges();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TB_Title.Text == "" || TB_ProductTypeId.Text == "" || TB_ArticleNumber.Text == "" || TB_MinCostForAgent.Text == "")
            {
                MessageBox.Show("Invalid input");
                return;
            }

            if(WindowPurpose == AddOrEditFlag.Add)
            {
                SetProduct();
                AddProduct();
            }
            else
            {
                SetProduct();
                EditProduct();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
