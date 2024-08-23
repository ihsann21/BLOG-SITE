using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace blogSitesi.Models
{
    public class Contact
    {
       
        public string? FromEmailAdress { get; set; }

        
        public string? ToEmailAdress { get; set; }
        public int Subject { get; set; }
        public string? EmailBodyMessage { get; set; }
        [ValidateNever]
        public string? AttachmentUrl { get; set; }

    }
}