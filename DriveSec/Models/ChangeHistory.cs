using System;
using System.Collections.Generic;

namespace DriveSec.Models;

public partial class ChangeHistory
{
    public int ChangeHistoryId { get; set; }

    public int FileId { get; set; }

    public int UserId { get; set; }

    public string? ChangeDescription { get; set; }

    public DateTime DateChange { get; set; }

    public virtual File File { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
