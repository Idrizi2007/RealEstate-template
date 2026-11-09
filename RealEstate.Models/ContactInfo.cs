using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Models
{
    public class ContactInfo
    {
        [Key]
        public int Id { get; set; }
        [Phone]
        [StringLength(30)]
        [DisplayName("Numri i telefonit")]
        public string? PhoneNumber { get; set; }
        [Phone]
        [StringLength(30)]
        [DisplayName("Numri i WhatsApp")]
        public string? WhatsAppNumber { get; set; }
        [EmailAddress]
        [StringLength(250)]
        [DisplayName("Email")]
        public string? Email { get; set; }
        [Url]
        [StringLength(350)]
        [DisplayName("Lokacioni Google Maps")]
        public string? Address { get; set; }
        [StringLength(100)]
        [DisplayName("Orari i punes")]
        public string? OpeningHours { get; set; }
        [Url]
        [StringLength(150)]
        [DisplayName("Instagram")]
        public string? Instagram { get; set; }
        [Url]
        [StringLength(150)]
        [DisplayName("Facebook")]
        public string? Facebook { get; set; }
        [Url]
        [StringLength(150)]
        [DisplayName("TikTok")]
        public string? TikTok { get; set; }
    }
}
