using Dominio.Comandos.Contratos;

namespace Dominio.Manipuladores.Contratos
{
    public interface IManipulador<T> where T : ICommando
    {
        IResultadoComando Manupulador(T comando);
    }
}