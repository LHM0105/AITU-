namespace AITU网站.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Collection")]
    public partial class Collection
    {
        [Key]
        [StringLength(13)]
        public string CollectId { get; set; }

        [Required]
        [StringLength(11)]
        public string UserId { get; set; }

        [Required]
        [StringLength(13)]
        public string ImgID { get; set; }

        public virtual Image Image { get; set; }
    }
}
