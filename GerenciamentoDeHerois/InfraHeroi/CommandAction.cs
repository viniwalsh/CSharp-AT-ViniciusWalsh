using HeroiModel.DAO;
using HeroiModel.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfraHeroi
{
    internal class CommandAction : ICommandAction
    {
        public bool Init()
        {
            Console.WriteLine(string.Format("Herois que fazem aniversário hoje ({0}):", DateTime.Now.ToString("dd/MM")));
            List<Heroi> heroi = GetTodayBirthdays();
            if (heroi.Count > 0)
            {
                foreach (var hero in heroi)
                {
                    int age = GetAge(hero);
                    Console.WriteLine(string.Format("> {0} {1} ({2} {3})",
                        hero.NomeCompleto, hero.Codinome, age, age > 1 ? "anos" : "ano"));
                }
            }
            else
            {
                Console.WriteLine("Ninguém faz aniversário hoje.");
            }
            showLastHeros();
            ShowCommands();
            return true;
        }

        private void showLastHeros()
        {
            Console.WriteLine(string.Format("Herois cadastrados"));
            foreach (var heroi in HeroiDAO.Instance.GetAll())
            {
                Console.WriteLine(string.Format("{0} - {1} {2} {3} ({4})",
                  heroi.Id, heroi.NomeCompleto, heroi.Codinome, heroi.Poder,
                  heroi.Nascimento.ToString("dd/MM/yyyy")));
            }
            Console.WriteLine();
        }

        public bool NewHero()
        {
            Console.WriteLine("Novo cadastro do Herói:");
            Console.WriteLine();
            Heroi heroi = new Heroi();
            heroi.NomeCompleto = ReadString("Nome do Héroi: ");
            heroi.Codinome = ReadString("Codinome do Héroi: ");
            heroi.Nascimento = ReadDateTime("Data de Lançamento (dd/MM/yyyy): ");
            Console.WriteLine("Informe se o Héroi tem algum SuperPoder:");
            Console.WriteLine("Poderes mágicos - 1 | Super Força - 2 | Cura - 3 | Invencibilidade - 4 | Voar - 5");
            if (!byte.TryParse(Console.ReadLine(), out var poderByte) || poderByte > 6)
            {
                Console.Write($"Dado inválido! Dados descartados! ");
            }
            var poderEnum = (Poder)poderByte;
            heroi.Poder = poderEnum;
            HeroiDAO.Instance.Add(heroi);
            return true;
        }

        public bool FindHero()
        {
            string search = ReadName();
            Console.WriteLine("Sua busca encontrou os seguintes resultados:");
            ListHerois(search);
            ShowMessageAndWaitKeyPress("Aperte qualquer tecla para voltar ao menu principal...");

            return true;
        }

        public bool ListHeros()
        {
            Console.WriteLine("ID - Nome Sobrenome (Nascimento)");
            ListHerois("");
            ShowMessageAndWaitKeyPress("Aperte qualquer tecla para sair da listagem...");
            return true;
        }

        // Método que lista os Héroi filtrando por uma palavra-chave.
        // Caso a string seja vazia, lista todas as pessoas.
        public bool ListHerois(string search)
        {
            Console.WriteLine();
            foreach (var heroi in HeroiDAO.Instance.GetAll())
            {
                if (HeroMatch(heroi, search))
                {
                    int days = GetDaysToNextBirthday(heroi);
                    string message = "Faz aniversário hoje!";
                    if (days > 1)
                    {
                        message = string.Format("Faltam {0} dias para seu aniversário.", days);
                    }
                    else if (days == 1)
                    {
                        message = "Aniversário do heroi é amanhã!";
                    }
                    Console.WriteLine(string.Format("{0} - {1} {2} {3} ({4}) - {5}",
                        heroi.Id, heroi.NomeCompleto, heroi.Codinome, heroi.Poder,
                        heroi.Nascimento.ToString("dd/MM/yyyy"), message));
                }
            }
            Console.WriteLine();
            return true;
        }

        public bool EditHero()
        {
            Console.WriteLine("Selecione uma Héroi para editar:");
            ListHerois("");
            int id = ReadId();
            if (id != 0)
            {
                Console.WriteLine(string.Format("Você está editando um Héroi de ID {0}!", id));
                Console.WriteLine("(Para manter o campo com o valor atual, deixe-o em branco)");
                Console.WriteLine();
                Heroi heroi = HeroiDAO.Instance.FindById(id);
                heroi.NomeCompleto = ReadString(string.Format("Nome completo (atual = {0}): ", heroi.NomeCompleto), heroi.NomeCompleto);
                heroi.Codinome = ReadString(string.Format("Codinome do Heroi (atual = {0}): ", heroi.Codinome), heroi.Codinome);
                heroi.Nascimento = ReadDateTime(string.Format("Lançamento do Heroi (atual = {0}): ", heroi.Nascimento.ToString("dd/MM/yyyy")), heroi.Nascimento);
                Console.WriteLine("Informe se o Héroi tem algum SuperPoder:");
                Console.WriteLine("Poderes mágicos - 1 | Super Força - 2 | Cura - 3 | Invencibilidade - 4 | Voar - 5");
                if (!byte.TryParse(Console.ReadLine(), out var poderByte) || poderByte > 6)
                {
                    Console.Write($"Dado inválido! Dados descartados! ");
                }
                var poderEnum = (Poder)poderByte;
                heroi.Poder = poderEnum;
                HeroiDAO.Instance.Update(heroi);
                ShowMessageAndWaitKeyPress("Héroi editado com sucesso! Aperte qualquer tecla para continuar...");
            }
            return true;
        }

        public bool RemoveHero()
        {
            Console.WriteLine("Selecione uma Héroi para remover:");
            ListHerois("");
            int id = ReadId();
            if (id != 0)
            {
                HeroiDAO.Instance.Remove(id);
                Console.WriteLine();
                ShowMessageAndWaitKeyPress("Héroi removido com sucesso! Aperte qualquer tecla para continuar...");
            }
            return true;
        }

        public bool Exit()
        {
            return false;
        }

        private void ShowCommands()
        {
            Console.WriteLine();
            foreach (var letter in Enum.GetValues(typeof(Command.Letter)))
            {
                if (!letter.Equals(Command.Letter.I))
                {
                    Console.WriteLine(string.Format("{0} - {1}", letter, Command.GetDescription((Command.Letter)letter)));
                }
            }
        }

        private int ReadId()
        {
            while (true)
            {
                try
                {
                    Console.Write("Digite um ID para buscar seu Héroi(0 para sair): ");
                    int id = Int32.Parse(Console.ReadLine());
                    if (id == 0 || HeroiDAO.Instance.FindById(id) != null)
                    {
                        return id;
                    }
                    else
                    {
                        Console.WriteLine("Esse ID não existe!");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Digite apenas números!");
                }
            }
        }

        private string ReadName()
        {
            Console.Write("Digite Nome e/ou Codinome do Héroi: ");
            return Console.ReadLine();
        }

        private string ReadString(string message)
        {
            return ReadString(message, null);
        }

        private string ReadString(string message, string defaultValue)
        {
            Console.Write(message);
            string value = Console.ReadLine().Trim();
            if (defaultValue != null && value == "")
            {
                return defaultValue;
            }
            return value;
        }

        private void ShowMessageAndWaitKeyPress(string message)
        {
            Console.Write(message);
            Console.ReadKey();
        }

        private DateTime ReadDateTime(string message)
        {
            return ReadDateTime(message, null);
        }

        private DateTime ReadDateTime(string message, DateTime? defaultValue)
        {
            while (true)
            {
                try
                {
                    Console.Write(message);
                    string value = Console.ReadLine().Trim();
                    if (defaultValue != null && value == "")
                    {
                        return defaultValue.Value;
                    }
                    return DateTime.ParseExact(value.Replace("/", ""), "ddMMyyyy",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    Console.WriteLine("Algo deu errado! Verifique se a data é válida ou o formato digitado está correto!");
                }
            }
        }

        // Método que busca os herois que fazem aniversário no dia atual
        private List<Heroi> GetTodayBirthdays()
        {
            DateTime now = DateTime.Now;
            List<Heroi> birthdayNow = new List<Heroi>();
            foreach (var heroi in HeroiDAO.Instance.GetAll())
            {
                if (now.Day == heroi.Nascimento.Day && now.Month == heroi.Nascimento.Month)
                {
                    birthdayNow.Add(heroi);
                }
            }
            return birthdayNow;
        }

        private int GetAge(Heroi heroi)
        {
            return DateTime.Now.Year - heroi.Nascimento.Year;
        }

        // Verifica se nome e codinome de um certo heroi bate com a string de busca.
        private bool HeroMatch(Heroi heroi, string search)
        {
            return (heroi.NomeCompleto.ToLower() + " " + heroi.Codinome.ToLower()).Contains(search);
        }

        private int GetDaysToNextBirthday(Heroi heroi)
        {
            DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime nextBirthday = new DateTime(now.Year, heroi.Nascimento.Month, heroi.Nascimento.Day);
            if (now.CompareTo(nextBirthday) > 0)
            {
                nextBirthday = new DateTime(now.Year + 1, heroi.Nascimento.Month, heroi.Nascimento.Day);
            }
            return Convert.ToInt32((nextBirthday - now).TotalDays);
        }
    }
}
