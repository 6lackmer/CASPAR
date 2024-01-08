using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        #region Model DbSet Declarations
        public DbSet<AcademicProgram> AcademicPrograms { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<ClassroomFeature> ClassroomFeatures { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<InstructorWishlist> InstructorWishlists { get; set; }
        public DbSet<InstructorWishlistCourse> InstructorWishlistCourses { get; set; }
        public DbSet<LoadRequirement> LoadRequirements { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Modality> Modalitys { get; set; }
        public DbSet<PartOfTerm> PartOfTerms { get; set; }
        public DbSet<PayModel> PayModels { get; set; }
        public DbSet<PayOrg> PayOrgs { get; set; }
        public DbSet<PrereqCourseAssignment> PrereqCourseAssignments { get; set; }
        public DbSet<ReleaseTime> ReleaseTimes { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SectionStatus> SectionStatuses { get; set; }
        public DbSet<SectionPay> SectionPays { get; set; }
        public DbSet<SectionTime> SectionTimes { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<SemesterInstance> SemesterInstances { get; set; }
        public DbSet<SemesterInstanceStatus> SemesterInstanceStatuses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentWishlist> StudentWishlists { get; set; }
        public DbSet<StudentWishlistCourse> StudentWishlistCourses { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<TemplateAssignment> TemplateAssignments { get; set; }
        public DbSet<TimeBlock> TimeBlocks { get; set; }
        public DbSet<TimeBlockType> TimeBlockTypes { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
