using Pustovoy.Lopushok.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            EditItemWindow window = new EditItemWindow();

            if(window.ShowDialog() == true)
            {

            }


        }
    }
}
