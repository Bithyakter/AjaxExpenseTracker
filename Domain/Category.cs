using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain
{
    public class Category : BaseModel
    {
        // <summary>
        /// Primary key of the table Category.
        /// </summary>
        [Key]
        public int OID { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        [Required(ErrorMessage = "Category name is required!")]
        [StringLength(90)]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}