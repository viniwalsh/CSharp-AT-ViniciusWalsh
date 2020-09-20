using Dominio;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IHeroiRepositorio _repositorio;
        private const string pressioneQualquerTecla = "Pressione qualquer tecla para exibir o menu principal ...";

        public Worker(IHeroiRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("pt-BR");

            string opcao;
            do
            {
                ShowMenu();

                opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        PesquisarHerois();
                        break;
                    case "2":
                        AdicionarHeroi();
                        break;
                    case "3":
                        AlterarHeroi();
                        break;
                    case "4":
                        ExcluirHeroi();
                        break;
                    case "5":
                        Console.Write("Saindo do programa! ");
                        break;
                    default:
                        Console.Write("Opcao inv�lida! Escolha uma op��o v�lida. ");
                        break;
                }

                Console.WriteLine(pressioneQualquerTecla);
                Console.ReadKey();
            }
            while (opcao != "5");
        }

        static void ShowMenu()
        {
            Console.Clear();
            try
            {
                    var lstData = GetHeros();
                Console.WriteLine(" -- Nome Completo --|-- Codinome --| -- Lan�amento -- | -- Poder --| -- Cadastrado Em:");
                foreach (var item in lstData)
                {
                    Console.WriteLine(" -- {0} --|-- {1} --| -- {2} -- | -- {3} --| -- {4} -- ",
                        item.NomeCompleto, item.Codinome, item.Nascimento, item.Poder, item.DataCadastro);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Oops! Algo deu errado");
                throw ex;
            }
            Console.WriteLine("*** Walsh Enterprise Apresenta*** ");
            Console.WriteLine("*** Gerenciador de Herois *** ");
            Console.WriteLine("1 - Pesquisar Herois:");
            Console.WriteLine("2 - Adicionar Herois:");
            Console.WriteLine("3 - Alterar Herois:");
            Console.WriteLine("4 - Excluir Herois:");
            Console.WriteLine("5 - Sair:");
            Console.WriteLine("\nEscolha uma das op��es acima: ");
        }

        static void PesquisarHerois()
        {
            Console.WriteLine("Informe o nome do H�roi que deseja pesquisar:");
            var termoDePesquisa = Console.ReadLine();
            var heroisEncontrados = _repositorio.Pesquisar(termoDePesquisa).ToList();

            if (heroisEncontrados.Count > 0)
            {
                Console.WriteLine("Selecione uma das op��es abaixo para visualizar os dados dos H�rois encontrados:");
                for (var index = 0; index < heroisEncontrados.Count; index++)
                    Console.WriteLine($"{index} - {heroisEncontrados[index].NomeCodinome()}");

                if (!ushort.TryParse(Console.ReadLine(), out var indexAExibir) || indexAExibir >= heroisEncontrados.Count)
                {
                    Console.Write($"Op��o inv�lida! ");
                    return;
                }

                if (indexAExibir < heroisEncontrados.Count)
                {
                    var heroi = heroisEncontrados[indexAExibir];

                    Console.WriteLine("Dados do H�roi:");
                    Console.WriteLine($"Nome do H�roi: {heroi.NomeCodinome()}");
                    Console.WriteLine($"Data de Cria��o: {heroi.Nascimento:dd/MM/yyyy}");
                    Console.WriteLine($"Poder do H�roi: {heroi.Poder}");

                    var qtdeDiasParaOProximoAniversario = heroi.ObterQtdeDeDiasParaOProximoAniversario();
                    Console.Write(ObterMensagemAniversario(qtdeDiasParaOProximoAniversario));
                }
            }
            else
            {
                Console.Write("N�o foi encontrado nenhum H�roi! ");
            }
        }

        static void AdicionarHeroi()
        {
            Console.WriteLine("Informe o nome do H�roi que deseja adicionar:");
            var nomeCompleto = Console.ReadLine();

            Console.WriteLine("Informe o codinome do H�roi que deseja adicionar:");
            var codinome = Console.ReadLine();

            Console.WriteLine("Informe a data de lan�amento do H�roi (formato dd/MM/yyyy):");
            if (!DateTime.TryParse(Console.ReadLine(), out var Nascimento))
            {
                Console.Write($"Dado inv�lido! Dados descartados! ");
                return;
            }

            Console.WriteLine("Informe se o H�roi tem algum SuperPoder:");
            Console.WriteLine("Poderes m�gicos - 1 | Super For�a - 2 | Cura - 3 | Invencibilidade - 4 | Voar - 5");
            if (!byte.TryParse(Console.ReadLine(), out var poderByte) || poderByte > 6)
            {
                Console.Write($"Dado inv�lido! Dados descartados! ");
                return;
            }
            var poderEnum = (Poder)poderByte;

            Console.WriteLine("Dados informados: ");
            Console.WriteLine($"Nome Completo: {nomeCompleto}");
            Console.WriteLine($"Codinome: {codinome}");
            Console.WriteLine($"Data nascimento: {Nascimento:dd/MM/yyyy}");
            Console.WriteLine($"Poder: {poderEnum}");

            Console.WriteLine("Os dados acima est�o corretos?");
            Console.WriteLine("1 - Sim \n2 - N�o");

            var opcaoParaAdicionar = Console.ReadLine();

            var DataCadastro = DateTime.Now;

            if (opcaoParaAdicionar == "1")
            {
                using (var connection = new SqliteConnection("DataSocurce=Herois.db"))
                {
                    //Montagem da query para adicionar novo Heroi
                    var command = connection.CreateCommand();
                    string query = "INSERT INTO Herois(HeroisID, NomeCompleto, Codinome, Nascimento, Poder, DataCadastro)"
                        + " VALUES(" + nomeCompleto + "'" + ","
                        + "'" + codinome + "'" + ","
                        + "'" + Nascimento + "'" + ","
                        + "'" + poderEnum + "'" + ","
                        + DataCadastro
                        + ");";

                    command.CommandText = query;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                Console.Write($"H�roi adicionado com sucesso! ");
            }
            else if (opcaoParaAdicionar == "2")
            {
                Console.Write($"Dados descartados! ");
            }
            else
            {
                Console.Write($"Op��o inv�lida! Tente de novo ");
            }
        }

        static void AlterarHeroi()
        {
            Console.WriteLine("Informe o nome do H�roi que deseja alterar:");
            var nomeCompleto = Console.ReadLine();

            Console.WriteLine("Informe o codinome do H�roi que deseja alterar:");
            var codinome = Console.ReadLine();

            Console.WriteLine("Informe a data de lan�amento do H�roi (formato dd/MM/yyyy):");
            if (!DateTime.TryParse(Console.ReadLine(), out var dataNascimento))
            {
                Console.Write($"Dado inv�lido! Dados descartados! ");
                return;
            }

            Console.WriteLine("Informe se o H�roi tem algum Super Poder:");
            Console.WriteLine("Poderes m�gicos - 1 | Super For�a - 2 | Cura - 3 | Invencibilidade - 4 | Voar - 5");
            if (!byte.TryParse(Console.ReadLine(), out var poderByte) || poderByte > 6)
            {
                Console.Write($"Dado inv�lido! Dados descartados! ");
                return;
            }
            var poderEnum = (Poder)poderByte;

            Console.WriteLine("Dados informados: ");
            Console.WriteLine($"Nome Completo: {nomeCompleto}");
            Console.WriteLine($"Codinome: {codinome}");
            Console.WriteLine($"Data nascimento: {dataNascimento:dd/MM/yyyy}");
            Console.WriteLine($"Poder: {poderEnum}");

            Console.WriteLine("Os dados acima est�o corretos?");
            Console.WriteLine("1 - Sim \n2 - N�o");

            var opcaoParaAdicionar = Console.ReadLine();

            if (opcaoParaAdicionar == "1")
            {
                using (var connection = new SqliteConnection("DataSocurce=Herois.db"))
                {
                    //Montagem da query para adicionar novo Heroi
                    var command = connection.CreateCommand();
                    string query = "UPDATE Herois SET"
                    + "NomeCompleto = '" + nomeCompleto + "'" + ","
                    + "Codinome = '" + codinome + "'" + ","
                    + "Nascimento = '" + dataNascimento + "'" + ","
                    + "Poder = '" + poderEnum + "'" + ","
                    + "WHERE NomeCompleto =" nomeCompleto + ";";

                    command.CommandText = query;
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.Write($"H�roi alterado com sucesso! ");
                }
            }
            else if (opcaoParaAdicionar == "2")
            {
                Console.Write($"Dados n�o alterados! ");
            }
            else
            {
                Console.Write($"Op��o inv�lida! Tente de novo ");
            }
        }

        static void ExcluirHeroi()
        {

            Console.WriteLine("Informe o nome do H�roi que deseja excluir:");
            var nomeCompleto = Console.ReadLine();
            using (var connection = new SqliteConnection("DataSocurce=Herois.db"))
            {
                //Montagem da query para deletar Heroi
                var command = connection.CreateCommand();
                string query = "DELETE FROM Herois WHERE nomeCompleto =" nomeCompleto + ";";

                command.CommandText = query;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        static List<Herois> GetHeros()
        {
            List<Herois> lstHeroi = new List<Herois>();
            using (var connection = new SqliteConnection("DataSocurce=Herois.db"))
            {
                //Montagem da query para deletar Heroi
                var command = connection.CreateCommand();
                string query = "Select * FROM Herois;";

                command.CommandText = query;
                connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Herois s = new Herois();
                    s.HeroiID = Convert.ToInt32(reader["HeroiID"]);
                    s.nomeCompleto = Convert.ToString(reader["NomeCompleto"]);
                    s.Codinome = Convert.ToString(reader["Codinome"]);
                    s.Nascimento = Convert.ToString(reader["Nascimento"]);
                    s.DataCadastro = Convert.ToString(reader["DataCadastro"]);
                    lstHeroi.Add(s);
                }

                return lstHeroi;
                }
            }
        }

        static string ObterMensagemAniversario(double qtdeDias)
        {
            if (double.IsNegative(qtdeDias))
                return $"Este H�roi j� fez anivers�rio neste ano! ";
            else if (qtdeDias.Equals(0d))
                return $"Este H�roi faz anivers�rio hoje! ";
            else
                return $"Faltam {qtdeDias:N0} dia(s) para o anivers�rio deste H�roi! ";
        }
    }
}