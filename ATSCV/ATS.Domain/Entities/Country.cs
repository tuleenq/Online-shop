using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Domain.Entities
{
    public class Country
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Country name is required")]
        public string Name { get; set; }
        public string Code { get; set; }
        public ICollection<Resume> Resumes { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
