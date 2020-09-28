using System.ComponentModel;

namespace Dominio.ObjetosDeValor
{
    public enum Poder
    {
        [Description("Poderes mágicos")]
        PoderesMagicos = 1,

        [Description("Super Força")]
        SuperForca = 2,

        [Description("Cura")]
        Cura = 3,

        [Description("Invencibilidade")]
        Invencibilidade = 4,

        [Description("Voar")]
        Voar = 5
    }
}