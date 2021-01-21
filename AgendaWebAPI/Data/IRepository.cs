using System.Threading.Tasks;
using AgendaWebAPI.Models;

namespace AgendaWebAPI.Data
{
    public interface IRepository
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        bool SaveChanges();

        // MÉTODOS CONTATOS
        Task<Contato[]> GetAllContatos(bool includeEvento = false);   //TODO includeEvento
        Task<Contato> GetContatoByIdAsync(int contatoId, bool includeEvento = false);
        Contato GetContatoById(int contatoId, bool includeEvento = false);  
        Task<Contato[]> GetAllContatosByEventoId(int eventoId);

        // MÉTODOS EVENTOS
        Task<Evento[]> GetAllEventos(bool includeContato = false);    //TODO includeContato
        Evento GetEventoById(int eventoId, bool includeContato = false);    //TODO includeEvento
        Task<Evento> GetEventoByIdAsync(int eventoId, bool includeContato = false);
        Task<Evento[]> GetAllEventosByContatoId(int contatoId);

    }

}