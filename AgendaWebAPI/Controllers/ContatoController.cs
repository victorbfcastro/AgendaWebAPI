using System.Threading.Tasks;
using AgendaWebAPI.Data;
using AgendaWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgendaWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContatoController : ControllerBase
    {
        public readonly IRepository _repo;

        public ContatoController(IRepository repository)
        {
            _repo = repository;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var contatos = await _repo.GetAllContatos(false);
            if(contatos == null) return BadRequest("Não há contatos cadastrados!");

            return Ok(contatos);
        }

        // api/contato/id       [Retorna um contato pelo ID]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contato = await _repo.GetContatoByIdAsync(id, false);
            if(contato == null) return BadRequest("Contato não encontrado!");

            return Ok(contato);         
        }

        // api/contato/byevento/id      [Retorna todos os contatos pelo ID de um Evento]
        [HttpGet("byEvento/{id}")]
        public async Task<IActionResult> GetByEventoId(int id)
        {
            var contatos = await _repo.GetAllContatosByEventoId(id);
            if (contatos == null) return BadRequest("Não há contatos no evento escolhido");

            return Ok(contatos);
        }

        // api/contato      [Adicionar um contato]
        [HttpPost]
        public IActionResult Post(Contato contato)
        {
            _repo.Add(contato);
            
            if(_repo.SaveChanges())
            {
                return Created($"/api/contato/{contato.Id}", contato);
            }

            return BadRequest("Não foi possível cadastrar novo contato!");
        }

        // api/contato/id       [Atualizar dados de um contato]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Contato contato)
        {
            var dominio = _repo.GetContatoById(id, false);
            if (dominio == null) return BadRequest("Contato não encontrado!");

            _repo.Update(contato);

            if(_repo.SaveChanges())
            {
                return Ok("Contato atualizado!");
            }

            return BadRequest("Contato não atualizado!");

        }

        // api/contato/id       [Deletar um contato]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var contato = _repo.GetContatoById(id, false);
            if (contato == null) return BadRequest("Contato não encontrado!");

            _repo.Delete(contato);

            if(_repo.SaveChanges())
            {
                return Ok("Contato removido!");
            }

            return BadRequest("Contato não removido!");

        }
    }
}