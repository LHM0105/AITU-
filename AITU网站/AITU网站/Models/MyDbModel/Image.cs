namespace AITU网站.Models.MyDbModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Image")]
    public partial class Image
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Image()
        {
            Collection = new HashSet<Collection>();
        }

        [Key]
        [StringLength(13)]
        public string ImgId { get; set; }

        [Required]
        [StringLength(50)]
        public string ImgName { get; set; }

        [Column(TypeName = "image")]
        [Required]
        public byte[] ImgContent { get; set; }

        [Required]
        [StringLength(11)]
        public string UserId { get; set; }

        public int DownNum { get; set; }

        public int ImgType { get; set; }

        public int CollectNum { get; set; }

        public int? ImgWidth { get; set; }

        public int? ImgHight { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Collection> Collection { get; set; }
    }
}
