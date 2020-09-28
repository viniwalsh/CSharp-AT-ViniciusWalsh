using Dominio;
using Dominio.Entities;
using Dominio.Queries;
using Infraestrutura.Contextos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infraestrutura.Repositorios
{
    public class HeroiRepositorio : IHeroiRepositorio
    {
        private readonly HeroiContexto _contexto;

        public HeroiRepositorio(HeroiContexto contexto)
        {
            _contexto = contexto;
        }

        public IEnumerable<Heroi> MostrarUltimosHeroisRegistrados(DateTime date, bool done)
        {
            return _contexto.Herois.AsNoTracking().Where(HeroisQueries.MostrarUltimosHeroisRegistrados(date, done)).OrderBy(x => x.DataCadastro);
        }

        public IEnumerable<Heroi> PesquisaCodinome(string codinome)
        {
            return _contexto.Herois.AsNoTracking().Where(HeroisQueries.PesquisaCodinome(codinome)).OrderBy(x => x.DataCadastro);
        }
        public Heroi PesquisarPorId(Guid id, string nome)
        {
            return _contexto.Herois.FirstOrDefault(x => x.Id == id && x.NomeCompleto == nome);
        }

        public void NovoHeroi(Heroi heroi)
        {
            _contexto.Herois.Add(heroi);
            _contexto.SaveChanges();
        }

        public void AtualizarHeroi(Heroi heroi)
        {
            _contexto.Entry(heroi).State = EntityState.Modified;
            _contexto.SaveChanges();
        }

        public void RemoverHeroi(Heroi heroi)
        {
            _contexto.Herois.Remove(heroi);
            _contexto.SaveChanges();
        }
    }
}