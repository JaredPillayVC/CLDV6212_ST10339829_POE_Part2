using Microsoft.Azure.Cosmos.Table;
using System;
using System.ComponentModel.DataAnnotations;

namespace CLDV6212_ST10339829_POE.Models
{
    public class Customer : TableEntity
    {
        [Required]
        public int? CID { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public Customer()
        {
            PartitionKey = "Customer"; 
        }

        public void SetRowKey()
        {
            if (!CID.HasValue)
            {
                RowKey = Guid.NewGuid().ToString();
            }
            else
            {
                RowKey = CID.ToString();
            }
        }
    }
}
