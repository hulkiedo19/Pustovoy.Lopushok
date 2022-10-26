using Microsoft.EntityFrameworkCore;
using Pustovoy.Lopushok.Domain.Entities;
using Pustovoy.Lopushok.Infrastucture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pustovoy.Lopushok.Presentation.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public List<Product> Products { get; set; }

        public MainWindowViewModel()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                Products = context.Products
                    .Include(p => p.ProductType)
                    .ToList();
            }
        }
    }
}
