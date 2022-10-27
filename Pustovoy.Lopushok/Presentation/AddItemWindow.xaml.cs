using Pustovoy.Lopushok.Domain.Entities;
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
    /// Interaction logic for AddItemWindow.xaml
    /// </summary>
    public partial class AddItemWindow : Window
    {
        public Product item;
        public AddItemWindow()
        {
            InitializeComponent();

            item = new Product();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // TODO: add list of products, to multiple adds in this window

            if (TB_Title.Text == "" || TB_ProductTypeId.Text == "" || TB_ArticleNumber.Text == "" || TB_Cost.Text == "")
            {
                MessageBox.Show("Invalid input");
                return;
            }

            item.Title = TB_Title.Text;
            item.ProductTypeId = Convert.ToInt32(TB_ProductTypeId.Text);
            item.ArticleNumber = TB_ArticleNumber.Text;
            item.MinCostForAgent = Convert.ToDecimal(TB_Cost.Text);

            item.Description = (TB_Description.Text == "") ? null : TB_Description.Text;
            item.ProductionPersonCount = (TB_PersonCount.Text == "") ? null : Convert.ToInt32(TB_PersonCount.Text);
            item.ProductionWorkshopNumber = (TB_WorkshopNumber.Text == "") ? null : Convert.ToInt32(TB_WorkshopNumber.Text);
            item.Image = (TB_ImagePath.Text == "") ? null : TB_ImagePath.Text;
        }
    }
}
