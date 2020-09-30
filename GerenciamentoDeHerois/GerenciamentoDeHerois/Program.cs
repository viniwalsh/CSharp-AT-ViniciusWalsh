using InfraHeroi;
using System;

namespace GerenciamentoDeHerois
{
    class Program
    {
        static void Main(string[] args)
        {
            char letter;
            Command command = new Command();
            do
            {
                command.Execute(Command.getValue('I'))();
                letter = Console.ReadKey().KeyChar;
            }
            while (command.Execute(Command.getValue(ToUpper(letter)))());
        }

        public static char ToUpper(char letter)
        {
            return letter.ToString().ToUpper().ToCharArray()[0];
        }
    }
}
