using ATS.Domain.Entities;
using ATS.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;
using System.Reflection.PortableExecutable;
using System.Security.AccessControl;

namespace ATS_CV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplateController : ApiControllercs
    {
        private readonly ApplicationDbContext _context;

        public TemplateController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("AddTemplate")]
        public async Task<IActionResult> CreateTemplate([FromBody] CreateTemplateCommand command)
        {
            if (command == null)
                return BadRequest("Invalid user data.");

            var createdTemplate = await Mediator.Send(command);

            return CreatedAtAction(nameof(CreateTemplate), new { id = createdTemplate.Id }, createdTemplate);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResumeDto>> GetResumeById(int id)
        {
            var resume = await _context.Resumes
                .Include(r => r.WorkExperiences)
                .Include(r => r.EducationDetails)
                .Include(r=> r.Skills)
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
                    StartDate= we.StartDate,
                    EndDate= we.EndDate

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
                return "Unknown"; // Fallback for invalid or empty dates
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

            
            return Ok(resumeDto);
        }

    }
}
