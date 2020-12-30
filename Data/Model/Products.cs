using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exercise.Data.Model
{
    public class Products
    {
        /// <summary>
        /// get / set id of table products
        /// </summary>
        /// <value>1</value>
        [Required]
        public int Id { get; set; }
        /// <summary>
        /// Get/Set Name of product
        /// </summary>
        /// <value>Barbie</value>

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// Get/Set Description of product
        /// </summary>
        /// <value>doll</value>
        [StringLength(100)]
        public string Description { get; set; }
        /// <summary>
        /// Get/Set Age Restriction of product
        /// </summary>
        /// <value>18</value>
        [Range(0, 100)]
        public int AgeRestriction { get; set; }
        /// <summary>
        /// Get/Set Company of product
        /// </summary>
        /// <value>Mattel</value>
        [StringLength(50)]
        public string Company { get; set; }
        /// <summary>
        /// Get/Set Price of product
        /// </summary>
        /// <value>25.99</value>
         [Column(TypeName = "decimal(18, 2)")]
        [Range(1.0, 1000.0)]
        public decimal Price { get; set; }
    }
}