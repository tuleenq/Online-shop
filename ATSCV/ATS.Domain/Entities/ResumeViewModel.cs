using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Domain.Entities
{
    public class ResumeViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Summary { get; set; }
        public List<WorkExperiencea> WorkExperiences { get; set; } = new List<WorkExperiencea>();
        public List<Skill> Skills { get; set; } = new List<Skill>();
        public List<EducationDetail> EducationDetails { get; set; } = new List<EducationDetail>();
    }

    public class WorkExperiencea
    {
        public string Company { get; set; }
        public string Position { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Responsibilities { get; set; }
    }

    public class Skill
    {
        public string Name { get; set; }
    }

    public class EducationDetail
    {
        public string Institution { get; set; }
        public string Degree { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
