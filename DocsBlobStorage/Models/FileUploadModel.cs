using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace DocsBlobStorage.Models
{
    public class FileUploadModel
    {
        [Required]
        public IBrowserFile File { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
