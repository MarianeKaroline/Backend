using System;
using System.Collections.Generic;
using System.Text;

namespace SingleExperience.Services.ClientServices.Models
{
    class SignInModel
    {
        public long SignInId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
