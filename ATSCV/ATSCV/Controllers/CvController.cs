using ATS.Application.Helpers;
using ATS.Domain.Entities;
using ATS.Infrastructure.Data;
using Environment.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Application.Category.Queries;
using Newtonsoft.Json;

namespace ATS_CV.Controllers
{

    
    public class CvController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenHelper _tokenService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DEVService _devService;


        public CvController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, TokenHelper tokenService, IHttpClientFactory httpClientFactory,DEVService dEVService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _httpClientFactory = httpClientFactory;
            _devService = dEVService;
        }
       
        public IActionResult craete()
        {
            if (!_devService.IsUserLoggedIn())
            {
                return RedirectToAction("Login", "Authentication");
            }

            var resume = new Resume
            {
                WorkExperiences = new List<WorkExperience>(),
                EducationDetails = new List<Education>(),
                Skills = new List<Skills>()
            };

            return View(resume);
            // var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();

            //var token = HttpContext.Session.GetString("AuthToken");


            //if (string.IsNullOrEmpty(token) || !_tokenService.ValidateAccessToken(token))
            //{
            //    return RedirectToAction("Login", "Authentication");
            //}
            //var resume = new Resume
            //{
            //    WorkExperiences = new List<WorkExperience>(),
            //    EducationDetails = new List<Education>(),
            //    Skills = new List<Skills>()
            //};

            //return View(resume);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewCV(int id)
        {
            ViewData["ResumeId"] = id;  
            return View();
        }
        public IActionResult Edit()
        {
            var resume = new Resume
            {
                WorkExperiences = new List<WorkExperience>(),
                EducationDetails = new List<Education>(),
                Skills = new List<Skills>()
            };

            return View(resume);
        }
        
        
        public  IActionResult EditResume(int resumeId)
        {
            //var model= await _context.Resumes.Where(x=>x.Id == resumeId).FirstOrDefaultAsync();
            return View(new EditModel { Id = resumeId });


        }
        public IActionResult GetAll()
        {
            return View();
        }
    }
}
public class ResumeDto
{
   
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Summary { get; set; }
    public List<SkillsDto> skills { get; set; }
    public List<WorkExperienceDto> WorkExperiences { get; set; }
    public List<EducationDto> EducationDetails { get; set; }
}

public class WorkExperienceDto
{
    public string Company { get; set; }
    public string Position { get; set; }
   
    public string Responsibilities { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}

public class EducationDto
{
    public string Institution { get; set; }
    public string Degree { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}
public class SkillsDto
{
    public string Name { get; set; }
    
}