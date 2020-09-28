using Dominio.ObjetosDeValor;
using System;

namespace Dominio.Entities
{
    public class Heroi : Entity
    {
        public Heroi(string nomeCompleto, string codinome, DateTime nascimento, Poder poder, DateTime dataCadastro)
        {
            NomeCompleto = nomeCompleto ?? throw new ArgumentException("Primeiro nome não foi preenchido");
            Codinome = codinome ?? throw new ArgumentException("Codinome não foi preenchido");
            Registrado = false;
            Nascimento = nascimento;
            Poder = poder;
            DataCadastro = dataCadastro;
        }

        public string NomeCompleto { get; private set; }
        public string Codinome { get; private set; }
        public bool Registrado { get; private set; }
        public DateTime Nascimento { get; private set; }
        public Poder Poder { get; private set; }
        public DateTime DataCadastro { get; private set; }

        public string NomeCodinome() => string.Format("{0} {1}", NomeCompleto, Codinome);

        public int ObterQtdeDeDiasParaOProximoAniversario()
        {
            var dataAniversarioAnoAtual = new DateTime(DateTime.Now.Year, Nascimento.Month, Nascimento.Day);
            var qtdeDiasDiff = dataAniversarioAnoAtual - DateTime.Now;
            return qtdeDiasDiff.Days;
        }

        public void EstaRegistrado()
        {
            Registrado = true;
        }

        public void NaoRegistrado()
        {
            Registrado = false;
        }
    }
}