using System;
using System.Collections.Generic;

namespace DriveSec.Models;

public partial class UsersFolder
{
    public int UsersFolderId { get; set; }
    public int UserId { get; set; }

    public int FolderId { get; set; }

    public string Role { get; set; } = null!;

    public virtual Folder Folder { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
