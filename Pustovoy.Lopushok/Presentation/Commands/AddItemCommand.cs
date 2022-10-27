using Microsoft.EntityFrameworkCore;
using Pustovoy.Lopushok.Domain.Entities;
using Pustovoy.Lopushok.Infrastucture.Persistence;
using Pustovoy.Lopushok.Presentation.ViewModels;
using System.Linq;

namespace Pustovoy.Lopushok.Presentation.Commands
{
    public class AddItemCommand : Command
    {
        private readonly MainWindowViewModel _viewModel;

        public AddItemCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            AddItemWindow window = new AddItemWindow();

            if (window.ShowDialog() == true)
            {
                AddItemWindow content = window.Content as AddItemWindow;
                Product item = content.item;

                AddProductItem(item);
                UpdateList();
            }
        }

        private void AddProductItem(Product item)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Products.Add(item);
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
