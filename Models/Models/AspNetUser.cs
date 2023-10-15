using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public string CreatedByName { get; set; } = null!;

    public string LastModifiedByName { get; set; } = null!;

    public string CreatedByEmployeeId { get; set; } = null!;

    public string CreatedByEmployeeNik { get; set; } = null!;

    public string LastModifiedByEmployeeId { get; set; } = null!;

    public string LastModifiedByEmployeeNik { get; set; } = null!;

    public DateTime CreatedTime { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public DateTime? LastModifiedTime { get; set; }

    public byte[] TimeStatus { get; set; } = null!;

    public int RowStatus { get; set; }

    public string ClientId { get; set; } = null!;

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
