using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ATS.Domain.Entities
{
    public class Users
    {
        public int Id { get; set; }
        [StringLength(25, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string? RefreshToken { get; set; }
        public string Role { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public ICollection<Resume> Resumes { get; set; }
    }
}
