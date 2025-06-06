using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Announcement_api.Model
{
    public class Announcement
    {
        [Key]
        public int id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
    }
}
