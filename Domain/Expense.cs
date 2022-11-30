using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Expense : BaseModel
    {
        // <summary>
        /// Primary key of the table Expense.
        /// </summary>
        [Key]
        public int OID { get; set; }

        // <summary>
        /// Date when patient deceased.
        /// </summary>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Expense Date")]
        [Column(TypeName = "TIMESTAMP(0)")]
        public DateTime? ExpenseDate { get; set; }

        /// <summary>
        /// Amount of the Expense.
        /// </summary>
        [Required(ErrorMessage = "Amount is required.")]
        [Display(Name = "Amount")]
        public decimal ExpenseAmount { get; set; }

        /// <summary>
        ///  Forengn key, Primary key of the table Countries.
        /// </summary>
        [Required(ErrorMessage = "Category is required!")]
        [Display(Name = "Category ID")]
        public int CategoryID { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category Categories { get; set; }
    }
}