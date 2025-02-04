using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ATS.Domain.Entities
{
    public class Resume
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Summary { get; set; }
        public ICollection<WorkExperience> WorkExperiences { get; set; }
        public ICollection<Education> EducationDetails { get; set; }
        public ICollection<Skills> Skills { get; set; }
        [JsonIgnore]
        public int TemplateId { get; set; }
        [JsonIgnore]
        public Template Template { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        [JsonIgnore]
        public Users? User { get; set; }
        [JsonIgnore]

        public int? CountryId { get; set; }
        [JsonIgnore]
        public Country? country { get; set; }
        [JsonIgnore]
        public DateTime? CreationDate { get; set; }

    }
    public class WorkExperience
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "Company name is required")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Position is required")]
        public string Position { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Responsibilities { get; set; }
        [JsonIgnore]
        public int ResumeId { get; set; }
        [JsonIgnore]
        public Resume Resume { get; set; }
    }

    public class Education
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "Institution is required")]
        public string Institution { get; set; }
        public string Degree { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        [JsonIgnore]
        public int ResumeId { get; set; }
        [JsonIgnore]
        public Resume Resume { get; set; }
    }
    public class Skills
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "Skill name is required")]
        public string Name { get; set; }
        [JsonIgnore]
        public int ResumeId { get; set; }
        [JsonIgnore]
        public Resume Resume { get; set; }

    }


}
