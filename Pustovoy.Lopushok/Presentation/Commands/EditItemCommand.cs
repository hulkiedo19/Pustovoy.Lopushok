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
    public class EditItemCommand : Command
    {
        private readonly MainWindowViewModel _viewModel;
        
        public EditItemCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            EditItemWindow window = new EditItemWindow(_viewModel.Products[_viewModel.SelectedIndex].Id);

            if(window.ShowDialog() == true)
            {
                UpdateList();
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
