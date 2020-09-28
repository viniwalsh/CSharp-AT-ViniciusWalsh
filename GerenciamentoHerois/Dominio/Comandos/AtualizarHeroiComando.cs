using Dominio.Comandos.Contratos;
using Dominio.ObjetosDeValor;
using Flunt.Notifications;
using Flunt.Validations;
using System;

namespace Dominio.Comandos
{
    public class AtualizarHeroiComando : Notifiable, ICommando
    {
        public AtualizarHeroiComando() { }

        public AtualizarHeroiComando(string nomeCompleto, string codinome, DateTime nascimento, Poder poder)
        {
            NomeCompleto = nomeCompleto;
            Codinome = codinome;
            Nascimento = nascimento;
            Poder = poder;
        }

        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Codinome { get; set; }
        public DateTime Nascimento { get; set; }
        public Poder Poder { get; set; }

        public void Validate()
        {
            AddNotifications(
                new Contract()
                    .Requires()
                    .HasMinLen(NomeCompleto, 6, "NomeCompleto", "O nome deve ter no mimino 6 caracteres")
                    .HasMinLen(Codinome, 2, "Codinome", "O codinome deve ter no minimo 2 caracteres")
            );
        }
    }
}