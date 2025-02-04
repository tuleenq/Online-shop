using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ATS.Infrastructure.Data;
using ATS.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using ATS.Application.Helpers;

namespace ATS.Application.Resumes;

public class CreateResumeCommand : IRequest<int>
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Summary { get; set; }
    public List<SkillsDTO> Skills { get; set; }
    public List<WorkExperienceDTO> WorkExperiences { get; set; }
    public List<EducationDTO> EducationDetails { get; set; }
    public int TemplateId { get; set; }
    [JsonIgnore]
    public int? UserId { get; set; }
    [JsonIgnore]
    public DateTime? CreationDate { get; set; }


}
public class WorkExperienceDTO
{
    
    public string Company { get; set; }
    public string Position { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Responsibilities { get; set; }
}

public class EducationDTO
{
   
    public string Institution { get; set; }
    public string Degree { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}
public class SkillsDTO
{
    
    public string Name { get; set; }
}

public class CreateResumeCommandHandler : IRequestHandler<CreateResumeCommand, int>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenHelper _tokenHelper;


    public CreateResumeCommandHandler(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, TokenHelper tokenHelper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _tokenHelper = tokenHelper;
    }

    public async Task<int> Handle(CreateResumeCommand request, CancellationToken cancellationToken)
    {
        var token = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");

        if (string.IsNullOrEmpty(token))
        {
            throw new UnauthorizedAccessException("Token is missing or user is not logged in.");
        }

        
        var userIdString = _tokenHelper.GetUserIdFromToken(token);
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid or missing user ID in token.");
        }

        request.UserId = userId;
        request.CreationDate = DateTime.Now;

        var resume = _mapper.Map<Resume>(request);
        resume.UserId = request.UserId.Value;
        resume.CreationDate = request.CreationDate.Value;
        await _context.Resumes.AddAsync(resume, cancellationToken);

        
        await _context.SaveChangesAsync(cancellationToken);

        return resume.Id; 
    }
}

