using Microsoft.AspNetCore.Identity;

namespace BlazorServerAppBase.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
        public string CompanyName { get; set; }
        public string Department { get; set; }
        public string HangulName { get; set; }
    }

}
