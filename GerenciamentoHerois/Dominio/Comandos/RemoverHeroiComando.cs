using Dominio.Comandos.Contratos;
using Flunt.Notifications;
using Flunt.Validations;
using System;

namespace Dominio.Comandos
{
    public class RemoverHeroiComando : Notifiable, ICommando
    {
        public RemoverHeroiComando() { }

        public RemoverHeroiComando(Guid id, string nomeCompleto)
        {
            Id = id;
            NomeCompleto = nomeCompleto;
        }

        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }

        public void Validate()
        {
            AddNotifications(
                new Contract()
                    .Requires()
                    .HasMinLen(NomeCompleto, 6, "NomeCompleto", "O nome deve ter no mimino 6 caracteres")
            );
        }
    }
}