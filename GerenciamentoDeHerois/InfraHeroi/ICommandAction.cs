using System;
using System.Collections.Generic;
using System.Text;

namespace InfraHeroi
{
    internal interface ICommandAction
    {
        bool Init();
        bool NewHero();
        bool FindHero();
        bool ListHeros();
        bool EditHero();
        bool RemoveHero();
        bool Exit();
    }
}
