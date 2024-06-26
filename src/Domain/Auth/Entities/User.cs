﻿using Domain.Credits.Entities;
using Domain.Stories.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain.Auth.Entities;

public class User : IdentityUser<int> {
    [ProtectedPersonalData]
    public override string? UserName
    {
        get => base.Email;
        set
        {
            base.Email = value;
            base.UserName = value;
        }
    }

    [ProtectedPersonalData]
    public override string? Email
    {
        get => base.Email;
        set
        {
            base.Email = value;
            base.UserName = value;
        }
    }
    
    public string? DisplayName { get; set; }

    public virtual IList<Story> Stories { get; set; }
    public virtual IList<CreditEvent> CreditEvents { get; set; }
}