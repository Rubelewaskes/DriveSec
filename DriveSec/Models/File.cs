﻿using System;
using System.Collections.Generic;

namespace DriveSec.Models;

public partial class File
{
    public int FileId { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public bool VirusAvailiability { get; set; }

    public string? VirusDescription { get; set; }

    public int UploaderId { get; set; }

    public int FolderId { get; set; }

    public virtual ICollection<ChangeHistory> ChangeHistories { get; set; } = new List<ChangeHistory>();

    public virtual Folder Folder { get; set; } = null!;

    public virtual User Uploader { get; set; } = null!;
}
