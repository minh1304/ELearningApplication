using ELearningApplication.StoredProcedure;

namespace ELearningApplication.Models;

public partial class ELearningApplicationContext : DbContext
{
    public ELearningApplicationContext()
    {
    }

    public ELearningApplicationContext(DbContextOptions<ELearningApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Clip> Clips { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }
    // Khai báo một phương thức DbSet để thực hiện stored procedure
    public DbSet<CourseDetails> CourseDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = localhost,1433;Initial Catalog =E_Learning_Application; User ID=sa; Password=Password123; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Clip>(entity =>
        {
            entity.HasKey(e => e.ClipId).HasName("PK__Clip__87CC71CA72A02821");

            entity.ToTable("Clip");

            entity.Property(e => e.ClipId).ValueGeneratedNever();
            entity.Property(e => e.ClipName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ClipUrl)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Lesson).WithMany(p => p.Clips)
                .HasForeignKey(d => d.LessonId)
                .HasConstraintName("FK__Clip__LessonId__4BAC3F29");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__C92D71A74288FFAE");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId).ValueGeneratedNever();
            entity.Property(e => e.CourseName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CourseType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lesson__B084ACD0F5F0609A");

            entity.ToTable("Lesson");

            entity.Property(e => e.LessonId).ValueGeneratedNever();
            entity.Property(e => e.LessonDescription)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.Course).WithMany(p => p.Lessons)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Lesson__CourseId__48CFD27E");
        });

        OnModelCreatingPartial(modelBuilder);
        modelBuilder.Entity<CourseDetails>().HasNoKey().ToView(null);
    }
    public virtual async Task<List<CourseDetails>> GetCourseDetails(int courseId)
    {
        // Sử dụng phương thức FromSqlInterpolated để gọi stored procedure
        return await CourseDetails.FromSqlInterpolated($"EXECUTE GetCourseDetails {courseId}").ToListAsync();
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
