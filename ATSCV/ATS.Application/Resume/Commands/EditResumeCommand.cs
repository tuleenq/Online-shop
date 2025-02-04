using ATS.Domain.Entities;
using ATS.Infrastructure.Data;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ATS.Application.Resumes;

public class UpdateResumeCommand : IRequest<ATS.Domain.Entities.Resume>
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public string Summary { get; set; }

    public List<WorkExperiencesDto> WorkExperiences { get; set; }
    public List<EducationsDto> EducationDetails { get; set; }
    public List<SkillDto> Skill { get; set; }
}
public class WorkExperiencesDto
{
    
    public string Company { get; set; }
    public string Position { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Responsibilities { get; set; }
}
public class EducationsDto
{
   
    public string Institution { get; set; }
    public string Degree { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}
public class SkillDto
{
    
    public string Name { get; set; }
}


public class UpdateResumeCommandHandler : IRequestHandler<UpdateResumeCommand, Resume>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateResumeCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Resume> Handle(UpdateResumeCommand request, CancellationToken cancellationToken)
    {
        var existingResume = await _context.Resumes
            .Include(r => r.WorkExperiences)
            .Include(r => r.EducationDetails)
            .Include(r => r.Skills)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (existingResume == null)
        {
            throw new InvalidOperationException($"Resume with ID {request.Id} was not found.");
        }

        
        existingResume.FullName = request.FullName;
        existingResume.Email = request.Email;
        existingResume.Phone = request.Phone;
        existingResume.Address = request.Address;
        existingResume.Summary = request.Summary;

       
        existingResume.WorkExperiences.Clear();
        if (request.WorkExperiences != null)
        {
            foreach (var workExperienceDto in request.WorkExperiences)
            {
                var workExperience = _mapper.Map<WorkExperience>(workExperienceDto);
                existingResume.WorkExperiences.Add(workExperience);
            }
        }

        
        existingResume.EducationDetails.Clear();
        if (request.EducationDetails != null)
        {
            foreach (var educationDto in request.EducationDetails)
            {
                var education = _mapper.Map<Education>(educationDto);
                existingResume.EducationDetails.Add(education);
            }
        }

       
        existingResume.Skills.Clear();
        if (request.Skill != null)
        {
            foreach (var skillDto in request.Skill)
            {
                var skill = _mapper.Map<Skills>(skillDto);
                existingResume.Skills.Add(skill);
            }
        }

        
        _context.Entry(existingResume).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);

        return existingResume;
    }
}

