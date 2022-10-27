using Microsoft.EntityFrameworkCore;
using Pustovoy.Lopushok.Infrastucture.Persistence;
using Pustovoy.Lopushok.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustovoy.Lopushok.Presentation.Commands
{
    public class SearchCommand : Command
    {
        public readonly MainWindowViewModel _viewModel;

        public SearchCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if (_viewModel.SearchText == "")
                FindAllItems();
            else
                FindItemsWithText();
        }

        private void FindAllItems()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                _viewModel.Products = context.Products
                    .Include(p => p.ProductType)
                    .ToList();
            }
        }

        private void FindItemsWithText()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                _viewModel.Products = context.Products
                    .Include(p => p.ProductType)
                    .Where(t => t.Title.ToLower().Contains(_viewModel.SearchText.ToLower()))
                    .ToList();
            }
        }
    }
}
