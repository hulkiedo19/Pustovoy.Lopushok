using Microsoft.EntityFrameworkCore;
using Pustovoy.Lopushok.Infrastucture.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pustovoy.Lopushok.Domain.Entities
{
    public partial class Product
    {
        private string? _image;
        private decimal _minCostForAgent;

        public Product()
        {
            ProductCostHistories = new HashSet<ProductCostHistory>();
            ProductMaterials = new HashSet<ProductMaterial>();
            ProductSales = new HashSet<ProductSale>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int? ProductTypeId { get; set; }
        public string ArticleNumber { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image
        {
            get => (_image == string.Empty) || (_image == null)
                ? $"..\\Resources\\picture.png"
                : $"..\\Resources{_image}";
            set => _image = value;
        }
        public int? ProductionPersonCount { get; set; }
        public int? ProductionWorkshopNumber { get; set; }
        public decimal MinCostForAgent 
        {
            get => _minCostForAgent;
            set => _minCostForAgent = value;
        }

        public string? Materials
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                List<ProductMaterial> productMaterials = new ApplicationDbContext().ProductMaterials
                    .Include(m => m.Material)
                    .Where(p => p.ProductId == this.Id)
                    .ToList();

                if (productMaterials.Count == 0)
                    return "Материалов нет";

                sb.Append("Материалы: ");

                foreach (ProductMaterial pm in productMaterials)
                    sb.Append($"{pm.Material.Title}, ");

                sb.Remove(sb.Length - 2, 2);

                return sb.ToString();
            }
        }
        public decimal Cost
        {
            get
            {
                List<ProductMaterial> productMaterials = new ApplicationDbContext().ProductMaterials
                    .Include(m => m.Material)
                    .Where(p => p.ProductId == this.Id)
                    .ToList();

                if (productMaterials.Count == 0)
                    return _minCostForAgent;

                decimal cost = 0;
                foreach (ProductMaterial pm in productMaterials)
                    cost += pm.Material.Cost;

                return cost;
            }
        }

        public virtual ProductType? ProductType { get; set; }
        public virtual ICollection<ProductCostHistory> ProductCostHistories { get; set; }
        public virtual ICollection<ProductMaterial> ProductMaterials { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }
    }
}
