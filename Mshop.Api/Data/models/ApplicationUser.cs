﻿using Microsoft.AspNetCore.Identity;

namespace Mshop.Api.Data.models
{
    public enum ApplicationUserGender
    {
        MALE,
        FEMALE
    }
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public ApplicationUserGender Gender { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
