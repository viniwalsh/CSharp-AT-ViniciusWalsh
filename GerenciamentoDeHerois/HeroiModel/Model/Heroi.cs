using HeroiModel.DAO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroiModel.Model
{
    public class Heroi
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Codinome { get; set; }
        public DateTime Nascimento { get; set; }
        public Poder Poder { get; set; }
        public DateTime DataCadastro { get; set; }

    }
}
