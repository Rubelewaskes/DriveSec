using System;
using System.Collections.Generic;

namespace DriveSec.Models;

public partial class Folder
{
    public int FolderId { get; set; }

    public string FolderWay { get; set; }

    public string FolderName { get; set; } = null!;

    public string? FolderDescription { get; set; }

    public DateTime CreationDate { get; set; }

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<UsersFolder> UsersFolders { get; set; } = new List<UsersFolder>();
}
