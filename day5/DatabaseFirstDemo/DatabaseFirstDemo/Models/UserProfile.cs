using System;
using System.Collections.Generic;

namespace DatabaseFirstDemo.Models;

public partial class UserProfile
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public virtual User User { get; set; } = null!;
}
