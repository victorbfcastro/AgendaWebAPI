using System.Collections.Generic;
using System.Threading.Tasks;
using AgendaWebAPI.Data;
using AgendaWebAPI.Dtos;
using AgendaWebAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AgendaWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public readonly IRepository _repo;
        private readonly IMapper _mapper;

        public EventoController(IRepository repository, IMapper mapper)
        {
            _repo = repository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var eventos = await _repo.GetAllEventos(false);
            if(eventos == null) return BadRequest("Não há eventos cadastrados!");

            var eventosMapped = _mapper.Map<IEnumerable<EventoDto>>(eventos);
            return Ok(eventosMapped);
        }

        // api/evento/id       [Retorna um evento pelo ID]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var evento = await _repo.GetEventoByIdAsync(id, false);
            if(evento == null) return BadRequest("evento não encontrado!");

            var eventoMapped = _mapper.Map<EventoDto>(evento);

            return Ok(eventoMapped);         
        }

        // api/contato/byevento/id      [Retorna todos os eventos pelo ID de um Evento]
        [HttpGet("byContato/{id}")]
        public async Task<IActionResult> GetByEventoId(int id)
        {
            var eventos = await _repo.GetAllEventosByContatoId(id);
            if (eventos == null) return BadRequest("Não há eventos no evento escolhido");

            var eventosMapped = _mapper.Map<IEnumerable<EventoDto>>(eventos);

            return Ok(eventosMapped);
        }

        // api/Evento      [Adicionar um contato]
        [HttpPost]
        public IActionResult Post(EventoDto model)
        {
            var eventosMapped = _mapper.Map<Evento>(model);
            
            _repo.Add(eventosMapped);
            
            if(_repo.SaveChanges())
            {
                return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(eventosMapped));
            }

            return BadRequest("Não foi possível cadastrar novo evento!");
        }

        // api/evento/id       [Atualizar dados de um evento]
        [HttpPut("{id}")]
        public IActionResult Put(int id, EventoDto model)
        {
            var evento = _repo.GetEventoById(id, false);
            if (evento == null) return BadRequest("Evento não encontrado!");

            _mapper.Map(model, evento);

            _repo.Update(evento);

            if(_repo.SaveChanges())
            {
                return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDto>(evento));
            }

            return BadRequest("Evento não atualizado!");

        }

        // api/evento/id       [Deletar um evento]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var evento = _repo.GetEventoById(id, false);
            if (evento == null) return BadRequest("Evento não encontrado!");

            _repo.Delete(evento);

            if(_repo.SaveChanges())
            {
                return Ok("Evento removido!");
            }

            return BadRequest("Evento não removido!");

        }
    }
}