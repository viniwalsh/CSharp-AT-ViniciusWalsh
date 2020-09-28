using Dominio.Comandos.Contratos;

namespace Dominio.Comandos
{
    public class ResultadoComandoGenerico : IResultadoComando
    {
        public ResultadoComandoGenerico() { }

        public ResultadoComandoGenerico(bool sucesso, string mensagem, object dado)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
            Dado = dado;
        }

        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public object Dado { get; set; }
    }
}