using System;
using System.Collections.Generic;
using System.Text;

namespace InfraHeroi
{
    public class Command
    {

        private ICommandAction action;
        public enum Letter { N, B, L, E, R, S, I }

        public Command()
        {
            action = new CommandAction();
        }

        public static Letter getValue(char option)
        {
            switch (option)
            {
                case 'N':
                    return Letter.N;
                case 'B':
                    return Letter.B;
                case 'L':
                    return Letter.L;
                case 'E':
                    return Letter.E;
                case 'R':
                    return Letter.R;
                case 'S':
                    return Letter.S;
                default:
                    return Letter.I;
            }
        }

        public static string GetDescription(Letter option2)
        {
            switch (option2)
            {
                case Letter.N:
                    return "Novo Heroi";
                case Letter.B:
                    return "Buscar Herois";
                case Letter.L:
                    return "Listar Herois";
                case Letter.E:
                    return "Editar Heroi";
                case Letter.R:
                    return "Remover Heroi";
                case Letter.S:
                    return "Sair";
                default:
                    return null;
            }
        }

        public Func<bool> Execute(Letter option)
        {
            Console.Clear();
            switch (option)
            {
                case Letter.N:
                    return action.NewHero;
                case Letter.B:
                    return action.FindHero;
                case Letter.L:
                    return action.ListHeros;
                case Letter.E:
                    return action.EditHero;
                case Letter.R:
                    return action.RemoveHero;
                case Letter.S:
                    return action.Exit;
                default:
                    return action.Init;
            }
        }
    }
}
