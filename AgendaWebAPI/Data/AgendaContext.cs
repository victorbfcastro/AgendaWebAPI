using System;
using System.Collections.Generic;
using AgendaWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendaWebAPI.Data
{
    public class AgendaContext : DbContext
    {
        public AgendaContext(DbContextOptions<AgendaContext> options) : base(options) { }

        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<PessoaEvento> PessoasEventos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){

            builder.Entity<PessoaEvento>().HasKey(PE => new { PE.ContatoId, PE.EventoId });

            builder.Entity<Contato>()
                .HasData(new List<Contato>(){
                    new Contato(1, "Peter", "Parker", "99999999", "peterpark@gmail.com"),
                    new Contato(2, "Michael", "Jackson", "888888888", "mjackson@hotmail.com"),
                    new Contato(3, "Jorge", "Wilson", "483948394", "jw92@gmail.com"),
                    new Contato(4, "Maria", "Paula", "123923834", "mariapaula@hotmail.com")
                });

            builder.Entity<Evento>()
                .HasData(new List<Evento>(){
                    new Evento(1, "Reunião Gerencial", DateTime.Parse("04/08/2021")),
                    new Evento(2, "Aniversario", DateTime.Parse("13/09/1991")),
                    new Evento(3, "Curso ASP.NET Core", DateTime.Parse("07/12/2021"))
                });

            builder.Entity<PessoaEvento>()
                .HasData(new List<PessoaEvento>(){
                    new PessoaEvento() {ContatoId = 1, EventoId = 2},
                    new PessoaEvento() {ContatoId = 1, EventoId = 3},
                    new PessoaEvento() {ContatoId = 2, EventoId = 1},
                    new PessoaEvento() {ContatoId = 3, EventoId = 1},
                    new PessoaEvento() {ContatoId = 3, EventoId = 3},
                });
        }
    }
}