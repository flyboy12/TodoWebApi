using System.ComponentModel.DataAnnotations;

namespace WebApi_TodoApp.Models
{
    public class TodoResponse
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
    }
    public class TodoCreateModel
    {
        [Required]
        [StringLength(250)]
        public string Text { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
    }
    public class TodoUpdateModel
    {
        [Required]
        [StringLength(250)]
        public string Text { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }

    }

}
