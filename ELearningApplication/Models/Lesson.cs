using System;
using System.Collections.Generic;

namespace ELearningApplication.Models;

public partial class Lesson
{
    public int LessonId { get; set; }

    public int? CourseId { get; set; }

    public string? LessonDescription { get; set; }

    public virtual ICollection<Clip> Clips { get; set; } = new List<Clip>();

    public virtual Course? Course { get; set; }
}
