using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.Contacts
{
    public class Contact
    {
        [Key]
        public int Id {set; get;}

        [Column(TypeName = "nvarchar")]
        [StringLength(20)]
        [Required]
        [Display(Name ="Họ tên")]
        public string Fullname {set; get;}
        [Required]
        [StringLength(100)]
        [Display(Name ="Email")]
        [EmailAddress(ErrorMessage ="sai dạng")]
        public string Email {set; get;}

        public DateTime DateSent {set; get;}

        [Display(Name ="Nội dung")]
        public string Message {set; get;}
        [StringLength(10)]
        [Display(Name ="SĐT")]
        [Phone(ErrorMessage ="sai dạng")]
        public string Phone {set; get;}
    }
}