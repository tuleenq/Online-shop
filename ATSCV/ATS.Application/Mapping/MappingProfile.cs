using ATS.Application.Cityy.Commands;
using ATS.Application.Countrys.Commands;
using ATS.Application.Resumes;
using ATS.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateResumeCommand, Resume>()
            .ForMember(dest => dest.WorkExperiences, opt => opt.MapFrom(src => src.WorkExperiences))
            .ForMember(dest => dest.EducationDetails, opt => opt.MapFrom(src => src.EducationDetails));

            CreateMap<WorkExperienceDTO, WorkExperience>();
            CreateMap<SkillsDTO, Skills>();

            
            CreateMap<EducationDTO, Education>();
            CreateMap<CreateTemplateCommand, Template>();
            CreateMap<CreateCountryCommand, Country>();
            CreateMap<CreateCityCommand, City>();
            CreateMap<UpdateResumeCommandHandler, Resume>();

            CreateMap<WorkExperiencesDto, WorkExperience>();
            CreateMap<EducationsDto, Education>();
            CreateMap<SkillDto, Skills>();
            CreateMap<SkillDto, Skill>();
        }
    }
}
