using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Application.Helpers;
using ATS.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;



public class GetAllResumesQuery : IRequest<List<ResumeDto>>
{
}

public class ResumeDto
{
    public int Id { get; set; }
    public DateTime? CreationDate { get; set; }
    public string Summary { get; set; }
}

public class GetResumeQueryHandler : IRequestHandler<GetAllResumesQuery, List<ResumeDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenHelper _tokenHelper;

    public GetResumeQueryHandler(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, TokenHelper tokenHelper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _tokenHelper = tokenHelper;
    }

    public async Task<List<ResumeDto>> Handle(GetAllResumesQuery request, CancellationToken cancellationToken)
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

        
        var resumes = await _context.Resumes
            .Where(r => r.UserId == userId)
            .Select(r => new ResumeDto
            {
                Id = r.Id,
                CreationDate = r.CreationDate,
                Summary = r.Summary
            })
            .ToListAsync(cancellationToken);

        return resumes;
    }
}







