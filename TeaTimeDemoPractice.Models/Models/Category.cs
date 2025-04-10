using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TeaTimeDemoPractice.Models
{
    public class Category
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("類別名稱")]
        public string name { get; set; }

        [DisplayName("顯示順序")]
        [Range(1, 200, ErrorMessage= "輸入範圍應該要在 0 - 200 之間")]
        public int DisplayOrder { get; set; }
    }
}
