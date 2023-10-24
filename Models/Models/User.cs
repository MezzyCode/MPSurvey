using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Model.Models;

public partial class User : IdentityUser
{

    //public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string Role { get; set; } = null!;


    public string ClientID { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedTime { get; set; }

    public string LastModifiedBy { get; set; } = null!;

    public DateTime LastModifiedTime { get; set; }

    public byte[] TimeStatus { get; set; } = null!;

    public int RowStatus { get; set; }
}
