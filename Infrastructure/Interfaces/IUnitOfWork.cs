using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<AcademicProgram> AcademicProgram { get; }
        public IGenericRepository<ApplicationUser> ApplicationUser { get; }
        public IGenericRepository<Building> Building { get; }
        public IGenericRepository<Campus> Campus { get; }
        public IGenericRepository<Classroom> Classroom { get; }
        public IGenericRepository<ClassroomFeature> ClassroomFeature { get; }
        public IGenericRepository<Course> Course { get; }
        public IGenericRepository<Instructor> Instructor { get; }
		public IGenericRepository<InstructorWishlist> InstructorWishlist { get; }
        public IGenericRepository<InstructorWishlistCourse> InstructorWishlistCourse { get; }
        public IGenericRepository<LoadRequirement> LoadRequirement { get; }
        public IGenericRepository<Major> Major { get; }
        public IGenericRepository<Modality> Modality { get; }
        public IGenericRepository<PartOfTerm> PartOfTerm { get; }
        public IGenericRepository<PayModel> PayModel { get; }
        public IGenericRepository<PayOrg> PayOrg { get; }
        public IGenericRepository<PrereqCourseAssignment> PrereqCourseAssignment { get; }
        public IGenericRepository<ReleaseTime> ReleaseTime { get; }
        public IGenericRepository<Section> Section { get; }
        public IGenericRepository<SectionStatus> SectionStatus { get; }
        public IGenericRepository<Semester> Semester { get; }
        public IGenericRepository<SemesterInstance> SemesterInstance { get; }
        public IGenericRepository<SemesterInstanceStatus> SemesterInstanceStatus { get; }
        public IGenericRepository<Student> Student { get; }
		public IGenericRepository<StudentWishlist> StudentWishlist { get; }
        public IGenericRepository<StudentWishlistCourse> StudentWishlistCourse { get; }
        public IGenericRepository<Template> Template { get; }
        public IGenericRepository<TemplateAssignment> TemplateAssignment { get;  }
        public IGenericRepository<TimeBlock> TimeBlock { get; }
        public IGenericRepository<TimeBlockType> TimeBlockType { get; }

        int Commit();

        Task<int> CommitAsync();

        void Dispose();
    }
}
