using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    /// <summary>
    /// Base Properties of the Model Classes.
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        /// Reference of the Facility where the row is created.
        /// </summary>       
        public int CreatedIn { get; set; }

        /// <summary>
        /// Creation date of the row.
        /// </summary>
        [Column(TypeName = "TIMESTAMP(0)")]
        [Display(Name = "Date created")]
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// Reference of the User who has created the row.
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Reference of the Facility where the row is modified.
        /// </summary>
        public int ModifiedIn { get; set; }

        /// <summary>
        /// Last modification date of the row.
        /// </summary>
        [Column(TypeName = "TIMESTAMP(0)")]
        [Display(Name = "Date modified")]
        public DateTime? DateModified { get; set; }

        /// <summary>
        /// Reference of the User who has last modified the row.
        /// </summary>
        public Guid? ModifiedBy { get; set; }

        /// <summary>
        /// Status of the row. It indicates the row is deleted or not.
        /// </summary>
        [Display(Name = "Row status")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Synced status of the row. It indicates the row is synced or not.
        /// </summary>
        public bool IsSynced { get; set; }
    }
}