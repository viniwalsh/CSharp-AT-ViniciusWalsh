using Dominio.Entities;
using System;
using System.Linq.Expressions;

namespace Dominio.Queries
{
    public static class HeroisQueries
    {
        public static Expression<Func<Heroi, bool>> MostrarUltimosHeroisRegistrados(DateTime data, bool done)
        {
            return x => x.Registrado == done && x.DataCadastro == data;
        }

        public static Expression<Func<Heroi, bool>> PesquisaCodinome(string codinome)
        {
            return x => x.Codinome == codinome;
        }
    }
}