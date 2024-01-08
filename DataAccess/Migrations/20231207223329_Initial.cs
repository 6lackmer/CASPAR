using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Campuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampusName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MajorName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modalitys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModalityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasCampuses = table.Column<bool>(type: "bit", nullable: false),
                    HasTimeBlocks = table.Column<bool>(type: "bit", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modalitys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PartOfTerms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartOfTermTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartOfTerms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayModelTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayOrgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayOrgTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayOrgs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SectionStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionStatusName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectionStatusColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SectionTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreditHours = table.Column<int>(type: "int", nullable: false),
                    DaysPerWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DisplayText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionTimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SemesterInstanceStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterInstanceStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultLoad = table.Column<int>(type: "int", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeBlockTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeBlockTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    InstructorTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instructors_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Buildings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampusId = table.Column<int>(type: "int", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buildings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buildings_Campuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MajorId = table.Column<int>(type: "int", nullable: true),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    GraduationYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Majors_MajorId",
                        column: x => x.MajorId,
                        principalTable: "Majors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SemesterInstances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    SemesterInstanceStatusId = table.Column<int>(type: "int", nullable: false),
                    SemesterRegistrationStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SemesterRegistrationEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SemesterStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SemesterEndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterInstances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SemesterInstances_SemesterInstanceStatuses_SemesterInstanceStatusId",
                        column: x => x.SemesterInstanceStatusId,
                        principalTable: "SemesterInstanceStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SemesterInstances_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeBlockTypeId = table.Column<int>(type: "int", nullable: false),
                    TimeBlockName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeBlocks_TimeBlockTypes_TimeBlockTypeId",
                        column: x => x.TimeBlockTypeId,
                        principalTable: "TimeBlockTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcademicPrograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgramCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstructorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicPrograms_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildingId = table.Column<int>(type: "int", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    ClassroomDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassroomCapacity = table.Column<int>(type: "int", nullable: true),
                    ClassroomNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classrooms_Buildings_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Buildings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstructorWishlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterInstanceId = table.Column<int>(type: "int", nullable: false),
                    InstructorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorWishlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructorWishlists_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorWishlists_SemesterInstances_SemesterInstanceId",
                        column: x => x.SemesterInstanceId,
                        principalTable: "SemesterInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoadRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructorId = table.Column<int>(type: "int", nullable: false),
                    SemesterInstanceId = table.Column<int>(type: "int", nullable: false),
                    LoadReqHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadRequirements_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoadRequirements_SemesterInstances_SemesterInstanceId",
                        column: x => x.SemesterInstanceId,
                        principalTable: "SemesterInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructorId = table.Column<int>(type: "int", nullable: false),
                    SemesterInstanceId = table.Column<int>(type: "int", nullable: false),
                    ReleaseTimeAmount = table.Column<int>(type: "int", nullable: false),
                    ReleaseTimeNotes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseTimes_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReleaseTimes_SemesterInstances_SemesterInstanceId",
                        column: x => x.SemesterInstanceId,
                        principalTable: "SemesterInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentWishlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterInstanceId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentWishlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentWishlists_SemesterInstances_SemesterInstanceId",
                        column: x => x.SemesterInstanceId,
                        principalTable: "SemesterInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentWishlists_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    CourseTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditHours = table.Column<int>(type: "int", nullable: false),
                    CourseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_AcademicPrograms_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "AcademicPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassroomFeatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    ClassroomId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassroomFeatures_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InstructorWishlistCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructorWishlistId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Ranking = table.Column<int>(type: "int", nullable: false),
                    DaysOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorWishlistCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructorWishlistCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorWishlistCourses_InstructorWishlists_InstructorWishlistId",
                        column: x => x.InstructorWishlistId,
                        principalTable: "InstructorWishlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrereqCourseAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    PrereqCourseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrereqCourseAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrereqCourseAssignments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrereqCourseAssignments_Courses_PrereqCourseId",
                        column: x => x.PrereqCourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SemesterInstanceId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    InstructorId = table.Column<int>(type: "int", nullable: true),
                    ModalityId = table.Column<int>(type: "int", nullable: true),
                    ClassroomId = table.Column<int>(type: "int", nullable: true),
                    PartOfTermId = table.Column<int>(type: "int", nullable: true),
                    SectionStatusId = table.Column<int>(type: "int", nullable: false),
                    SectionTimeId = table.Column<int>(type: "int", nullable: true),
                    DaysOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxEnrollment = table.Column<int>(type: "int", nullable: false),
                    CurrentEnrollment = table.Column<int>(type: "int", nullable: false),
                    Waitlist = table.Column<int>(type: "int", nullable: false),
                    FinalEnrollment = table.Column<int>(type: "int", nullable: false),
                    SectionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannerCRN = table.Column<int>(type: "int", nullable: false),
                    SectionUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SectionBannerUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sections_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sections_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sections_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sections_Modalitys_ModalityId",
                        column: x => x.ModalityId,
                        principalTable: "Modalitys",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sections_PartOfTerms_PartOfTermId",
                        column: x => x.PartOfTermId,
                        principalTable: "PartOfTerms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sections_SectionStatuses_SectionStatusId",
                        column: x => x.SectionStatusId,
                        principalTable: "SectionStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sections_SectionTimes_SectionTimeId",
                        column: x => x.SectionTimeId,
                        principalTable: "SectionTimes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sections_SemesterInstances_SemesterInstanceId",
                        column: x => x.SemesterInstanceId,
                        principalTable: "SemesterInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentWishlistCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentWishlistId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentWishlistCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentWishlistCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentWishlistCourses_StudentWishlists_StudentWishlistId",
                        column: x => x.StudentWishlistId,
                        principalTable: "StudentWishlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    TotalCount = table.Column<int>(type: "int", nullable: false),
                    FaceToFaceCount = table.Column<int>(type: "int", nullable: false),
                    VirtualCount = table.Column<int>(type: "int", nullable: false),
                    OnlineCount = table.Column<int>(type: "int", nullable: false),
                    FlexCount = table.Column<int>(type: "int", nullable: false),
                    HybridCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateAssignments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateAssignments_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampusInstructorWishlistCourse",
                columns: table => new
                {
                    CampusesId = table.Column<int>(type: "int", nullable: false),
                    InstructorWishlistCoursesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusInstructorWishlistCourse", x => new { x.CampusesId, x.InstructorWishlistCoursesId });
                    table.ForeignKey(
                        name: "FK_CampusInstructorWishlistCourse_Campuses_CampusesId",
                        column: x => x.CampusesId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampusInstructorWishlistCourse_InstructorWishlistCourses_InstructorWishlistCoursesId",
                        column: x => x.InstructorWishlistCoursesId,
                        principalTable: "InstructorWishlistCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstructorWishlistCourseModality",
                columns: table => new
                {
                    InstructorWishlistCoursesId = table.Column<int>(type: "int", nullable: false),
                    ModalitiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorWishlistCourseModality", x => new { x.InstructorWishlistCoursesId, x.ModalitiesId });
                    table.ForeignKey(
                        name: "FK_InstructorWishlistCourseModality_InstructorWishlistCourses_InstructorWishlistCoursesId",
                        column: x => x.InstructorWishlistCoursesId,
                        principalTable: "InstructorWishlistCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorWishlistCourseModality_Modalitys_ModalitiesId",
                        column: x => x.ModalitiesId,
                        principalTable: "Modalitys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InstructorWishlistCourseTimeBlock",
                columns: table => new
                {
                    InstructorWishlistCoursesId = table.Column<int>(type: "int", nullable: false),
                    TimeBlocksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorWishlistCourseTimeBlock", x => new { x.InstructorWishlistCoursesId, x.TimeBlocksId });
                    table.ForeignKey(
                        name: "FK_InstructorWishlistCourseTimeBlock_InstructorWishlistCourses_InstructorWishlistCoursesId",
                        column: x => x.InstructorWishlistCoursesId,
                        principalTable: "InstructorWishlistCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorWishlistCourseTimeBlock_TimeBlocks_TimeBlocksId",
                        column: x => x.TimeBlocksId,
                        principalTable: "TimeBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SectionPays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    PayModelId = table.Column<int>(type: "int", nullable: true),
                    PayOrgId = table.Column<int>(type: "int", nullable: true),
                    CreditHours = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionPays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionPays_PayModels_PayModelId",
                        column: x => x.PayModelId,
                        principalTable: "PayModels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SectionPays_PayOrgs_PayOrgId",
                        column: x => x.PayOrgId,
                        principalTable: "PayOrgs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SectionPays_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampusStudentWishlistCourse",
                columns: table => new
                {
                    CampusesId = table.Column<int>(type: "int", nullable: false),
                    StudentWishlistCourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampusStudentWishlistCourse", x => new { x.CampusesId, x.StudentWishlistCourseId });
                    table.ForeignKey(
                        name: "FK_CampusStudentWishlistCourse_Campuses_CampusesId",
                        column: x => x.CampusesId,
                        principalTable: "Campuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampusStudentWishlistCourse_StudentWishlistCourses_StudentWishlistCourseId",
                        column: x => x.StudentWishlistCourseId,
                        principalTable: "StudentWishlistCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModalityStudentWishlistCourse",
                columns: table => new
                {
                    ModalitiesId = table.Column<int>(type: "int", nullable: false),
                    StudentWishlistCourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModalityStudentWishlistCourse", x => new { x.ModalitiesId, x.StudentWishlistCourseId });
                    table.ForeignKey(
                        name: "FK_ModalityStudentWishlistCourse_Modalitys_ModalitiesId",
                        column: x => x.ModalitiesId,
                        principalTable: "Modalitys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModalityStudentWishlistCourse_StudentWishlistCourses_StudentWishlistCourseId",
                        column: x => x.StudentWishlistCourseId,
                        principalTable: "StudentWishlistCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentWishlistCourseTimeBlock",
                columns: table => new
                {
                    StudentWishlistCourseId = table.Column<int>(type: "int", nullable: false),
                    TimeBlocksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentWishlistCourseTimeBlock", x => new { x.StudentWishlistCourseId, x.TimeBlocksId });
                    table.ForeignKey(
                        name: "FK_StudentWishlistCourseTimeBlock_StudentWishlistCourses_StudentWishlistCourseId",
                        column: x => x.StudentWishlistCourseId,
                        principalTable: "StudentWishlistCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentWishlistCourseTimeBlock_TimeBlocks_TimeBlocksId",
                        column: x => x.TimeBlocksId,
                        principalTable: "TimeBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicPrograms_InstructorId",
                table: "AcademicPrograms",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Buildings_CampusId",
                table: "Buildings",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusInstructorWishlistCourse_InstructorWishlistCoursesId",
                table: "CampusInstructorWishlistCourse",
                column: "InstructorWishlistCoursesId");

            migrationBuilder.CreateIndex(
                name: "IX_CampusStudentWishlistCourse_StudentWishlistCourseId",
                table: "CampusStudentWishlistCourse",
                column: "StudentWishlistCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomFeatures_ClassroomId",
                table: "ClassroomFeatures",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_BuildingId",
                table: "Classrooms",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ProgramId",
                table: "Courses",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_UserId",
                table: "Instructors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWishlistCourseModality_ModalitiesId",
                table: "InstructorWishlistCourseModality",
                column: "ModalitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWishlistCourses_CourseId",
                table: "InstructorWishlistCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWishlistCourses_InstructorWishlistId",
                table: "InstructorWishlistCourses",
                column: "InstructorWishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWishlistCourseTimeBlock_TimeBlocksId",
                table: "InstructorWishlistCourseTimeBlock",
                column: "TimeBlocksId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWishlists_InstructorId",
                table: "InstructorWishlists",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorWishlists_SemesterInstanceId",
                table: "InstructorWishlists",
                column: "SemesterInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadRequirements_InstructorId",
                table: "LoadRequirements",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadRequirements_SemesterInstanceId",
                table: "LoadRequirements",
                column: "SemesterInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ModalityStudentWishlistCourse_StudentWishlistCourseId",
                table: "ModalityStudentWishlistCourse",
                column: "StudentWishlistCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_PrereqCourseAssignments_CourseId",
                table: "PrereqCourseAssignments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_PrereqCourseAssignments_PrereqCourseId",
                table: "PrereqCourseAssignments",
                column: "PrereqCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseTimes_InstructorId",
                table: "ReleaseTimes",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseTimes_SemesterInstanceId",
                table: "ReleaseTimes",
                column: "SemesterInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionPays_PayModelId",
                table: "SectionPays",
                column: "PayModelId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionPays_PayOrgId",
                table: "SectionPays",
                column: "PayOrgId");

            migrationBuilder.CreateIndex(
                name: "IX_SectionPays_SectionId",
                table: "SectionPays",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_ClassroomId",
                table: "Sections",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_CourseId",
                table: "Sections",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_InstructorId",
                table: "Sections",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_ModalityId",
                table: "Sections",
                column: "ModalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_PartOfTermId",
                table: "Sections",
                column: "PartOfTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SectionStatusId",
                table: "Sections",
                column: "SectionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SectionTimeId",
                table: "Sections",
                column: "SectionTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_SemesterInstanceId",
                table: "Sections",
                column: "SemesterInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterInstances_SemesterId",
                table: "SemesterInstances",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_SemesterInstances_SemesterInstanceStatusId",
                table: "SemesterInstances",
                column: "SemesterInstanceStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_MajorId",
                table: "Students",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWishlistCourses_CourseId",
                table: "StudentWishlistCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWishlistCourses_StudentWishlistId",
                table: "StudentWishlistCourses",
                column: "StudentWishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWishlistCourseTimeBlock_TimeBlocksId",
                table: "StudentWishlistCourseTimeBlock",
                column: "TimeBlocksId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWishlists_SemesterInstanceId",
                table: "StudentWishlists",
                column: "SemesterInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentWishlists_StudentId",
                table: "StudentWishlists",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateAssignments_CourseId",
                table: "TemplateAssignments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateAssignments_TemplateId",
                table: "TemplateAssignments",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_SemesterId",
                table: "Templates",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeBlocks_TimeBlockTypeId",
                table: "TimeBlocks",
                column: "TimeBlockTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CampusInstructorWishlistCourse");

            migrationBuilder.DropTable(
                name: "CampusStudentWishlistCourse");

            migrationBuilder.DropTable(
                name: "ClassroomFeatures");

            migrationBuilder.DropTable(
                name: "InstructorWishlistCourseModality");

            migrationBuilder.DropTable(
                name: "InstructorWishlistCourseTimeBlock");

            migrationBuilder.DropTable(
                name: "LoadRequirements");

            migrationBuilder.DropTable(
                name: "ModalityStudentWishlistCourse");

            migrationBuilder.DropTable(
                name: "PrereqCourseAssignments");

            migrationBuilder.DropTable(
                name: "ReleaseTimes");

            migrationBuilder.DropTable(
                name: "SectionPays");

            migrationBuilder.DropTable(
                name: "StudentWishlistCourseTimeBlock");

            migrationBuilder.DropTable(
                name: "TemplateAssignments");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "InstructorWishlistCourses");

            migrationBuilder.DropTable(
                name: "PayModels");

            migrationBuilder.DropTable(
                name: "PayOrgs");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "StudentWishlistCourses");

            migrationBuilder.DropTable(
                name: "TimeBlocks");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "InstructorWishlists");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "Modalitys");

            migrationBuilder.DropTable(
                name: "PartOfTerms");

            migrationBuilder.DropTable(
                name: "SectionStatuses");

            migrationBuilder.DropTable(
                name: "SectionTimes");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "StudentWishlists");

            migrationBuilder.DropTable(
                name: "TimeBlockTypes");

            migrationBuilder.DropTable(
                name: "Buildings");

            migrationBuilder.DropTable(
                name: "AcademicPrograms");

            migrationBuilder.DropTable(
                name: "SemesterInstances");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Campuses");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "SemesterInstanceStatuses");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropTable(
                name: "Majors");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
