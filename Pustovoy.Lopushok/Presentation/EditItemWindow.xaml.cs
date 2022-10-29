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
    /// Interaction logic for EditItemWindow.xaml
    /// </summary>
    public partial class EditItemWindow : Window
    {
        private Product Item;
        public EditItemWindow(int itemId)
        {
            InitializeComponent();

            GetItem(itemId);
            SetTextBoxes();
        }

        private void GetItem(int itemId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Item = context.Products
                    .Where(p => p.Id == itemId)
                    .First();
            }
        }

        private void SetTextBoxes()
        {
            TB_Title.Text = Item.Title;
            TB_ProductTypeId.Text = Convert.ToString(Item.ProductTypeId);
            TB_Description.Text = Item.Description;
            TB_ArticleNumber.Text = Item.ArticleNumber;
            //TB_ImagePath.Text = Item.Image;
            TB_PersonCount.Text = Convert.ToString(Item.ProductionPersonCount);
            TB_WorkshopNumber.Text = Convert.ToString(Item.ProductionWorkshopNumber);
            TB_MinCostForAgent.Text = Convert.ToString(Item.MinCostForAgent);
        }

        private Product SetItemFromTextBoxes()
        {
            Product product = new Product();
            product.Title = TB_Title.Text;
            product.ProductTypeId = Convert.ToInt32(TB_ProductTypeId.Text);
            product.ArticleNumber = TB_ArticleNumber.Text;
            product.MinCostForAgent = Convert.ToDecimal(TB_MinCostForAgent.Text);

            product.Description = (TB_Description.Text == "") ? null : TB_Description.Text;
            product.ProductionPersonCount = (TB_PersonCount.Text == "") ? null : Convert.ToInt32(TB_PersonCount.Text);
            product.ProductionWorkshopNumber = (TB_WorkshopNumber.Text == "") ? null : Convert.ToInt32(TB_WorkshopNumber.Text);
            //product.Image = (TB_ImagePath.Text == "") ? null : TB_ImagePath.Text;

            return product;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (TB_Title.Text == "" || TB_ProductTypeId.Text == "" || TB_ArticleNumber.Text == "" || TB_MinCostForAgent.Text == "")
            {
                MessageBox.Show("Invalid input");
                return;
            }

            Product product = SetItemFromTextBoxes();

            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                Product item = context.Products
                    .Where(p => p.Id == Item.Id)
                    .First();

                item.Title = product.Title;
                item.ProductTypeId = product.ProductTypeId;
                item.ArticleNumber = product.ArticleNumber;
                item.MinCostForAgent = product.MinCostForAgent;

                item.Description = product.Description;
                item.ProductionPersonCount = product.ProductionPersonCount;
                item.ProductionWorkshopNumber = product.ProductionWorkshopNumber;
                //item.Image = product.Image;

                context.SaveChanges();
            }
        }
    }
}
