using ATS.Domain.Entities;
using ATS.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Application.Category.Queries;
using System.Text.Json;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Text.Json.Serialization;
using ATS.Application.Resumes;


namespace ATS_CV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ApiControllercs

    {
        private readonly ApplicationDbContext _context;

        public ResumeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("createResume")]
        public async Task<IActionResult> CreateResume([FromBody] CreateResumeCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var resumeId = await Mediator.Send(command);
            return Ok(resumeId);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResumeDto>> GetResumeById(int id)
        {
            var resume = await _context.Resumes
                .Include(r => r.WorkExperiences)
                .Include(r => r.EducationDetails)
                .Include(r => r.Skills)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (resume == null)
            {
                return NotFound();
            }


            var resumeDto = new ResumeDto
            {

                FullName = resume.FullName,
                Email = resume.Email,
                Phone = resume.Phone,
                Address = resume.Address,
                Summary = resume.Summary,

                WorkExperiences = resume.WorkExperiences.Select(we => new WorkExperienceDto
                {
                    Position = we.Position,
                    Company = we.Company,
                    Responsibilities = we.Responsibilities,
                    StartDate = we.StartDate,
                    EndDate = we.EndDate

                }).ToList(),
                EducationDetails = resume.EducationDetails.Select(ed => new EducationDto
                {
                    Degree = ed.Degree,
                    Institution = ed.Institution,
                    StartDate = ed.StartDate,
                    EndDate = ed.EndDate
                }).ToList(),
                skills = resume.Skills.Select(sk => new SkillsDto
                {
                    Name = sk.Name,
                }).ToList()

            };
            string FormatDate(string date)
            {
                if (DateTime.TryParse(date, out var parsedDate))
                {
                    return parsedDate.ToString("yyyy-MM-dd");
                }
                return "Unknown";
            }

            string resumeContent = $@"
    <div style=""font-family: 'Times New Roman', serif; padding: 20px; border: 1px solid #000; max-width: 800px; margin: auto;"">
        <div style=""text-align: center; margin-bottom: 20px;"">
            <h1 style=""font-size: 32px; margin: 0;"">{resumeDto.FullName.ToUpper()}</h1>
            <p style=""margin: 5px 0; font-size: 18px; font-weight: bold;"">{resumeDto.Summary ?? "Professional Title"}</p>
            <p style=""margin: 0; font-size: 16px;"">{resumeDto.Email} | {resumeDto.Phone} | {resumeDto.Address}</p>
        </div>

        <div style=""margin-bottom: 20px;"">
            <h2 style=""border-bottom: 1px solid #000; font-size: 20px;"">Objective</h2>
            <p>{resumeDto.Summary}</p>
        </div>

        <div style=""margin-bottom: 20px;"">
            <h2 style=""border-bottom: 1px solid #000; font-size: 20px;"">Experience</h2>
             {string.Join("", resumeDto.WorkExperiences.Select(exp => $@"
            <div style=""margin-bottom: 10px;"">
                <p style=""margin: 0;""><strong>{exp.Company}</strong> | <em>{exp.Position}</em></p>
                <p style=""margin: 0; font-size: 14px;"">{FormatDate(exp.StartDate)} - {FormatDate(exp.EndDate)}</p>
                <ul style=""margin: 5px 0; padding-left: 20px;"">
                    <li>{exp.Responsibilities}</li>
                </ul>
            </div>
        "))}

        </div>

        <div style=""margin-bottom: 20px;"">
            <h2 style=""border-bottom: 1px solid #000; font-size: 20px;"">Education</h2>
            {string.Join("", resumeDto.EducationDetails.Select(edu => $@"
            <div style=""margin-bottom: 10px;"">
                <p style=""margin: 0;""><strong>{edu.Institution}</strong> | <em>{edu.Degree}</em></p>
                <p style=""margin: 0; font-size: 14px;"">{FormatDate(edu.StartDate)} - {FormatDate(edu.EndDate)}</p>
            </div>
        "))}
        </div>
             <div style=""margin-bottom: 20px;"">
        <h2 style=""border-bottom: 1px solid #000; font-size: 20px;"">Skills</h2>
        {string.Join("", resumeDto.skills.Select(ski => $@"
            <div style=""margin-bottom: 10px;"">
                <p style=""margin: 0;""><strong>{ski.Name}</strong> </p>
               
            </div>
        "))}
        </div>
    </div>
    </div>";

            
            return Ok(new { cvHtml = resumeContent });
        }

        [HttpPut("updateResume")]
        public async Task<IActionResult> UpdateResume([FromBody] UpdateResumeCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);

                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };
                var jsonResult = JsonSerializer.Serialize(result, options);

                return Ok(jsonResult);
            }
            catch (Exception ex) {
                throw;
                    }
        }

        [HttpGet("getResumes")]
        public async Task<IActionResult> GetResumes(CancellationToken cancellationToken)
        {
            //try
            //{
            var result = await Mediator.Send(new GetAllResumesQuery(), cancellationToken);
            return Ok(result);
            //}
            //catch (UnauthorizedAccessException)
            //{
            //    return Unauthorized(new { Message = "User is not logged in." });
            //}

        }
        [HttpGet("download-pdf/{id}")]
        public async Task<ActionResult> DownloadResumeAsPdf(int id)
        {
            var resume = await _context.Resumes
                .Include(r => r.WorkExperiences)
                .Include(r => r.EducationDetails)
                .Include(r => r.Skills)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (resume == null)
            {
                return NotFound();
            }

            var resumeDto = new ResumeDto
            {
                FullName = resume.FullName,
                Email = resume.Email,
                Phone = resume.Phone,
                Address = resume.Address,
                Summary = resume.Summary,
                WorkExperiences = resume.WorkExperiences.Select(we => new WorkExperienceDto
                {
                    Position = we.Position,
                    Company = we.Company,
                    Responsibilities = we.Responsibilities,
                    StartDate = we.StartDate,
                    EndDate = we.EndDate
                }).ToList(),
                EducationDetails = resume.EducationDetails.Select(ed => new EducationDto
                {
                    Degree = ed.Degree,
                    Institution = ed.Institution,
                    StartDate = ed.StartDate,
                    EndDate = ed.EndDate
                }).ToList(),
                skills = resume.Skills.Select(sk => new SkillsDto
                {
                    Name = sk.Name,
                }).ToList()
            };

            string FormatDate(string date)
            {
                if (DateTime.TryParse(date, out var parsedDate))
                {
                    return parsedDate.ToString("yyyy-MM-dd");
                }
                return "Unknown";
            }

            string resumeContent = $@"
    <div style=""font-family: 'Times New Roman', serif; padding: 20px; border: 1px solid #000; max-width: 800px; margin: auto;"">
        <div style=""text-align: center; margin-bottom: 20px;"">
            <h1 style=""font-size: 32px; margin: 0;"">{resumeDto.FullName.ToUpper()}</h1>
            <p style=""margin: 5px 0; font-size: 18px; font-weight: bold;"">{resumeDto.Summary ?? "Professional Title"}</p>
            <p style=""margin: 0; font-size: 16px;"">{resumeDto.Email} | {resumeDto.Phone} | {resumeDto.Address}</p>
        </div>

        <div style=""margin-bottom: 20px;"">
            <h2 style=""border-bottom: 1px solid #000; font-size: 20px;"">Objective</h2>
            <p>{resumeDto.Summary}</p>
        </div>

        <div style=""margin-bottom: 20px;"">
            <h2 style=""border-bottom: 1px solid #000; font-size: 20px;"">Experience</h2>
             {string.Join("", resumeDto.WorkExperiences.Select(exp => $@"
            <div style=""margin-bottom: 10px;"">
                <p style=""margin: 0;""><strong>{exp.Company}</strong> | <em>{exp.Position}</em></p>
                <p style=""margin: 0; font-size: 14px;"">{FormatDate(exp.StartDate)} - {FormatDate(exp.EndDate)}</p>
                <ul style=""margin: 5px 0; padding-left: 20px;"">
                    <li>{exp.Responsibilities}</li>
                </ul>
            </div>
        "))}

        </div>

        <div style=""margin-bottom: 20px;"">
            <h2 style=""border-bottom: 1px solid #000; font-size: 20px;"">Education</h2>
            {string.Join("", resumeDto.EducationDetails.Select(edu => $@"
            <div style=""margin-bottom: 10px;"">
                <p style=""margin: 0;""><strong>{edu.Institution}</strong> | <em>{edu.Degree}</em></p>
                <p style=""margin: 0; font-size: 14px;"">{FormatDate(edu.StartDate)} - {FormatDate(edu.EndDate)}</p>
            </div>
        "))}

        </div>
        <div style=""margin-bottom: 20px;"">
        <h2 style=""border-bottom: 1px solid #000; font-size: 20px;"">Skills</h2>
        {string.Join("", resumeDto.skills.Select(ski => $@"
            <div style=""margin-bottom: 10px;"">
                <p style=""margin: 0;""><strong>{ski.Name}</strong> </p>
            </div>
        "))}

        </div>
    </div>";


            var converter = new BasicConverter(new PdfTools());

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
            Out = null
        },
                Objects = {
            new ObjectSettings() {
                HtmlContent = resumeContent,
                WebSettings = { DefaultEncoding = "utf-8" },
                LoadSettings = { BlockLocalFileAccess = false }
            }
        }
            };

            byte[] pdfBytes = converter.Convert(doc);

            Response.Headers.Add("Content-Disposition", "attachment; filename=Resume_" + id + ".pdf");
            return File(pdfBytes, "application/pdf");
        }
        [HttpDelete("DeleteResume")]
        public async Task<IActionResult> Delete([FromBody] DeleteResumeCommand command)
        {
            if (command.Id <= 0)
            {
                return BadRequest(new { Message = "Invalid resume ID" });
            }

            var result = await Mediator.Send(command);

            if (!result)
            {
                return NotFound(new { Message = "Resume not found" });
            }

            return NoContent();
        }
    }
}

