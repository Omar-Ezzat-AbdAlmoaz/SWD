using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWDteam.Data
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string ? FirstName {  get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string? LastName { get; set;}
        public string Role { get; internal set; }

        //[PersonalData]
        //[Column(TypeName = "nvarchar(50)")]
        //public string? Role { get; set; }
    }
}
