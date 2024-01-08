using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using Utility;

namespace DataAccess
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            _context.Database.EnsureCreated();

            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            // check if the database has been seeded
            if (_context.Majors.Any())
            {
                return;
            }

            IdentityResult? result;
            Random random = new Random();
            var daysOfWeek = new List<string>
            {
                "MW",
                "MWF",
                "TH",
                "MTWH",
                "M",
                "T",
                "W",
                "H",
            };

            #region Majors
            var majors = new List<Major>
            {
                new Major { MajorName = "Computer Science" },
                new Major { MajorName = "Computer Science Teaching" },
                new Major { MajorName = "Cybersecurity & Network Management" },
                new Major { MajorName = "Web & User Experience" },
                new Major { MajorName = "Web & User Experience: Full Stack Web Development" },
                new Major { MajorName = "Web & User Experience: User Experience Design" },
            };

            foreach (var major in majors)
            {
                _context.Majors.Add(major);
            }

            _context.SaveChanges();
            #endregion

            #region Roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole(SD.AdminRole),
                new IdentityRole(SD.FlexCoordinatorRole),
                new IdentityRole(SD.GraduateCoordinatorRole),
                new IdentityRole(SD.ProgramCoordinatorRole),
                new IdentityRole(SD.SecretaryRole),
                new IdentityRole(SD.InstructorRole),
                new IdentityRole(SD.StudentRole)
            };

            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(role);
            }

            _context.SaveChanges();
            #endregion

            #region Administrator
            var admin = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "Istrator",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                UserName = "admin@admin.com",
                NormalizedUserName = "ADMIN@ADMIN.COM"
            };

            result = _userManager.CreateAsync(admin, "Pa$$w0rd").Result;

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, SD.AdminRole);
            }
            #endregion

            #region Flex Coordinator
            var flexCoordinator = new ApplicationUser
            {
                FirstName = "Flex Coor",
                LastName = "Dinator",
                Email = "flexcd@flexcd.com",
                NormalizedEmail = "FLEXCD@FLEXCD.COM",
                UserName = "flexcd@flexcd.com",
                NormalizedUserName = "FLEXCD@FLEXCD.COM"
            };

            result = _userManager.CreateAsync(flexCoordinator, "Pa$$w0rd").Result;

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(flexCoordinator, SD.FlexCoordinatorRole);
            }
            #endregion

            #region Program Coordinator
            var programCoordinator = new ApplicationUser
            {
                FirstName = "Pro",
                LastName = "Gram",
                Email = "programcd@programcd.com",
                NormalizedEmail = "PROGRAMCD@PROGRAMCD.COM",
                UserName = "programcd@programcd.com",
                NormalizedUserName = "PROGRAMCD@PROGRAMCD.COM"
            };

            result = _userManager.CreateAsync(programCoordinator, "Pa$$w0rd").Result;

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(programCoordinator, SD.ProgramCoordinatorRole);
            }
            #endregion

            #region Graduate Coordinator
            var graduateCoordinator = new ApplicationUser
            {
                FirstName = "Gradu",
                LastName = "Ate",
                Email = "graduatecd@graduatecd.com",
                NormalizedEmail = "GRADUATECD@GRADUATECD.COM",
                UserName = "graduatecd@graduatecd.com",
                NormalizedUserName = "GRADUATECD@GRADUATECD.COM"
            };

            result = _userManager.CreateAsync(graduateCoordinator, "Pa$$w0rd").Result;

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(graduateCoordinator, SD.GraduateCoordinatorRole);
            }
            #endregion

            #region Secretary
            var secretary = new ApplicationUser
            {
                FirstName = "Secret",
                LastName = "Ary",
                Email = "secretary@secretary.com",
                NormalizedEmail = "SECRETARY@SECRETARY.COM",
                UserName = "secretary@secretary.com",
                NormalizedUserName = "SECRETARY@SECRETARY.COM"
            };

            result = _userManager.CreateAsync(secretary, "Pa$$w0rd").Result;

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(secretary, SD.SecretaryRole);
            }
            #endregion

            #region Instructors
            var instructorAccounts = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    FirstName = "Prof.",
                    LastName = "Professorson",
                    Email = "instructor@instructor.com",
                    NormalizedEmail = "INSTRUCTOR@INSTRUCTOR.COM",
                    UserName = "instructor@instructor.com",
                    NormalizedUserName = "INSTRUCTOR@INSTRUCTOR.COM",
                    LockoutEnabled = false
                },
            };

            using (TextFieldParser parser = new TextFieldParser(@"src/instructorData.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields() ?? Array.Empty<string>();
                    instructorAccounts.Add(
                        new ApplicationUser
                        {
                            FirstName = fields[1],
                            LastName = fields[0],
                            Email = fields[2],
                            NormalizedEmail = fields[2].ToUpper(),
                            UserName = fields[2],
                            NormalizedUserName = fields[2].ToUpper()
                        }
                    );
                }
            }

            var instructors = new List<Instructor>();

            foreach (var instructorAccount in instructorAccounts)
            {
                result = _userManager.CreateAsync(instructorAccount, "Pa$$w0rd").Result;

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(instructorAccount, SD.InstructorRole);

                    var instructor = new Instructor
                    {
                        UserId = instructorAccount.Id,
                        InstructorTitle = "Professor",
                    };

                    instructors.Add(instructor);
                    _context.Instructors.Add(instructor);
                }
            }

            _context.SaveChanges();
            #endregion

            #region Students
            var studentAccounts = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    FirstName = "Stu",
                    LastName = "Dent",
                    Email = "student@student.com",
                    NormalizedEmail = "STUDENT@STUDENT.COM",
                    UserName = "student@student.com",
                    NormalizedUserName = "STUDENT@STUDENT.COM"
                }
            };

            //using (TextFieldParser parser = new TextFieldParser(@"src/studentData.csv"))
            //{
            //    parser.TextFieldType = FieldType.Delimited;
            //    parser.SetDelimiters(",");
            //    while (!parser.EndOfData)
            //    {
            //        string[] fields = parser.ReadFields() ?? Array.Empty<string>();
            //        studentAccounts.Add(
            //            new ApplicationUser
            //            {
            //                FirstName = fields[1],
            //                LastName = fields[2],
            //                Email = fields[3],
            //                NormalizedEmail = fields[3].ToUpper(),
            //                UserName = fields[3],
            //                NormalizedUserName = fields[3].ToUpper(),
            //            }
            //        );
            //    }
            //}

            //var students = new List<Student>();

            //foreach (var studentAccount in studentAccounts)
            //{
            //    result = _userManager.CreateAsync(studentAccount, "Pa$$w0rd").Result;

            //    if (result.Succeeded)
            //    {
            //        await _userManager.AddToRoleAsync(studentAccount, SD.StudentRole);

            //        var student = new Student
            //        {
            //            UserId = studentAccount.Id,
            //            Major = majors.ElementAt(random.Next(majors.Count)),
            //            GraduationYear = random.Next(2023, 2030),
            //        };

            //        students.Add(student);
            //        _context.Students.Add(student);
            //    }
            //}

            //_context.SaveChanges();
            #endregion

            #region Academic Programs
            var computerScience = new AcademicProgram { ProgramName = "Computer Science", ProgramCode = "CS" };
            var networkManagement = new AcademicProgram { ProgramName = "Network Management Tech", ProgramCode = "NET" };
            var webUserExperience = new AcademicProgram { ProgramName = "Web And User Experience", ProgramCode = "WEB" };

            var academicPrograms = new List<AcademicProgram>
            {
                computerScience,
                networkManagement,
                webUserExperience,
            };

            foreach (var academicProgram in academicPrograms)
            {
                _context.AcademicPrograms.Add(academicProgram);
            }

            _context.SaveChanges();
            #endregion

            #region Courses
            var cs1030 = new Course { Program = computerScience, CourseTitle = "Foundations of Computing", CreditHours = 4, CourseNumber = "1030", CourseDescription = "" };
            var cs1400 = new Course { Program = computerScience, CourseTitle = "Programming I", CreditHours = 4, CourseNumber = "1400", CourseDescription = "" };
            var cs1410 = new Course { Program = computerScience, CourseTitle = "Object Oriented Programming", CreditHours = 4, CourseNumber = "1410", CourseDescription = "" };
            var cs2130 = new Course { Program = computerScience, CourseTitle = "Computational Structures", CreditHours = 4, CourseNumber = "2130", CourseDescription = "" };
            var cs2350 = new Course { Program = computerScience, CourseTitle = "Client-Side Web Development", CreditHours = 4, CourseNumber = "2350", CourseDescription = "" };
            var cs2420 = new Course { Program = computerScience, CourseTitle = "Data Structures & Algorithms", CreditHours = 4, CourseNumber = "2420", CourseDescription = "" };
            var cs2450 = new Course { Program = computerScience, CourseTitle = "Software Engineering I", CreditHours = 4, CourseNumber = "2450", CourseDescription = "" };
            var cs2550 = new Course { Program = computerScience, CourseTitle = "Intro to Databases", CreditHours = 4, CourseNumber = "2550", CourseDescription = "" };
            var cs2705 = new Course { Program = computerScience, CourseTitle = "Network Fundamentals & Design", CreditHours = 4, CourseNumber = "2705", CourseDescription = "" };
            var cs2810 = new Course { Program = computerScience, CourseTitle = "Computer Architecture", CreditHours = 4, CourseNumber = "2810", CourseDescription = "" };
            var cs2899 = new Course { Program = computerScience, CourseTitle = "Associate Degree Assessment", CreditHours = 0, CourseNumber = "2899", CourseDescription = "" };
            var cs3030 = new Course { Program = computerScience, CourseTitle = "Scripting Languages", CreditHours = 4, CourseNumber = "3030", CourseDescription = "" };
            var cs3100 = new Course { Program = computerScience, CourseTitle = "Operating Systems", CreditHours = 4, CourseNumber = "3100", CourseDescription = "" };
            var cs3280 = new Course { Program = computerScience, CourseTitle = "Object Oriented Windows Applications", CreditHours = 4, CourseNumber = "3280", CourseDescription = "" };
            var cs3550 = new Course { Program = computerScience, CourseTitle = "Adv Database Programming", CreditHours = 4, CourseNumber = "3550", CourseDescription = "" };
            var cs3650 = new Course { Program = computerScience, CourseTitle = "Human Computer Interaction", CreditHours = 4, CourseNumber = "3650", CourseDescription = "" };
            var cs3750 = new Course { Program = computerScience, CourseTitle = "Software Engineering II", CreditHours = 4, CourseNumber = "3750", CourseDescription = "" };
            var cs4110 = new Course { Program = computerScience, CourseTitle = "Formal Languages & Algorithms", CreditHours = 4, CourseNumber = "4110", CourseDescription = "" };
            var cs4760 = new Course { Program = computerScience, CourseTitle = "CS Capstone", CreditHours = 4, CourseNumber = "4760", CourseDescription = "" };
            var cs4899 = new Course { Program = computerScience, CourseTitle = "Bachelor Degree Assessment", CreditHours = 0, CourseNumber = "4899", CourseDescription = "" };
            var net2200 = new Course { Program = networkManagement, CourseTitle = "Cybersecurity & System", CreditHours = 3, CourseNumber = "2200", CourseDescription = "" };
            var net2210 = new Course { Program = networkManagement, CourseTitle = "Linux Systems Administration", CreditHours = 3, CourseNumber = "2210", CourseDescription = "" };
            var net2300 = new Course { Program = networkManagement, CourseTitle = "Introduction to LAN Management", CreditHours = 3, CourseNumber = "2300", CourseDescription = "" };
            var net2415 = new Course { Program = networkManagement, CourseTitle = "Cisco TCP/IP Routing Prot/Conf", CreditHours = 3, CourseNumber = "2415", CourseDescription = "" };
            var net2435 = new Course { Program = networkManagement, CourseTitle = "Cisco Adv LAN/WAN Switch/Routing", CreditHours = 3, CourseNumber = "2435", CourseDescription = "" };
            var net3300 = new Course { Program = networkManagement, CourseTitle = "Advanced LAN Security Management", CreditHours = 3, CourseNumber = "3300", CourseDescription = "" };
            var net3710 = new Course { Program = networkManagement, CourseTitle = "Switching & Transmission", CreditHours = 3, CourseNumber = "3710", CourseDescription = "" };
            var net3715 = new Course { Program = networkManagement, CourseTitle = "Transmission Network Apps", CreditHours = 3, CourseNumber = "3715", CourseDescription = "" };
            var net4700 = new Course { Program = networkManagement, CourseTitle = "Data & Voice Network Design", CreditHours = 4, CourseNumber = "4700", CourseDescription = "" };
            var net4740 = new Course { Program = networkManagement, CourseTitle = "Security Intrusion Mitigation", CreditHours = 4, CourseNumber = "4740", CourseDescription = "" };
            var net4760 = new Course { Program = networkManagement, CourseTitle = "INT Network Management Tech Intern", CreditHours = 3, CourseNumber = "4760", CourseDescription = "" };
            var net4790 = new Course { Program = networkManagement, CourseTitle = "INT Network Management Tech Senior Project", CreditHours = 3, CourseNumber = "4790", CourseDescription = "" };
            var web1400 = new Course { Program = webUserExperience, CourseTitle = "Web Design & Usability", CreditHours = 3, CourseNumber = "1400", CourseDescription = "" };
            var web1430 = new Course { Program = webUserExperience, CourseTitle = "Client side Programming", CreditHours = 3, CourseNumber = "1430", CourseDescription = "" };
            var web1700 = new Course { Program = webUserExperience, CourseTitle = "Intro to Computer Applications", CreditHours = 3, CourseNumber = "1700", CourseDescription = "" };
            var web1701 = new Course { Program = webUserExperience, CourseTitle = "Document Creation", CreditHours = 1, CourseNumber = "1701", CourseDescription = "" };
            var web1702 = new Course { Program = webUserExperience, CourseTitle = "Content, Internet & Device", CreditHours = 1, CourseNumber = "1702", CourseDescription = "" };
            var web1703 = new Course { Program = webUserExperience, CourseTitle = "Data, Visual & Presentation", CreditHours = 1, CourseNumber = "1703", CourseDescription = "" };
            var web2500 = new Course { Program = webUserExperience, CourseTitle = "User Experience Design", CreditHours = 3, CourseNumber = "2500", CourseDescription = "" };
            var web2620 = new Course { Program = webUserExperience, CourseTitle = "Advanced CSS", CreditHours = 3, CourseNumber = "2620", CourseDescription = "" };
            var web2630 = new Course { Program = webUserExperience, CourseTitle = "Client Side Frameworks", CreditHours = 4, CourseNumber = "2630", CourseDescription = "" };
            var web3200 = new Course { Program = webUserExperience, CourseTitle = "Dynamic Languages Web Development", CreditHours = 3, CourseNumber = "3200", CourseDescription = "" };
            var web3400 = new Course { Program = webUserExperience, CourseTitle = "LAMP Stack Web Development", CreditHours = 3, CourseNumber = "3400", CourseDescription = "" };
            var web3430 = new Course { Program = webUserExperience, CourseTitle = "Full Stack Javascript Development", CreditHours = 3, CourseNumber = "3430", CourseDescription = "" };
            var web3500 = new Course { Program = webUserExperience, CourseTitle = "User Interface Prototype Design", CreditHours = 3, CourseNumber = "3500", CourseDescription = "" };
            var web3600 = new Course { Program = webUserExperience, CourseTitle = "User Research Methods", CreditHours = 4, CourseNumber = "3600", CourseDescription = "" };
            var web3700 = new Course { Program = webUserExperience, CourseTitle = "Web Development with .NET", CreditHours = 4, CourseNumber = "3700", CourseDescription = "" };
            var web4350 = new Course { Program = webUserExperience, CourseTitle = "Web Development Capstone", CreditHours = 4, CourseNumber = "4350", CourseDescription = "" };
            var web4800 = new Course { Program = webUserExperience, CourseTitle = "Independent Research", CreditHours = 4, CourseNumber = "4800", CourseDescription = "" };
            var web4860 = new Course { Program = webUserExperience, CourseTitle = "INT Internship", CreditHours = 3, CourseNumber = "4860", CourseDescription = "" };
            var web4890 = new Course { Program = webUserExperience, CourseTitle = "Server-Side Portfolio", CreditHours = 3, CourseNumber = "4890", CourseDescription = "" };

            var courses = new List<Course>
            {
                cs1030,
                cs1400,
                cs1410,
                cs2130,
                cs2350,
                cs2420,
                cs2450,
                cs2550,
                cs2705,
                cs2810,
                cs2899,
                cs3030,
                cs3100,
                cs3280,
                cs3550,
                cs3650,
                cs3750,
                cs4110,
                cs4760,
                cs4899,
                net2200,
                net2210,
                net2300,
                net2415,
                net2435,
                net3300,
                net3710,
                net3715,
                net4700,
                net4740,
                net4760,
                net4790,
                web1400,
                web1430,
                web1700,
                web1701,
                web1702,
                web1703,
                web2500,
                web2620,
                web2630,
                web3200,
                web3400,
                web3430,
                web3500,
                web3600,
                web3700,
                web4350,
                web4800,
                web4860,
                web4890,
            };

            foreach (var course in courses)
            {
                _context.Courses.Add(course);
            }

            _context.SaveChanges();
            #endregion

            #region Modalities
            var modalities = new List<Modality>
            {
                new Modality
                {
                    ModalityName = "Online",
                    Description = "These courses are taught through WSU Online - Canvas and have no set meeting time or location.",
                    HasCampuses = false,
                    HasTimeBlocks = false,
                },
                new Modality
                {
                    ModalityName = "Virtual",
                    Description = "These courses are taught through virtual lectures during regular meeting times.",
                    HasCampuses = false,
                    HasTimeBlocks = true,
                },
                new Modality
                {
                    ModalityName = "Hybrid",
                    Description = "These courses can combine online, virtual, and face-to-face learning.",
                    HasCampuses = true,
                    HasTimeBlocks = true,
                },
                new Modality
                {
                    ModalityName = "Face-to-Face",
                    Description = "These courses are taught in-person at a specified location during regular meeting times.",
                    HasCampuses = true,
                    HasTimeBlocks = true,
                },
                new Modality
                {
                    ModalityName = "CS FLEX",
                    Description = "These accelerated online courses have flexible dates so students can work at their own pace.",
                    HasCampuses = false,
                    HasTimeBlocks = false,
                },
            };

            foreach (var modality in modalities)
            {
                _context.Modalitys.Add(modality);
            }

            _context.SaveChanges();
            #endregion

            #region Campuses
            var ogden = new Campus { CampusName = "WSU Ogden" };
            var davis = new Campus { CampusName = "WSU Davis" };
            var taylorsville = new Campus { CampusName = "SLCC Taylorsville" };

            var campuses = new List<Campus>
            {
                ogden,
                davis,
                taylorsville,
            };

            foreach (var campus in campuses)
            {
                _context.Campuses.Add(campus);
            }

            _context.SaveChanges();
            #endregion

            #region Buildings
            var noorda = new Building { Name = "NOORDA", Campus = ogden };
            var elizabethHall = new Building { Name = "Elizabeth Hall", Campus = ogden };
            var cae = new Building { Name = "Computer & Automotive Engineering Building", Campus = davis };
            var buildingTwo = new Building { Name = "Building 2", Campus = davis };
            var gmbb = new Building { Name = "GMBB", Campus = taylorsville };

            var buildings = new List<Building>
            {
                noorda,
                elizabethHall,
                cae,
                buildingTwo,
                gmbb
            };

            foreach (var building in buildings)
            {
                _context.Buildings.Add(building);
            }

            _context.SaveChanges();
            #endregion

            #region Classrooms
            var classrooms = new List<Classroom>
            {
                new Classroom { ClassroomNumber = "142", Building = cae },
                new Classroom { ClassroomNumber = "143", Building = cae },
                new Classroom { ClassroomNumber = "145", Building = cae },
                new Classroom { ClassroomNumber = "146", Building = cae },
                new Classroom { ClassroomNumber = "147", Building = cae },
                new Classroom { ClassroomNumber = "212", Building = noorda },
                new Classroom { ClassroomNumber = "213", Building = noorda },
                new Classroom { ClassroomNumber = "232", Building = noorda },
                new Classroom { ClassroomNumber = "234", Building = noorda },
                new Classroom { ClassroomNumber = "318", Building = noorda },
                new Classroom { ClassroomNumber = "322", Building = noorda },
                new Classroom { ClassroomNumber = "324", Building = noorda },
                new Classroom { ClassroomNumber = "327", Building = noorda },
                new Classroom { ClassroomNumber = "328", Building = noorda },
                new Classroom { ClassroomNumber = "221", Building = gmbb },
                new Classroom { ClassroomNumber = "330", Building = gmbb },
                new Classroom { ClassroomNumber = "313", Building = elizabethHall },
                new Classroom { ClassroomNumber = "311", Building = buildingTwo },
                new Classroom { ClassroomNumber = "315", Building = buildingTwo },
            };

            foreach (var classroom in classrooms)
            {
                _context.Classrooms.Add(classroom);
            }

            _context.SaveChanges();
            #endregion

            #region Parts-of-Term
            var partsOfTerm = new List<PartOfTerm>
            {
                new PartOfTerm { PartOfTermTitle = "Full Semester" },
                new PartOfTerm { PartOfTermTitle = "First Block" },
                new PartOfTerm { PartOfTermTitle = "Second Block" }
            };

            foreach (var partOfTerm in partsOfTerm)
            {
                _context.PartOfTerms.Add(partOfTerm);
            }

            _context.SaveChanges();
            #endregion

            #region Time Block Types
            var instructorTimeBlock = new TimeBlockType { Name = "Instructor" };
            var studentTimeBlock = new TimeBlockType { Name = "Student" };

            var timeBlockTypes = new List<TimeBlockType>
            {
                instructorTimeBlock,
                studentTimeBlock,
            };

            foreach (var timeBlockType in timeBlockTypes)
            {
                _context.TimeBlockTypes.Add(timeBlockType);
            }

            _context.SaveChanges();
            #endregion

            #region Time Blocks
            var timeBlocks = new List<TimeBlock>
            {
                new TimeBlock
                {
                    TimeBlockName = "Early Morning",
                    TimeBlockType = instructorTimeBlock,
                    StartTime = new TimeSpan(7, 30, 0),
                    EndTime = new TimeSpan(10, 0, 0),
                },
                new TimeBlock
                {
                    TimeBlockName = "Late Morning",
                    TimeBlockType = instructorTimeBlock ,
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(12, 0, 0),
                },
                new TimeBlock
                {
                    TimeBlockName = "Early Afternoon",
                    TimeBlockType = instructorTimeBlock,
                    StartTime = new TimeSpan(12, 0, 0),
                    EndTime = new TimeSpan(14, 30, 0),
                },
                new TimeBlock
                {
                    TimeBlockName = "Late Afternoon",
                    TimeBlockType = instructorTimeBlock,
                    StartTime = new TimeSpan(14, 30, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                },
                new TimeBlock
                {
                    TimeBlockName = "Early Evening",
                    TimeBlockType = instructorTimeBlock,
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(19, 30, 0),
                },
                new TimeBlock
                {
                    TimeBlockName = "Late Evening",
                    TimeBlockType = instructorTimeBlock,
                    StartTime = new TimeSpan(19, 30, 0),
                    EndTime = new TimeSpan(21, 45, 0),
                },
                new TimeBlock
                {
                    TimeBlockName = "Morning",
                    TimeBlockType = studentTimeBlock,
                    StartTime = new TimeSpan(7, 30, 0),
                    EndTime = new TimeSpan(12, 0, 0),
                },
                new TimeBlock
                {
                    TimeBlockName = "Afternoon",
                    TimeBlockType = studentTimeBlock,
                    StartTime = new TimeSpan(12, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                },
                new TimeBlock
                {
                    TimeBlockName = "Evening",
                    TimeBlockType = studentTimeBlock,
                    StartTime = new TimeSpan(17, 0, 0),
                    EndTime = new TimeSpan(21, 45, 0),
                },
            };

            foreach (var timeBlock in timeBlocks)
            {
                _context.TimeBlocks.Add(timeBlock);
            }

            _context.SaveChanges();
            #endregion

            #region Pay Models
            var loadPayModel = new PayModel { PayModelTitle = "Load" };
            var overloadPayModel = new PayModel { PayModelTitle = "Overload" };

            var payModels = new List<PayModel>
            {
                loadPayModel,
                overloadPayModel,
                new PayModel { PayModelTitle = "Adjunct Pay" }
            };

            foreach (var payModel in payModels)
            {
                _context.PayModels.Add(payModel);
            }

            _context.SaveChanges();
            #endregion

            #region Funding Sources
            var fundingSources = new List<PayOrg>
            {
                new PayOrg { PayOrgTitle = "EAST" },
                new PayOrg { PayOrgTitle = "Grant" },
                new PayOrg { PayOrgTitle = "OCE" },
                new PayOrg { PayOrgTitle = "Provost" },
                new PayOrg { PayOrgTitle = "SWI" },
                new PayOrg { PayOrgTitle = "SoC" },
                new PayOrg { PayOrgTitle = "EAST + OCE" }
            };

            foreach (var fundingSource in fundingSources)
            {
                _context.PayOrgs.Add(fundingSource);
            }

            _context.SaveChanges();
            #endregion

            #region Semester Types
            var fallSemester = new Semester { SemesterName = "Fall", DefaultLoad = 12 };
            var springSemester = new Semester { SemesterName = "Spring", DefaultLoad = 12 };
            var summerSemester = new Semester { SemesterName = "Summer", DefaultLoad = 0 };

            var semesters = new List<Semester>
            {
                fallSemester,
                springSemester,
                summerSemester,
            };

            foreach (var semester in semesters)
            {
                _context.Semesters.Add(semester);
            }

            _context.SaveChanges();
            #endregion

            #region Semester Templates
            var fallTemplate = new Template { Semester = fallSemester };
            var springTemplate = new Template { Semester = springSemester };
            var summerTemplate = new Template { Semester = summerSemester };

            var templates = new List<Template>
            {
                fallTemplate,
                springTemplate,
                summerTemplate,
            };

            foreach (var template in templates)
            {
                _context.Templates.Add(template);
            }

            _context.SaveChanges();
            #endregion

            #region Semester Template Courses
            var templateAssignments = new List<TemplateAssignment>
            {
                new TemplateAssignment { Template = fallTemplate, Course = cs1030, TotalCount = 6, FaceToFaceCount = 3, OnlineCount = 1, FlexCount = 2 }, // CS 1030
                new TemplateAssignment { Template = fallTemplate, Course = cs1400, TotalCount = 7, FaceToFaceCount = 3, OnlineCount = 2, FlexCount = 2 }, // CS 1400
                new TemplateAssignment { Template = fallTemplate, Course = cs1410, TotalCount = 5, FaceToFaceCount = 3, OnlineCount = 0, FlexCount = 2 }, // CS 2130
                new TemplateAssignment { Template = fallTemplate, Course = cs2350, TotalCount = 5, FaceToFaceCount = 1, OnlineCount = 2, FlexCount = 2 }, // CS 2350
                new TemplateAssignment { Template = fallTemplate, Course = cs2420, TotalCount = 5, FaceToFaceCount = 2, OnlineCount = 1, FlexCount = 2 }, // CS 2420
                new TemplateAssignment { Template = fallTemplate, Course = cs2450, TotalCount = 3, FaceToFaceCount = 1, OnlineCount = 1, FlexCount = 1 }, // CS 2450
                new TemplateAssignment { Template = fallTemplate, Course = cs2550, TotalCount = 5, FaceToFaceCount = 2, OnlineCount = 1, FlexCount = 2 }, // CS 2550
                new TemplateAssignment { Template = fallTemplate, Course = cs2705, TotalCount = 4, FaceToFaceCount = 1, OnlineCount = 2, FlexCount = 1 }, // CS 2705
                new TemplateAssignment { Template = fallTemplate, Course = cs2810, TotalCount = 3, FaceToFaceCount = 2, OnlineCount = 1 }, // CS 2810
                new TemplateAssignment { Template = fallTemplate, Course = cs2899, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // CS 2899
                new TemplateAssignment { Template = fallTemplate, Course = cs3030, TotalCount = 2, FaceToFaceCount = 1, OnlineCount = 1 }, // CS 3030
                new TemplateAssignment { Template = fallTemplate, Course = cs3100, TotalCount = 3, FaceToFaceCount = 2, OnlineCount = 1 }, // CS 3100
                new TemplateAssignment { Template = fallTemplate, Course = cs3280, TotalCount = 2, FaceToFaceCount = 0, OnlineCount = 2 }, // CS 3280
                new TemplateAssignment { Template = fallTemplate, Course = cs3550, TotalCount = 4, FaceToFaceCount = 1, OnlineCount = 1, FlexCount = 2 }, // CS 3550
                new TemplateAssignment { Template = fallTemplate, Course = cs3650, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // CS 3650
                new TemplateAssignment { Template = fallTemplate, Course = cs3750, TotalCount = 3, FaceToFaceCount = 2, OnlineCount = 0, FlexCount = 1 }, // CS 3750
                new TemplateAssignment { Template = fallTemplate, Course = cs4110, TotalCount = 3, FaceToFaceCount = 3, OnlineCount = 0 }, // CS 4110
                new TemplateAssignment { Template = fallTemplate, Course = cs4760, TotalCount = 2, FaceToFaceCount = 1, OnlineCount = 1 }, // CS 4760
                new TemplateAssignment { Template = fallTemplate, Course = cs4899, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // CS 4899
                new TemplateAssignment { Template = fallTemplate, Course = net2200, TotalCount = 2, FaceToFaceCount = 2, OnlineCount = 0 }, // NET 2200
                new TemplateAssignment { Template = fallTemplate, Course = net2210, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 2210
                new TemplateAssignment { Template = fallTemplate, Course = net2300, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // NET 2300
                new TemplateAssignment { Template = fallTemplate, Course = net2415, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 2415
                new TemplateAssignment { Template = fallTemplate, Course = net2435, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 2435
                new TemplateAssignment { Template = fallTemplate, Course = net3300, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 3300
                new TemplateAssignment { Template = fallTemplate, Course = net3710, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // NET 3710
                new TemplateAssignment { Template = fallTemplate, Course = net4760, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 4760
                new TemplateAssignment { Template = fallTemplate, Course = net4790, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 4790
                new TemplateAssignment { Template = fallTemplate, Course = web1400, TotalCount = 2, FaceToFaceCount = 0, OnlineCount = 2 }, // WEB 1400
                new TemplateAssignment { Template = fallTemplate, Course = web1430, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 0, VirtualCount = 1 }, // WEB 1430
                new TemplateAssignment { Template = fallTemplate, Course = web1700, TotalCount = 2, FaceToFaceCount = 2, OnlineCount = 0 }, // WEB 1700
                new TemplateAssignment { Template = fallTemplate, Course = web2500, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 0, VirtualCount = 1 }, // WEB 2500
                new TemplateAssignment { Template = fallTemplate, Course = web2620, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // WEB 2620
                new TemplateAssignment { Template = fallTemplate, Course = web3200, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // WEB 3200
                new TemplateAssignment { Template = fallTemplate, Course = web3400, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 0, VirtualCount = 1 }, // WEB 3400
                new TemplateAssignment { Template = fallTemplate, Course = web3430, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // WEB 3430
                new TemplateAssignment { Template = fallTemplate, Course = web3600, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // WEB 3600
                new TemplateAssignment { Template = fallTemplate, Course = web4350, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // WEB 4350
                new TemplateAssignment { Template = fallTemplate, Course = web4800, TotalCount = 4, FaceToFaceCount = 0, OnlineCount = 4 }, // WEB 4800
                new TemplateAssignment { Template = fallTemplate, Course = web4860, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // WEB 4860
                new TemplateAssignment { Template = fallTemplate, Course = web4890, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 },  // WEB 4890

                new TemplateAssignment { Template = springTemplate, Course = cs1030, TotalCount = 7, FaceToFaceCount = 4, OnlineCount = 1, FlexCount = 2 }, // CS 1030
                new TemplateAssignment { Template = springTemplate, Course = cs1400, TotalCount = 8, FaceToFaceCount = 4, OnlineCount = 1, FlexCount = 2, VirtualCount = 1 }, // CS 1400
                new TemplateAssignment { Template = springTemplate, Course = cs1410, TotalCount = 6, FaceToFaceCount = 3, OnlineCount = 1, FlexCount = 2 }, // CS 1410
                new TemplateAssignment { Template = springTemplate, Course = cs2130, TotalCount = 5, FaceToFaceCount = 2, OnlineCount = 2, FlexCount = 1 }, // CS 2130
                new TemplateAssignment { Template = springTemplate, Course = cs2350, TotalCount = 4, FaceToFaceCount = 2, OnlineCount = 1, FlexCount = 1 }, // CS 2350
                new TemplateAssignment { Template = springTemplate, Course = cs2420, TotalCount = 4, FaceToFaceCount = 2, OnlineCount = 1, FlexCount = 1 }, // CS 2420
                new TemplateAssignment { Template = springTemplate, Course = cs2450, TotalCount = 3, FaceToFaceCount = 2, OnlineCount = 1 }, // CS 2450
                new TemplateAssignment { Template = springTemplate, Course = cs2550, TotalCount = 6, FaceToFaceCount = 4, OnlineCount = 1, FlexCount = 1 }, // CS 2550
                new TemplateAssignment { Template = springTemplate, Course = cs2705, TotalCount = 4, FaceToFaceCount = 2, OnlineCount = 2 }, // CS 2705
                new TemplateAssignment { Template = springTemplate, Course = cs2810, TotalCount = 4, FaceToFaceCount = 2, OnlineCount = 2 }, // CS 2810
                new TemplateAssignment { Template = springTemplate, Course = cs2899, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // CS 2899
                new TemplateAssignment { Template = springTemplate, Course = cs3030, TotalCount = 2, FaceToFaceCount = 0, OnlineCount = 1, FlexCount = 1 }, // CS 3030
                new TemplateAssignment { Template = springTemplate, Course = cs3100, TotalCount = 3, FaceToFaceCount = 2, OnlineCount = 1 }, // CS 3100
                new TemplateAssignment { Template = springTemplate, Course = cs3280, TotalCount = 2, FaceToFaceCount = 0, OnlineCount = 1, FlexCount = 1 }, // CS 3280
                new TemplateAssignment { Template = springTemplate, Course = cs3550, TotalCount = 3, FaceToFaceCount = 1, OnlineCount = 1, FlexCount = 1 }, // CS 3550
                new TemplateAssignment { Template = springTemplate, Course = cs3750, TotalCount = 3, FaceToFaceCount = 2, OnlineCount = 0, FlexCount = 1 }, // CS 3750
                new TemplateAssignment { Template = springTemplate, Course = cs4110, TotalCount = 2, FaceToFaceCount = 1, OnlineCount = 1 }, // CS 4110
                new TemplateAssignment { Template = springTemplate, Course = cs4760, TotalCount = 2, FaceToFaceCount = 1, OnlineCount = 1 }, // CS 4760
                new TemplateAssignment { Template = springTemplate, Course = cs4899, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // CS 4899
                new TemplateAssignment { Template = springTemplate, Course = net2200, TotalCount = 2, FaceToFaceCount = 2, OnlineCount = 0 }, // NET 2200
                new TemplateAssignment { Template = springTemplate, Course = net2210, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 2210
                new TemplateAssignment { Template = springTemplate, Course = net2300, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 2300
                new TemplateAssignment { Template = springTemplate, Course = net2415, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 2415
                new TemplateAssignment { Template = springTemplate, Course = net2435, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 2435
                new TemplateAssignment { Template = springTemplate, Course = net3300, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // NET 3300
                new TemplateAssignment { Template = springTemplate, Course = net3710, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // NET 3710
                new TemplateAssignment { Template = springTemplate, Course = net4700, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // NET 4700
                new TemplateAssignment { Template = springTemplate, Course = net4740, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 4740
                new TemplateAssignment { Template = springTemplate, Course = net4760, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // NET 4760
                new TemplateAssignment { Template = springTemplate, Course = web1400, TotalCount = 2, FaceToFaceCount = 0, OnlineCount = 2 }, // WEB 1400
                new TemplateAssignment { Template = springTemplate, Course = web1430, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 0, VirtualCount = 1 }, // WEB 1430
                new TemplateAssignment { Template = springTemplate, Course = web1700, TotalCount = 2, FaceToFaceCount = 1, OnlineCount = 1 }, // WEB 1700
                new TemplateAssignment { Template = springTemplate, Course = web2500, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // WEB 2500
                new TemplateAssignment { Template = springTemplate, Course = web2620, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // WEB 2620
                new TemplateAssignment { Template = springTemplate, Course = web3200, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // WEB 3200
                new TemplateAssignment { Template = springTemplate, Course = web3400, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // WEB 3400
                new TemplateAssignment { Template = springTemplate, Course = web3430, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // WEB 3430
                new TemplateAssignment { Template = springTemplate, Course = web3500, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // WEB 3500
                new TemplateAssignment { Template = springTemplate, Course = web3600, TotalCount = 1, FaceToFaceCount = 1, OnlineCount = 0 }, // WEB 3600
                new TemplateAssignment { Template = springTemplate, Course = web4350, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // WEB 4350
                new TemplateAssignment { Template = springTemplate, Course = web4800, TotalCount = 4, FaceToFaceCount = 0, OnlineCount = 4 }, // WEB 4800
                new TemplateAssignment { Template = springTemplate, Course = web4860, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // WEB 4860
                new TemplateAssignment { Template = springTemplate, Course = web4890, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 },  // WEB 4890

                new TemplateAssignment { Template = summerTemplate, Course = cs1030, TotalCount = 4, FaceToFaceCount = 0, OnlineCount = 2, FlexCount = 2 }, // CS 1030
                new TemplateAssignment { Template = summerTemplate, Course = cs1400, TotalCount = 5, FaceToFaceCount = 1, OnlineCount = 1, FlexCount = 2, VirtualCount = 1 }, // CS 1400
                new TemplateAssignment { Template = summerTemplate, Course = cs1410, TotalCount = 4, FaceToFaceCount = 1, OnlineCount = 0, FlexCount = 3 }, // CS 1410
                new TemplateAssignment { Template = summerTemplate, Course = cs2130, TotalCount = 2, FaceToFaceCount = 1, OnlineCount = 1 }, // CS 2130
                new TemplateAssignment { Template = summerTemplate, Course = cs2350, TotalCount = 3, FaceToFaceCount = 0, OnlineCount = 1, FlexCount = 2 }, // CS 2350
                new TemplateAssignment { Template = summerTemplate, Course = cs2420, TotalCount = 4, FaceToFaceCount = 1, OnlineCount = 1, FlexCount = 2 }, // CS 2420
                new TemplateAssignment { Template = summerTemplate, Course = cs2450, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // CS 2450
                new TemplateAssignment { Template = summerTemplate, Course = cs2550, TotalCount = 4, FaceToFaceCount = 1, OnlineCount = 1, FlexCount = 2 }, // CS 2550
                new TemplateAssignment { Template = summerTemplate, Course = cs2705, TotalCount = 2, FaceToFaceCount = 0, OnlineCount = 2 }, // CS 2705
                new TemplateAssignment { Template = summerTemplate, Course = cs2810, TotalCount = 3, FaceToFaceCount = 0, OnlineCount = 1, FlexCount = 2 }, // CS 2810
                new TemplateAssignment { Template = summerTemplate, Course = cs2899, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // CS 2899
                new TemplateAssignment { Template = summerTemplate, Course = cs3030, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // CS 3030
                new TemplateAssignment { Template = summerTemplate, Course = cs3100, TotalCount = 3, FaceToFaceCount = 0, OnlineCount = 1, FlexCount = 2 }, // CS 3100
                new TemplateAssignment { Template = summerTemplate, Course = cs3280, TotalCount = 2, FaceToFaceCount = 0, OnlineCount = 0, FlexCount = 2 }, // CS 3280
                new TemplateAssignment { Template = summerTemplate, Course = cs3550, TotalCount = 2, FaceToFaceCount = 1, OnlineCount = 1 }, // CS 3550
                new TemplateAssignment { Template = summerTemplate, Course = cs3750, TotalCount = 2, FaceToFaceCount = 1, OnlineCount = 0, VirtualCount = 1 }, // CS 3750
                new TemplateAssignment { Template = summerTemplate, Course = cs4110, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // CS 4110
                new TemplateAssignment { Template = summerTemplate, Course = cs4760, TotalCount = 2, FaceToFaceCount = 0, OnlineCount = 1, VirtualCount = 1 }, // CS 4760
                new TemplateAssignment { Template = summerTemplate, Course = cs4899, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // CS 4899
                new TemplateAssignment { Template = summerTemplate, Course = net2210, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 2210
                new TemplateAssignment { Template = summerTemplate, Course = net4760, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 4760
                new TemplateAssignment { Template = summerTemplate, Course = net4790, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // NET 4790
                new TemplateAssignment { Template = summerTemplate, Course = web1400, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // WEB 1400
                new TemplateAssignment { Template = summerTemplate, Course = web1700, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // WEB 1700
                new TemplateAssignment { Template = summerTemplate, Course = web2620, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // WEB 2620
                new TemplateAssignment { Template = summerTemplate, Course = web3500, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // WEB 3500
                new TemplateAssignment { Template = summerTemplate, Course = web4800, TotalCount = 4, FaceToFaceCount = 0, OnlineCount = 4 }, // WEB 4800
                new TemplateAssignment { Template = summerTemplate, Course = web4860, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }, // WEB 4860
                new TemplateAssignment { Template = summerTemplate, Course = web4890, TotalCount = 1, FaceToFaceCount = 0, OnlineCount = 1 }  // WEB 4890
            };

            foreach (var templateAssignment in templateAssignments)
            {
                _context.TemplateAssignments.Add(templateAssignment);
            }

            _context.SaveChanges();
            #endregion

            #region Semester Instance Statuses
            var newStatus = new SemesterInstanceStatus { Status = "New", Description = "The semester has been created but not modified" };
            var preparingStatus = new SemesterInstanceStatus { Status = "Preparing", Description = "The semester is open for wishlist submissions" };
            var schedulingStatus = new SemesterInstanceStatus { Status = "Scheduling", Description = "The semester schedule is being built" };

            var semesterInstanceStatuses = new List<SemesterInstanceStatus>
            {
                newStatus,
                preparingStatus,
                schedulingStatus,
                new SemesterInstanceStatus { Status = "Finalizing", Description = "The semester schedule is complete and ready for approval" },
                new SemesterInstanceStatus { Status = "Approved", Description = "The semester is approved and ready to be uploaded" },
                new SemesterInstanceStatus { Status = "Posted", Description = "The semester has been finalized and uploaded to Banner" }
            };

            foreach (var semesterInstanceStatus in semesterInstanceStatuses)
            {
                _context.SemesterInstanceStatuses.Add(semesterInstanceStatus);
            }

            _context.SaveChanges();
            #endregion

            #region Semester Instances
            //var fall2023 = new SemesterInstance
            //{
            //    Name = "Fall 2023",
            //    Semester = fallSemester,
            //    SemesterInstanceStatus = schedulingStatus,
            //    SemesterRegistrationStart = DateTime.Parse("04/10/2023"),
            //    SemesterRegistrationEnd = DateTime.Parse("08/28/2023"),
            //    SemesterStartDate = DateTime.Parse("08/28/2023"),
            //    SemesterEndDate = DateTime.Parse("12/16/2023"),
            //};

            //var spring2024 = new SemesterInstance
            //{
            //    Name = "Spring 2024",
            //    Semester = springSemester,
            //    SemesterInstanceStatus = preparingStatus,
            //    SemesterRegistrationStart = DateTime.Parse("11/06/2023"),
            //    SemesterRegistrationEnd = DateTime.Parse("01/12/2024"),
            //    SemesterStartDate = DateTime.Parse("01/12/2024"),
            //    SemesterEndDate = DateTime.Parse("04/26/2024"),
            //};

            //var summer2024 = new SemesterInstance
            //{
            //    Name = "Summer 2024",
            //    Semester = summerSemester,
            //    SemesterInstanceStatus = newStatus,
            //    SemesterRegistrationStart = DateTime.Parse("04/01/2024"),
            //    SemesterRegistrationEnd = DateTime.Parse("05/06/2024"),
            //    SemesterStartDate = DateTime.Parse("05/06/2024"),
            //    SemesterEndDate = DateTime.Parse("08/14/2024"),
            //};

            //var semesterInstances = new List<SemesterInstance>
            //{
            //    fall2023,
            //    spring2024,
            //    summer2024,
            //};

            //foreach (var semesterInstance in semesterInstances)
            //{
            //    _context.SemesterInstances.Add(semesterInstance);
            //}

            //_context.SaveChanges();
            #endregion

            #region Section Statuses
            var tentative = new SectionStatus { SectionStatusName = "Tentative", SectionStatusColor = "gray" };

            var sectionStatuses = new List<SectionStatus>
            {
                tentative,
                new SectionStatus { SectionStatusName = "Pending", SectionStatusColor = "yellow" },
                new SectionStatus { SectionStatusName = "Approved", SectionStatusColor = "green" },
            };

            foreach (var sectionStatus in sectionStatuses)
            {
                _context.SectionStatuses.Add(sectionStatus);
            }

            _context.SaveChanges();
            #endregion

            #region Section Times
            var sectionTime1 = new SectionTime { CreditHours = 3, DaysPerWeek = 1, StartTime = new TimeSpan(7, 30, 0), EndTime = new TimeSpan(10, 10, 0), DisplayText = "7:30 AM - 10:10 AM" };
            var sectionTime2 = new SectionTime { CreditHours = 3, DaysPerWeek = 1, StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(11, 10, 0), DisplayText = "8:30 AM - 11:10 AM" };
            var sectionTime3 = new SectionTime { CreditHours = 3, DaysPerWeek = 1, StartTime = new TimeSpan(9, 30, 0), EndTime = new TimeSpan(12, 10, 0), DisplayText = "9:30 AM - 12:10 PM" };
            var sectionTime4 = new SectionTime { CreditHours = 3, DaysPerWeek = 1, StartTime = new TimeSpan(10, 30, 0), EndTime = new TimeSpan(13, 10, 0), DisplayText = "10:30 AM - 1:10 PM" };
            var sectionTime5 = new SectionTime { CreditHours = 3, DaysPerWeek = 1, StartTime = new TimeSpan(11, 30, 0), EndTime = new TimeSpan(14, 10, 0), DisplayText = "11:30 AM - 2:10 PM" };
            var sectionTime6 = new SectionTime { CreditHours = 3, DaysPerWeek = 1, StartTime = new TimeSpan(12, 30, 0), EndTime = new TimeSpan(15, 10, 0), DisplayText = "12:30 PM - 3:10 PM" };
            var sectionTime7 = new SectionTime { CreditHours = 3, DaysPerWeek = 1, StartTime = new TimeSpan(13, 30, 0), EndTime = new TimeSpan(16, 10, 0), DisplayText = "1:30 PM - 4:10 PM" };
            var sectionTime8 = new SectionTime { CreditHours = 3, DaysPerWeek = 1, StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(17, 10, 0), DisplayText = "2:30 PM - 5:10 PM" };
            var sectionTime9 = new SectionTime { CreditHours = 3, DaysPerWeek = 1, StartTime = new TimeSpan(15, 30, 0), EndTime = new TimeSpan(18, 10, 0), DisplayText = "3:30 PM - 6:10 PM" };
            var sectionTime10 = new SectionTime { CreditHours = 3, DaysPerWeek = 1, StartTime = new TimeSpan(16, 30, 0), EndTime = new TimeSpan(19, 10, 0), DisplayText = "4:30 PM - 7:10 PM" };
            var sectionTime11 = new SectionTime { CreditHours = 3, DaysPerWeek = 1, StartTime = new TimeSpan(17, 30, 0), EndTime = new TimeSpan(20, 10, 0), DisplayText = "5:30 PM - 8:10 PM" };
            var sectionTime12 = new SectionTime { CreditHours = 3, DaysPerWeek = 1, StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(21, 10, 0), DisplayText = "6:30 PM - 9:10 PM" };

            var sectionTime13 = new SectionTime { CreditHours = 3, DaysPerWeek = 2, StartTime = new TimeSpan(7, 30, 0), EndTime = new TimeSpan(8, 45, 0), DisplayText = "7:30 AM - 8:45 AM" };
            var sectionTime14 = new SectionTime { CreditHours = 3, DaysPerWeek = 2, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 15, 0), DisplayText = "9:00 AM - 10:15 AM" };
            var sectionTime15 = new SectionTime { CreditHours = 3, DaysPerWeek = 2, StartTime = new TimeSpan(10, 30, 0), EndTime = new TimeSpan(11, 45, 0), DisplayText = "10:30 AM - 11:45 AM" };
            var sectionTime16 = new SectionTime { CreditHours = 3, DaysPerWeek = 2, StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(13, 15, 0), DisplayText = "12:00 PM - 1:15 PM" };
            var sectionTime17 = new SectionTime { CreditHours = 3, DaysPerWeek = 2, StartTime = new TimeSpan(13, 30, 0), EndTime = new TimeSpan(14, 45, 0), DisplayText = "1:30 PM - 2:45 PM" };
            var sectionTime18 = new SectionTime { CreditHours = 3, DaysPerWeek = 2, StartTime = new TimeSpan(15, 0, 0), EndTime = new TimeSpan(16, 15, 0), DisplayText = "3:00 PM - 4:15 PM" };
            var sectionTime19 = new SectionTime { CreditHours = 3, DaysPerWeek = 2, StartTime = new TimeSpan(16, 30, 0), EndTime = new TimeSpan(17, 45, 0), DisplayText = "4:30 PM - 5:45 PM" };
            var sectionTime20 = new SectionTime { CreditHours = 3, DaysPerWeek = 2, StartTime = new TimeSpan(18, 0, 0), EndTime = new TimeSpan(19, 15, 0), DisplayText = "6:00 PM - 7:15 PM" };
            var sectionTime21 = new SectionTime { CreditHours = 3, DaysPerWeek = 2, StartTime = new TimeSpan(19, 30, 0), EndTime = new TimeSpan(20, 45, 0), DisplayText = "7:30 PM - 8:45 PM" };

            var sectionTime22 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(7, 30, 0), EndTime = new TimeSpan(8, 20, 0), DisplayText = "7:30 AM - 8:20 AM" };
            var sectionTime23 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(9, 20, 0), DisplayText = "8:30 AM - 9:20 AM" };
            var sectionTime24 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(9, 30, 0), EndTime = new TimeSpan(10, 20, 0), DisplayText = "9:30 AM - 10:20 AM" };
            var sectionTime25 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(10, 30, 0), EndTime = new TimeSpan(11, 20, 0), DisplayText = "10:30 AM - 11:20 AM" };
            var sectionTime26 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(11, 30, 0), EndTime = new TimeSpan(12, 20, 0), DisplayText = "11:30 AM - 12:20 PM" };
            var sectionTime27 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(12, 30, 0), EndTime = new TimeSpan(13, 20, 0), DisplayText = "12:30 PM - 1:20 PM" };
            var sectionTime28 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(13, 30, 0), EndTime = new TimeSpan(14, 20, 0), DisplayText = "1:30 PM - 2:20 PM" };
            var sectionTime29 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(15, 20, 0), DisplayText = "2:30 PM - 3:20 PM" };
            var sectionTime30 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(15, 30, 0), EndTime = new TimeSpan(16, 20, 0), DisplayText = "3:30 PM - 4:20 PM" };
            var sectionTime31 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(16, 30, 0), EndTime = new TimeSpan(17, 20, 0), DisplayText = "4:30 PM - 5:20 PM" };
            var sectionTime32 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(17, 30, 0), EndTime = new TimeSpan(18, 20, 0), DisplayText = "5:30 PM - 6:20 PM" };
            var sectionTime33 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(19, 20, 0), DisplayText = "6:30 PM - 7:20 PM" };
            var sectionTime34 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(19, 30, 0), EndTime = new TimeSpan(20, 20, 0), DisplayText = "7:30 PM - 8:20 PM" };
            var sectionTime35 = new SectionTime { CreditHours = 3, DaysPerWeek = 3, StartTime = new TimeSpan(20, 30, 0), EndTime = new TimeSpan(21, 20, 0), DisplayText = "8:30 PM - 9:20 PM" };

            var sectionTime36 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(7, 30, 0), EndTime = new TimeSpan(9, 20, 0), DisplayText = "7:30 AM - 9:20 AM" };
            var sectionTime37 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(10, 20, 0), DisplayText = "8:30 AM - 10:20 AM" };
            var sectionTime38 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(9, 30, 0), EndTime = new TimeSpan(11, 20, 0), DisplayText = "9:30 AM - 11:20 AM" };
            var sectionTime39 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(10, 30, 0), EndTime = new TimeSpan(12, 20, 0), DisplayText = "10:30 AM - 12:20 PM" };
            var sectionTime40 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(11, 30, 0), EndTime = new TimeSpan(13, 20, 0), DisplayText = "11:30 AM - 1:20 PM" };
            var sectionTime41 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(12, 30, 0), EndTime = new TimeSpan(14, 20, 0), DisplayText = "12:30 PM - 2:20 PM" };
            var sectionTime42 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(13, 30, 0), EndTime = new TimeSpan(15, 20, 0), DisplayText = "1:30 PM - 3:20 PM" };
            var sectionTime43 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(16, 20, 0), DisplayText = "2:30 PM - 4:20 PM" };
            var sectionTime44 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(15, 30, 0), EndTime = new TimeSpan(17, 20, 0), DisplayText = "3:30 PM - 5:20 PM" };
            var sectionTime45 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(16, 30, 0), EndTime = new TimeSpan(18, 20, 0), DisplayText = "4:30 PM - 6:20 PM" };
            var sectionTime46 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(17, 30, 0), EndTime = new TimeSpan(19, 20, 0), DisplayText = "5:30 PM - 7:20 PM" };
            var sectionTime47 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(20, 20, 0), DisplayText = "6:30 PM - 8:20 PM" };
            var sectionTime48 = new SectionTime { CreditHours = 4, DaysPerWeek = 2, StartTime = new TimeSpan(19, 30, 0), EndTime = new TimeSpan(21, 20, 0), DisplayText = "7:30 PM - 9:20 PM" };

            var sectionTime49 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(7, 30, 0), EndTime = new TimeSpan(8, 20, 0), DisplayText = "7:30 AM - 8:20 AM" };
            var sectionTime50 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(9, 20, 0), DisplayText = "8:30 AM - 9:20 AM" };
            var sectionTime51 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(9, 30, 0), EndTime = new TimeSpan(10, 20, 0), DisplayText = "9:30 AM - 10:20 AM" };
            var sectionTime52 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(10, 30, 0), EndTime = new TimeSpan(11, 20, 0), DisplayText = "10:30 AM - 11:20 AM" };
            var sectionTime53 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(11, 30, 0), EndTime = new TimeSpan(12, 20, 0), DisplayText = "11:30 AM - 12:20 PM" };
            var sectionTime54 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(12, 30, 0), EndTime = new TimeSpan(13, 20, 0), DisplayText = "12:30 PM - 1:20 PM" };
            var sectionTime55 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(13, 30, 0), EndTime = new TimeSpan(14, 20, 0), DisplayText = "1:30 PM - 2:20 PM" };
            var sectionTime56 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(14, 30, 0), EndTime = new TimeSpan(15, 20, 0), DisplayText = "2:30 PM - 3:20 PM" };
            var sectionTime57 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(15, 30, 0), EndTime = new TimeSpan(16, 20, 0), DisplayText = "3:30 PM - 4:20 PM" };
            var sectionTime58 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(16, 30, 0), EndTime = new TimeSpan(17, 20, 0), DisplayText = "4:30 PM - 5:20 PM" };
            var sectionTime59 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(17, 30, 0), EndTime = new TimeSpan(18, 20, 0), DisplayText = "5:30 PM - 6:20 PM" };
            var sectionTime60 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(19, 20, 0), DisplayText = "6:30 PM - 7:20 PM" };
            var sectionTime61 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(19, 30, 0), EndTime = new TimeSpan(20, 20, 0), DisplayText = "7:30 PM - 8:20 PM" };
            var sectionTime62 = new SectionTime { CreditHours = 4, DaysPerWeek = 4, StartTime = new TimeSpan(20, 30, 0), EndTime = new TimeSpan(21, 20, 0), DisplayText = "8:30 PM - 9:20 PM" };

            var sectionTimes = new List<SectionTime>
            {
                sectionTime1,
                sectionTime2,
                sectionTime3,
                sectionTime4,
                sectionTime5,
                sectionTime6,
                sectionTime7,
                sectionTime8,
                sectionTime9,
                sectionTime10,
                sectionTime11,
                sectionTime12,
                sectionTime13,
                sectionTime14,
                sectionTime15,
                sectionTime16,
                sectionTime17,
                sectionTime18,
                sectionTime19,
                sectionTime20,
                sectionTime21,
                sectionTime22,
                sectionTime23,
                sectionTime24,
                sectionTime25,
                sectionTime26,
                sectionTime27,
                sectionTime28,
                sectionTime29,
                sectionTime30,
                sectionTime31,
                sectionTime32,
                sectionTime33,
                sectionTime34,
                sectionTime35,
                sectionTime36,
                sectionTime37,
                sectionTime38,
                sectionTime39,
                sectionTime40,
                sectionTime41,
                sectionTime42,
                sectionTime43,
                sectionTime44,
                sectionTime45,
                sectionTime46,
                sectionTime47,
                sectionTime48,
                sectionTime49,
                sectionTime50,
                sectionTime51,
                sectionTime52,
                sectionTime53,
                sectionTime54,
                sectionTime55,
                sectionTime56,
                sectionTime57,
                sectionTime58,
                sectionTime59,
                sectionTime60,
                sectionTime61,
                sectionTime62,
            };

            foreach (var sectionTime in sectionTimes)
            {
                _context.SectionTimes.Add(sectionTime);
            }

            _context.SaveChanges();
            #endregion

            #region Instructor Release Times
            //foreach (var instructor in instructors)
            //{
            //    //randomly add release time
            //    if (random.Next(10) == 0)
            //    {
            //        var releaseTime = new ReleaseTime
            //        {
            //            Instructor = instructor,
            //            SemesterInstance = fall2023,
            //            ReleaseTimeAmount = random.Next(2, 9),
            //            ReleaseTimeNotes = "Mock Data"
            //        };

            //        _context.ReleaseTimes.Add(releaseTime);
            //        _context.SaveChanges();
            //    }
            //}
            #endregion

            #region Instructor Load Requirements
            //foreach (var semesterInstance in semesterInstances)
            //{
            //    foreach (var instructor in instructors)
            //    {
            //        var loadRequirement = new LoadRequirement
            //        {
            //            Instructor = instructor,
            //            SemesterInstance = semesterInstance,
            //            LoadReqHours = semesterInstance.Semester?.DefaultLoad ?? 0,
            //        };

            //        _context.LoadRequirements.Add(loadRequirement);
            //        _context.SaveChanges();
            //    }
            //}
            #endregion

            #region Sections
            //foreach (var assignment in templateAssignments.Where(t => t.Template == fallTemplate))
            //{
            //    var sections = new List<Section>();

            //    for (int i = 0; i < assignment.TotalCount; i++)
            //    {
            //        //randomize instructors, modalities, campuses, days-of-week, and time blocks
            //        var sectionInstructor = instructors.OrderBy(t => Guid.NewGuid()).First();
            //        var sectionModality = modalities.OrderBy(t => Guid.NewGuid()).First();
            //        Classroom? sectionClassroom = null;
            //        int sectionMaxEnrollment = random.Next(20, 46);
            //        string sectionDaysOfWeek = string.Empty;
            //        TimeSpan sectionStartTime = TimeSpan.Zero;
            //        TimeSpan sectionEndTime = TimeSpan.Zero;
            //        SectionTime? sectionTime = null;
            //        var sectionPartOfTerm = partsOfTerm.OrderBy(t => Guid.NewGuid()).First();

            //        if (sectionModality.HasCampuses)
            //        {
            //            sectionClassroom = classrooms.OrderBy(t => Guid.NewGuid()).First();
            //            sectionMaxEnrollment = sectionClassroom.ClassroomCapacity ?? sectionMaxEnrollment;
            //        }

            //        if (sectionModality.HasTimeBlocks)
            //        {
            //            //randomly select days of the week
            //            sectionDaysOfWeek = daysOfWeek.ElementAt(random.Next(daysOfWeek.Count));

            //            //randomly select a section time
            //            sectionTime = sectionTimes
            //                .Where(s => s.CreditHours == assignment.Course?.CreditHours && s.DaysPerWeek == sectionDaysOfWeek.Count())
            //                .OrderBy(t => Guid.NewGuid()).FirstOrDefault();
            //        }

            //        var section = new Section
            //        {
            //            SemesterInstance = fall2023,
            //            CourseId = assignment.CourseId,
            //            SectionStatus = tentative,
            //            InstructorId = sectionInstructor.Id,
            //            ModalityId = sectionModality.Id,
            //            ClassroomId = sectionClassroom?.Id,
            //            DaysOfWeek = sectionDaysOfWeek,
            //            SectionTime = sectionTime,
            //            MaxEnrollment = sectionMaxEnrollment,
            //            PartOfTermId = sectionPartOfTerm.Id,
            //        };

            //        sections.Add(section);
            //        _context.Sections.Add(section);

            //        _context.SaveChanges();

            //        int instructorCurrentCredits = _context.Sections
            //            .Where(s => s.InstructorId == sectionInstructor.Id)
            //            .Include(t => t.Course)
            //            .Select(t => t.Course.CreditHours)
            //            .Sum();
            //        int instructorLoad = _context.LoadRequirements.Where(l => l.InstructorId == sectionInstructor.Id).First().LoadReqHours;

            //        int creditsLeft = _context.Courses.Where(c => c.Id == assignment.CourseId).First().CreditHours;

            //        var sectionPays = new List<SectionPay>();

            //        //the course will put the instructor into overload
            //        if (instructorCurrentCredits + creditsLeft > instructorLoad)
            //        {
            //            var overloadCredits = instructorCurrentCredits + creditsLeft - instructorLoad > creditsLeft ? creditsLeft : instructorCurrentCredits + creditsLeft - instructorLoad;
            //            creditsLeft -= overloadCredits;

            //            var sectionFundingSource = fundingSources.OrderBy(t => Guid.NewGuid()).First();

            //            _context.SectionPays.Add(new SectionPay
            //            {
            //                Section = section,
            //                PayModelId = overloadPayModel.Id,
            //                PayOrgId = sectionFundingSource.Id,
            //                CreditHours = overloadCredits,
            //            });
            //        }

            //        if (creditsLeft > 0)
            //        {
            //            var sectionFundingSource = fundingSources.OrderBy(t => Guid.NewGuid()).First();

            //            _context.SectionPays.Add(new SectionPay
            //            {
            //                Section = section,
            //                PayModelId = loadPayModel.Id,
            //                PayOrgId = sectionFundingSource.Id,
            //                CreditHours = creditsLeft,
            //            });
            //        }

            //        _context.SaveChanges();
            //    }
            //}
            #endregion

            #region Instructor Wishlists
            //foreach (var instructor in instructors)
            //{
            //    var wishlist = new InstructorWishlist
            //    {
            //        Instructor = instructor,
            //        SemesterInstance = fall2023,
            //    };

            //    _context.InstructorWishlists.Add(wishlist);
            //    _context.SaveChanges();

            //    //randomly add between 0 and 5 courses to the wishlist
            //    var randomCourses = courses.OrderBy(t => Guid.NewGuid()).Take(random.Next(5));
            //    int i = 1;

            //    foreach (var course in randomCourses)
            //    {
            //        //randomize modalities, campuses, days-of-week, and time blocks
            //        var wishlistModalities = modalities.OrderBy(t => Guid.NewGuid()).Take(random.Next(modalities.Count)).ToList();
            //        var wishlistCampuses = new List<Campus>();
            //        var wishlistDaysOfWeek = string.Empty;
            //        var wishlistTimeBlocks = new List<TimeBlock>();

            //        if (wishlistModalities.Any(t => t.HasCampuses))
            //        {
            //            wishlistCampuses = campuses.OrderBy(t => Guid.NewGuid()).Take(random.Next(campuses.Count)).ToList();
            //        }

            //        if (wishlistModalities.Any(t => t.HasTimeBlocks))
            //        {
            //            wishlistDaysOfWeek = daysOfWeek.ElementAt(random.Next(daysOfWeek.Count));
            //            wishlistTimeBlocks = timeBlocks
            //                .Where(t => t.TimeBlockType == instructorTimeBlock)
            //                .OrderBy(t => Guid.NewGuid())
            //                .Take(random.Next(timeBlocks.Where(t => t.TimeBlockType == instructorTimeBlock).Count()))
            //                .ToList();
            //        }

            //        var wishlistCourse = new InstructorWishlistCourse
            //        {
            //            InstructorWishlist = wishlist,
            //            Course = course,
            //            Ranking = i++,
            //            Modalities = wishlistModalities,
            //            Campuses = wishlistCampuses,
            //            DaysOfWeek = wishlistDaysOfWeek,
            //            TimeBlocks = wishlistTimeBlocks,
            //        };

            //        _context.InstructorWishlistCourses.Add(wishlistCourse);
            //    }

            //    _context.SaveChanges();
            //}
            #endregion

            #region Student Wishlists
            //foreach (var student in students)
            //{
            //    var wishlist = new StudentWishlist
            //    {
            //        Student = student,
            //        SemesterInstance = fall2023,
            //    };

            //    _context.StudentWishlists.Add(wishlist);
            //    _context.SaveChanges();

            //    //randomly add between 0 and 5 courses to the wishlist
            //    var randomCourses = courses.OrderBy(t => Guid.NewGuid()).Take(random.Next(5));

            //    foreach (var course in randomCourses)
            //    {
            //        //randomize modalities, campuses, and time blocks
            //        var wishlistModalities = modalities.OrderBy(t => Guid.NewGuid()).Take(random.Next(modalities.Count)).ToList();
            //        var wishlistCampuses = new List<Campus>();
            //        var wishlistTimeBlocks = new List<TimeBlock>();

            //        if (wishlistModalities.Any(t => t.HasCampuses))
            //        {
            //            wishlistCampuses = campuses.OrderBy(t => Guid.NewGuid()).Take(random.Next(campuses.Count)).ToList();
            //        }

            //        if (wishlistModalities.Any(t => t.HasTimeBlocks))
            //        {
            //            wishlistTimeBlocks = timeBlocks
            //                .Where(t => t.TimeBlockType == studentTimeBlock)
            //                .OrderBy(t => Guid.NewGuid())
            //                .Take(random.Next(timeBlocks.Where(t => t.TimeBlockType == studentTimeBlock).Count()))
            //                .ToList();
            //        }

            //        var wishlistCourse = new StudentWishlistCourse
            //        {
            //            StudentWishlist = wishlist,
            //            Course = course,
            //            Modalities = wishlistModalities,
            //            Campuses = wishlistCampuses,
            //            TimeBlocks = wishlistTimeBlocks,
            //        };

            //        _context.StudentWishlistCourses.Add(wishlistCourse);
            //    }

            //    _context.SaveChanges();
            //}
            #endregion
        }
    }
}