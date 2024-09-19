using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Models
{
    public class ClientModel
    {
        [Required(ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "LicenceKey is required")]
        public Guid LicenceKey { get; set; }

        [Required(ErrorMessage = "ClientName is required")]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "LicenceStartDate is required")]
        public DateTime LicenceStartDate { get; set; }

        [Required(ErrorMessage = "LicenceEndDate is required")]
        public DateTime LicenceEndDate { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }
}
