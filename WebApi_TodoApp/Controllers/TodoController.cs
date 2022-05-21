using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_TodoApp.Entities;
using WebApi_TodoApp.Models;

namespace WebApi_TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private DatabaseContext _db;
        //Dependency injection
        public TodoController(DatabaseContext db)
        {
            _db = db;
        }
        [HttpGet("generate-fakedata")]
        public IActionResult GenerateFakeData()
        {
            if (_db.Todos.Any())
                return Ok("Veri tabanındaki Todos tablasunda örnek veri var");

            for (int i =0; i<50 ; i++)
            {
                _db.Todos.Add(new Todo() { Text= MFramework.Services.FakeData.TextData.GetSentence(),IsCompleted = MFramework.Services.FakeData.BooleanData.GetBoolean(),Description= MFramework.Services.FakeData.TextData.GetSentences(2)});
            }
            _db.SaveChanges();
            return Ok("OK");

        }
        [HttpGet("List")]
        [ProducesResponseType(200,Type= typeof(List<TodoResponse>))]
        public IActionResult List()
        {
            //List<Todo> todos = _db.Todos.ToList();
            //List<TodoResponse> list = new List<TodoResponse>();
            //foreach (Todo todosItem in todos)
            //{
            //    list.Add(new TodoResponse
            //    {
            //        Id = todosItem.Id,
            //        Text = todosItem.Text,
            //        Description = todosItem.Description,
            //        IsCompleted = todosItem.IsCompleted,
            //    });
            //}
            List<TodoResponse> list = _db.Todos.Select(t=> 
              new TodoResponse
              {
                 Id = t.Id,
                 Text = t.Text,
                 IsCompleted = t.IsCompleted,
                 Description = t.Description,
              }).ToList();   
            return Ok(list);
        }
        [HttpPost("create")]
        [ProducesResponseType(201, Type = typeof(TodoResponse))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(201, Type = typeof(TodoResponse))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult Create([FromBody] TodoCreateModel model)
        {
            if (ModelState.IsValid)
            {

                Todo todo = new Todo
                {
                    Text = model.Text,
                    Description = model.Description,
                };
                _db.Todos.Add(todo);
                int affected = _db.SaveChanges();
                if (affected > 0)
                {
                    TodoResponse result = new TodoResponse
                    {
                        Id = todo.Id,
                        Text = todo.Text,
                        Description = todo.Description,
                        IsCompleted = todo.IsCompleted,
                    };
                    return Created(String.Empty, result);
                }
                else
                {
                    return BadRequest("Kayıt Yapılamadı.");
                }
            }
            return BadRequest(ModelState);

        }
        [HttpPut("edit/{id}")]
        [ProducesResponseType(200, Type = typeof(TodoResponse))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult Update([FromRoute]int id ,[FromBody] TodoUpdateModel model)
        {
            Todo? todo = _db.Todos.Find(id);
            if (todo==null)
                return NotFound("Kayıt Bulunamadı");
            todo.Text = model.Text;
            todo.Description = model.Description;   
            todo.IsCompleted = model.IsCompleted;   
            int affected = _db.SaveChanges();
            if (affected > 0)
                return Ok(new TodoResponse { Id= todo.Id,Text=todo.Text,Description=todo.Description,IsCompleted=todo.IsCompleted});
            else
                return BadRequest("Güncelleme Yapılamadı");
        }
        [HttpPatch("changestate/{id}/{isComleted}")]
        [ProducesResponseType(200, Type = typeof(TodoResponse))]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult ChangeState([FromRoute] int id, [FromRoute] bool isCompleted)
        {
            Todo? todo = _db.Todos.Find(id);
            if (todo == null)
                return NotFound("Kayıt Bulunamadı");
            todo.IsCompleted = isCompleted;
            int affected = _db.SaveChanges();
            if (affected > 0)
                return Ok(new TodoResponse { Id = todo.Id, Text = todo.Text, Description = todo.Description, IsCompleted = todo.IsCompleted });
            else
                return BadRequest("Durum Değiştirilemedi");
        }
        [HttpDelete("remove/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult Delete([FromRoute] int id)
        {
            Todo? todo = _db.Todos.Find(id);
            if (todo == null)
                return NotFound("Kayıt Bulunamadı");
            _db.Todos.Remove(todo);
            int affected = _db.SaveChanges();
            if (affected > 0)
                return Ok();
            else
                return BadRequest("Silinemedi");
        }
        [HttpDelete("remove-all")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(string))]
        public IActionResult DeleteAll()
        {
            List<Todo> todos = _db.Todos.ToList();
            foreach (Todo item in todos)
            {
                _db.Todos.Remove(item);
            }
            int affected = _db.SaveChanges();
            if (affected > 0)
                return Ok();
            else
                return BadRequest("Kayıt Yok veya İşlem Gerçekleşmedi");
        }
    }
}
