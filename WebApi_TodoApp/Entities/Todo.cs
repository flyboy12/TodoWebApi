using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi_TodoApp.Entities
{ [Table("todos")]
    public class Todo
    {
       
        [Key]
       public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Text { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }

    }
    public class DatabaseContext : DbContext
    {


        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<Todo> Todos { get; set; }

    }
}
