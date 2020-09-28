using Dominio.Entities;
using System;
using System.Collections.Generic;

namespace Dominio
{
    public interface IHeroiRepositorio
    {
        Heroi PesquisarPorId(Guid id, string nome);
        void NovoHeroi(Heroi heroi);
        void AtualizarHeroi(Heroi heroi);
        void RemoverHeroi(Heroi heroi);
        IEnumerable<Heroi> MostrarUltimosHeroisRegistrados(DateTime date, bool done);
        IEnumerable<Heroi> PesquisaCodinome(string codinome);
    }
}
