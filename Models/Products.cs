using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CrudOperation.Models
{
    public class Products
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsActive { get; set; }

        public Products()
        {
           IsActive= true; // Set IsDeleted to true (active) by default
        }
    }
    public class EFCodeFirstEntities: DbContext
    {
        public DbSet<Products> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}