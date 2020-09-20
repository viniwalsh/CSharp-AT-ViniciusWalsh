using Dominio;
using System.Collections.Generic;
using System.Linq;

namespace Infraestrutura
{
    public sealed class HeroiRepositorioList : IHeroiRepositorio
    {
        private static List<Heroi> heroiList = new List<Heroi>();

        public IList<Heroi> Pesquisar(string termoDePesquisa)
        {
            var heroisEncontrados = heroiList.Where(x => x.NomeCodinome().ToLower().Contains(termoDePesquisa.ToLower()))
                                                 .ToList();
            return heroisEncontrados;
        }

        public void Adicionar(Heroi heroi)
        {
            heroiList.Add(heroi);
        }
    }
}
