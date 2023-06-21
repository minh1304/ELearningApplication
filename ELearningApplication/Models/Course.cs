using System;
using System.Collections.Generic;

namespace ELearningApplication.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public string? CourseType { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
