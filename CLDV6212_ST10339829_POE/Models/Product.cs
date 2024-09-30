using Microsoft.Azure.Cosmos.Table;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace CLDV6212_ST10339829_POE.Models
{
    public class Product : TableEntity
    {
        [Required]
        public int? PID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public Product()
        {
            PartitionKey = "Product";
        }
        public void SetRowKey()
        {
            if (!PID.HasValue)
            {
                RowKey = Guid.NewGuid().ToString();
            }
            else
            {
                RowKey = PID.ToString();
            }
        }
    }
}
