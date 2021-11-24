using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace WebApplicationData.Enties
{
    public class Role : IdentityRole<String>
    {
        public string Description { get; set; }
    }
}
