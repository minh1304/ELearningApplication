using System;
using System.Collections.Generic;

namespace ELearningApplication.Models;

public partial class Clip
{
    public int ClipId { get; set; }

    public int? LessonId { get; set; }

    public string? ClipName { get; set; }

    public string? ClipUrl { get; set; }

    public TimeSpan? Duration { get; set; }

    public virtual Lesson? Lesson { get; set; }
}
