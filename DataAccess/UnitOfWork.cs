using Infrastructure.Interfaces;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<AcademicProgram> academicProgram;
        private readonly IGenericRepository<Building> building;
        private readonly IGenericRepository<Campus> campus;
        private readonly IGenericRepository<Classroom> classroom;
        private readonly IGenericRepository<ClassroomFeature> classroomFeature;
        private readonly IGenericRepository<Course> course;
        private readonly IGenericRepository<Instructor> instructor;
		private readonly IGenericRepository<InstructorWishlist> instructorWishlist;
        private readonly IGenericRepository<InstructorWishlistCourse> instructorWishlistCourse;
        private readonly IGenericRepository<LoadRequirement> loadRequirement;
        private readonly IGenericRepository<Major> major;
        private readonly IGenericRepository<Modality> modality;
        private readonly IGenericRepository<PartOfTerm> partOfTerm;
        private readonly IGenericRepository<PayModel> payModel;
        private readonly IGenericRepository<PayOrg> payOrg;
        private readonly IGenericRepository<PrereqCourseAssignment> prereqCourseAssignment;
        private readonly IGenericRepository<ReleaseTime> releaseTime;
        private readonly IGenericRepository<Section> section;
        private readonly IGenericRepository<SectionStatus> sectionStatus;
        private readonly IGenericRepository<SectionPay> sectionPay;
        private readonly IGenericRepository<SectionTime> sectionTime;
        private readonly IGenericRepository<Semester> semester;
        private readonly IGenericRepository<SemesterInstance> semesterInstance;
        private readonly IGenericRepository<SemesterInstanceStatus> semesterInstanceStatus;
        private readonly IGenericRepository<Student> student;
        private readonly IGenericRepository<StudentWishlist> studentWishlist;
        private readonly IGenericRepository<StudentWishlistCourse> studentWishlistCourse;
        private readonly IGenericRepository<Template> template;
        private readonly IGenericRepository<TemplateAssignment> templateAssignment;
        private readonly IGenericRepository<TimeBlock> timeBlock;
        private readonly IGenericRepository<TimeBlockType> timeBlockType;
        private readonly IGenericRepository<ApplicationUser> applicationUser;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            academicProgram = new GenericRepository<AcademicProgram>(_context);
            building = new GenericRepository<Building>(_context);
            campus = new GenericRepository<Campus>(_context);
            classroom = new GenericRepository<Classroom>(_context);
            classroomFeature = new GenericRepository<ClassroomFeature>(_context);
            course = new GenericRepository<Course>(_context);
            instructor = new GenericRepository<Instructor>(_context);
            instructorWishlist = new GenericRepository<InstructorWishlist>(_context);
            instructorWishlistCourse = new GenericRepository<InstructorWishlistCourse>(_context);
            loadRequirement = new GenericRepository<LoadRequirement>(_context);
            major = new GenericRepository<Major>(_context);
            modality = new GenericRepository<Modality>(_context);
            partOfTerm = new GenericRepository<PartOfTerm>(_context);
            payModel = new GenericRepository<PayModel>(_context);
            payOrg = new GenericRepository<PayOrg>(_context);
            prereqCourseAssignment = new GenericRepository<PrereqCourseAssignment>(_context);
            releaseTime = new GenericRepository<ReleaseTime>(_context);
            section = new GenericRepository<Section>(_context);
            sectionStatus = new GenericRepository<SectionStatus>(_context);
            sectionPay = new GenericRepository<SectionPay>(_context);
            sectionTime = new GenericRepository<SectionTime>(_context);
            semester = new GenericRepository<Semester>(_context);
            semesterInstance = new GenericRepository<SemesterInstance>(_context);
            semesterInstanceStatus = new GenericRepository<SemesterInstanceStatus>(_context);
            student = new GenericRepository<Student>(_context);
            studentWishlist = new GenericRepository<StudentWishlist>(_context);
            studentWishlistCourse = new GenericRepository<StudentWishlistCourse>(_context);
            template = new GenericRepository<Template>(_context);
            templateAssignment = new GenericRepository<TemplateAssignment>(_context);
            timeBlock = new GenericRepository<TimeBlock>(_context);
            timeBlockType = new GenericRepository<TimeBlockType>(_context);
            applicationUser = new GenericRepository<ApplicationUser>(_context);
        }

        public IGenericRepository<AcademicProgram> AcademicProgram => academicProgram;
        public IGenericRepository<Building> Building => building;
        public IGenericRepository<Campus> Campus => campus;
        public IGenericRepository<Classroom> Classroom => classroom;
        public IGenericRepository<ClassroomFeature> ClassroomFeature => classroomFeature;
        public IGenericRepository<Course> Course => course;
        public IGenericRepository<Instructor> Instructor => instructor;
		public IGenericRepository<InstructorWishlist> InstructorWishlist => instructorWishlist;
        public IGenericRepository<InstructorWishlistCourse> InstructorWishlistCourse => instructorWishlistCourse;
        public IGenericRepository<LoadRequirement> LoadRequirement => loadRequirement;
        public IGenericRepository<Major> Major => major;
        public IGenericRepository<Modality> Modality => modality;
        public IGenericRepository<PartOfTerm> PartOfTerm => partOfTerm;
        public IGenericRepository<PayModel> PayModel => payModel;
        public IGenericRepository<PayOrg> PayOrg => payOrg;
        public IGenericRepository<PrereqCourseAssignment> PrereqCourseAssignment => prereqCourseAssignment;
        public IGenericRepository<ReleaseTime> ReleaseTime => releaseTime;
        public IGenericRepository<Section> Section => section;
        public IGenericRepository<SectionStatus> SectionStatus => sectionStatus;
        public IGenericRepository<SectionPay> SectionPay => sectionPay;
        public IGenericRepository<SectionTime> SectionTime => sectionTime;
        public IGenericRepository<Semester> Semester => semester;
        public IGenericRepository<SemesterInstance> SemesterInstance => semesterInstance;
        public IGenericRepository<SemesterInstanceStatus> SemesterInstanceStatus => semesterInstanceStatus;
        public IGenericRepository<Student> Student => student;
        public IGenericRepository<StudentWishlist> StudentWishlist => studentWishlist;
        public IGenericRepository<StudentWishlistCourse> StudentWishlistCourse => studentWishlistCourse;
        public IGenericRepository<Template> Template => template;
        public IGenericRepository<TemplateAssignment> TemplateAssignment => templateAssignment;
        public IGenericRepository<TimeBlock> TimeBlock => timeBlock;
        public IGenericRepository<TimeBlockType> TimeBlockType => timeBlockType;
        public IGenericRepository<ApplicationUser> ApplicationUser => applicationUser;

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
