using Dominio.Comandos;
using Dominio.Comandos.Contratos;
using Dominio.Entities;
using Dominio.Manipuladores.Contratos;
using Flunt.Notifications;

namespace Dominio.Manipuladores
{
    public class HeroisManupulador : Notifiable, IManipulador<CriarNovoHeroiComando>, IManipulador<AtualizarHeroiComando>, IManipulador<RemoverHeroiComando>
    {
        private readonly IHeroiRepositorio _repositorio;

        public HeroisManupulador(IHeroiRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public IResultadoComando Manupulador(CriarNovoHeroiComando comando)
        {
            comando.Validate();
            if (comando.Invalid)
                return new ResultadoComandoGenerico(false, "Opa, sua tentativa foi inválida", comando.Notifications);

            var heroi = new Heroi(comando.NomeCompleto, comando.Codinome, comando.Nascimento, comando.Poder, comando.DataCadastro);

            heroi.EstaRegistrado();

            _repositorio.NovoHeroi(heroi);

            return new ResultadoComandoGenerico(true, "Heroi salvo", heroi);

        }

        public IResultadoComando Manupulador(AtualizarHeroiComando comando)
        {
            comando.Validate();
            if (comando.Invalid)
                return new ResultadoComandoGenerico(false, "Opa, sua tentativa foi inválida", comando.Notifications);

            var heroi = _repositorio.PesquisarPorId(comando.Id, comando.NomeCompleto);

            _repositorio.AtualizarHeroi(heroi);

            return new ResultadoComandoGenerico(true, "Heroi atualizdo", heroi);
        }

        public IResultadoComando Manupulador(RemoverHeroiComando comando)
        {
            comando.Validate();
            if (comando.Invalid)
                return new ResultadoComandoGenerico(false, "Opa, sua tentativa foi inválida", comando.Notifications);

            var heroi = _repositorio.PesquisarPorId(comando.Id, comando.NomeCompleto);

            heroi.NaoRegistrado();

            _repositorio.RemoverHeroi(heroi);

            return new ResultadoComandoGenerico(true, "Heroi removido", comando.NomeCompleto);
        }
    }
}