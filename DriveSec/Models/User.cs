using System;
using System.Collections.Generic;

namespace DriveSec.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<ChangeHistory> ChangeHistories { get; set; } = new List<ChangeHistory>();

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<UsersFolder> UsersFolders { get; set; } = new List<UsersFolder>();

    public virtual ICollection<UsersMac> UsersMacs { get; set; } = new List<UsersMac>();
}
