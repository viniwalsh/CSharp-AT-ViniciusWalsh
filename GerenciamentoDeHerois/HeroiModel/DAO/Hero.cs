using HeroiModel.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroiModel.DAO
{
    internal class Hero
    {
        public List<Heroi> List { get; set; }
        public int Index { get; set; }

        public Hero()
        {
            List = new List<Heroi>();
            Index = 1;
        }
    }

}
