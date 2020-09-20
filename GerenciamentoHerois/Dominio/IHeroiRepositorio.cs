using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public interface IHeroiRepositorio
    {
        IList<Heroi> Pesquisar(string termoDePesquisa);
        void Adicionar(Heroi entidade);
    }
}
