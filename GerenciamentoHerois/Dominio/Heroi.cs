using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Heroi
    {
        public Heroi(string nomeCompleto, string codinome, DateTime nascimento, Poder poder, DateTime DataCadastro)
        {
            HeroiID = Guid.NewGuid();
            NomeCompleto = nomeCompleto ?? throw new ArgumentException("Primeiro nome não foi preenchido");
            Codinome = codinome ?? throw new ArgumentException("Codinome não foi preenchido");
            Nascimento = nascimento;
            Poder = poder;
            DataCadastro = DateTime.Now.Date;
        }

        public Guid HeroiID { get; private set; }
        public string NomeCompleto { get; private set; }
        public string Codinome { get; private set; }
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
    }
}

