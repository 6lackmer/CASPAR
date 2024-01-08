using CsvHelper;
using DataAccess;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Utility;
using CsvHelper.Configuration.Attributes;

namespace CASPAR.Pages.Schedule
{
	[Authorize(Roles = SD.AdminRole + "," + SD.FlexCoordinatorRole + "," + SD.ProgramCoordinatorRole + "," + SD.GraduateCoordinatorRole + "," + SD.SecretaryRole)]
	public class PreviewModel : PageModel
	{

		private readonly ApplicationDbContext _context;

		private readonly IUnitOfWork _unitOfWork;

		public SemesterInstance SemesterInstance { get; set; } = new();

		public IEnumerable<Section> Sections { get; set; } = new List<Section>();

		public bool AllSectionsAssigned { get; set; }

		public string ReturnRoute { get; set; } = "index";

		public PreviewModel(ApplicationDbContext context, IUnitOfWork unitOfWork)
		{
			_context = context;
			_unitOfWork = unitOfWork;
		}

		public IActionResult OnGet(int id, string returnRoute)
		{
			ReturnRoute = returnRoute;
			var semesterInstance = _unitOfWork.SemesterInstance.Get(t => t.Id == id, false, "Semester,SemesterInstanceStatus");

			if (semesterInstance != null)
			{
				SemesterInstance = semesterInstance;
				Sections = _unitOfWork.Section.GetAll(t => t.SemesterInstanceId == id, t => t.Id,
                    "SemesterInstance,Course,Instructor,Instructor.ApplicationUser,Modality,Classroom,Classroom.Building,Classroom.Building.Campus,PartOfTerm,SectionPays,SectionPays.PayModel,SectionPays.PayOrg,SectionStatus,SectionTime"
                );
			}
            AllSectionsAssigned = _context.Sections
				.Where(t => t.SemesterInstanceId == SemesterInstance.Id)
				.All(t =>
					t.InstructorId != null &&
					t.ModalityId != null &&
					t.PartOfTermId != null &&
					(!t.Modality.HasCampuses || (t.Modality.HasCampuses && t.ClassroomId != null)) &&
					(!t.Modality.HasTimeBlocks || (t.Modality.HasTimeBlocks && t.DaysOfWeek != string.Empty && t.SectionTime != null)) &&
					t.SectionPays != null && t.SectionPays.Count > 0
				);

			return Page();
		}

		public IActionResult OnPost()
		{
			var id = int.Parse(Request.Query["id"]);

			var semesterInstance = _unitOfWork.SemesterInstance.Get(t => t.Id == id, false, "Semester,SemesterInstanceStatus");

			if (semesterInstance != null)
			{
				SemesterInstance = semesterInstance;
				Sections = _unitOfWork.Section.GetAll(t => t.SemesterInstanceId == id, t => t.Id,
                    "SemesterInstance,Course,Instructor,Instructor.ApplicationUser,Modality,Classroom,Classroom.Building,Classroom.Building.Campus,PartOfTerm,SectionPays,SectionPays.PayModel,SectionPays.PayOrg,SectionStatus,SectionTime"
                );
			}
			var formattedCSVObjects = new List<FormattedCSV>();

			var currentFormattedCSV = new FormattedCSV();
			foreach (var el in Sections)
			{
				currentFormattedCSV =
					new FormattedCSV
					{
						Course = el.Course?.CourseNumber ?? "",
						InstructorCredits = el.SectionPays?.Select(s => s.CreditHours).Sum().ToString() ?? "",
						StudentCredits = el.Course?.CreditHours.ToString() ?? "",
						Instructor = el.Instructor?.ApplicationUser?.FirstName + " " + el.Instructor?.ApplicationUser?.LastName,
						Modality = el.Modality?.ModalityName ?? "",
						Campus = el.Classroom?.Building?.Campus?.CampusName,
						Days = el.DaysOfWeek,
						Time = el.SectionTime?.DisplayText ?? "",
						PartOfTerm = el.PartOfTerm?.PartOfTermTitle ?? "",
						BannerCRN = el.BannerCRN > 0 ? el.BannerCRN.ToString() : "",
						EnrollmentMax = el.MaxEnrollment.ToString(),
						CurrentEnrollment = el.CurrentEnrollment.ToString(),
						Waitlist = el.Waitlist.ToString(),
						FinalEnrollment = el.FinalEnrollment.ToString(),
						Notes = el.SectionNotes,
						SectionStatus = el.SectionStatus.SectionStatusName
					};

				if (el.ClassroomId != null)
				{
					currentFormattedCSV.Classroom = el.Classroom?.Building?.Name + ", Room " + el.Classroom?.ClassroomNumber;
				}
				if (el.SectionPays != null && el.SectionPays.Count() > 0)
				{
					currentFormattedCSV.PayModels = string.Join(", ", el.SectionPays?.Select(s => s.PayModel?.PayModelTitle + " paid by " + s.PayOrg?.PayOrgTitle + " (" + s.CreditHours + " credits)") ?? new List<string>());

                }
				formattedCSVObjects.Add(currentFormattedCSV);
			}

			using (var writer = new StringWriter())
			using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
			{
				csv.WriteRecords(formattedCSVObjects);
				var result = writer.ToString();
				return File(new System.Text.UTF8Encoding().GetBytes(result), "text/csv", "Schedule - " + SemesterInstance.Name + ".csv");
			}
		}

		public async Task<IActionResult> OnPostUpdateFieldAsync([FromBody] FieldUpdateModel model)
		{
			var section = await _context.Sections.FindAsync(model.SectionId);

			if (section == null)
			{
				return NotFound();
			}

            switch (model.FieldName)
			{
				case "BannerCRN":
					section.BannerCRN = int.Parse(model.Value);
					break;
                case "CurrentEnrollment":
                    section.CurrentEnrollment = int.Parse(model.Value);
                    break;
                case "Waitlist":
                    section.Waitlist = int.Parse(model.Value);
                    break;
				case "FinalEnrollment":
					section.FinalEnrollment = int.Parse(model.Value);
					break;
            }

			await _context.SaveChangesAsync();

			return new JsonResult(new { success = true });
		}

		public class FieldUpdateModel
		{
			public int SectionId { get; set; }
			public string FieldName { get; set; }
			public string Value { get; set; }
		}

		public class FormattedCSV
		{
			public string Course { get; set; }

            [Name("Instructor Credits")]
            public string InstructorCredits { get; set; }

            [Name("Student Credits")]
            public string StudentCredits { get; set; }
            public string Instructor { get; set; }
			public string Modality { get; set; }
			public string? Campus { get; set; }
			public string? Classroom { get; set; }
			public string? Days { get; set; }
			public string? Time { get; set; }

			[Name("Part-Of-Term")]
			public string PartOfTerm { get; set; }

			[Name("Pay Models")]
			public string PayModels { get; set; }

			[Name("Banner CRN")]
			public string? BannerCRN { get; set; }

            [Name("Enrollment Max")]
            public string EnrollmentMax { get; set; }

			/*
            [Name("Enrollment W1")]
            public string EnrollmentWeek1 { get; set; }

            [Name("Enrollment W3")]
            public string EnrollmentWeek3 { get; set; }
			*/

			// Added the following 3 to replace the above 2
			[Name("Current Enrollment")]
			public string CurrentEnrollment { get; set; }

			[Name("Waitlist")]
			public string Waitlist { get; set; }

			[Name("Final Enrollment")]
			public string FinalEnrollment { get; set; }

			[Name("Section Status")]
			public string SectionStatus { get; set; }

            public string? Notes { get; set; }
		}
	}
}