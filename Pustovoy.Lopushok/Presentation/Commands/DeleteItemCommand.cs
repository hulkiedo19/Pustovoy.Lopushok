using Microsoft.EntityFrameworkCore;
using Pustovoy.Lopushok.Domain.Entities;
using Pustovoy.Lopushok.Infrastucture.Persistence;
using Pustovoy.Lopushok.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pustovoy.Lopushok.Presentation.Commands
{
    public class DeleteItemCommand : Command
    {
        private readonly MainWindowViewModel _viewModel;

        public DeleteItemCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            Product item = _viewModel.Products.ElementAt(_viewModel.SelectedIndex);

            DeleteProductMaterialsWithId(item.Id);

            DeleteProductItem(item);

            UpdateList();
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

        private void DeleteProductItem(Product item)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Products.Remove(item);
                context.SaveChanges();
            }
        }

        private void UpdateList()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                _viewModel.Products = context.Products
                    .Include(p => p.ProductType)
                    .ToList();
            }
        }
    }
}
