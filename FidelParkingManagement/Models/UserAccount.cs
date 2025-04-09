using System;
using System.Collections.Generic;

namespace FidelParkingManagement.Models;

public partial class UserAccount
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Role { get; set; }

    public string? ProfilePhotoUrl { get; set; }
}
