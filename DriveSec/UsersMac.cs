using System;
using System.Collections.Generic;

namespace DriveSec;

public partial class UsersMac
{
    public int UserMacId { get; set; }

    public int UserId { get; set; }

    public string Mac { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
