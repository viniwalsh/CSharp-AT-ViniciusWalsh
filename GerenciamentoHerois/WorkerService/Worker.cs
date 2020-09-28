using Dominio;
using Dominio.Comandos;
using Dominio.Manipuladores;
using Dominio.ObjetosDeValor;
using Microsoft.Extensions.Hosting;
using System;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService
{
    public class Worker : BackgroundService, IWorker
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
                        Console.Write("Opcao inválida! Escolha uma opção válida. ");
                        break;
                }

                Console.WriteLine(pressioneQualquerTecla);
                Console.ReadKey();
            }
            while (opcao != "5");
        }

        public void Iniciar()
        {
            throw new NotImplementedException();
        }

        static void ShowMenu()
        {
            Console.Clear();
            try
            {
                var lstData = UltimosHerois();
                Console.WriteLine(" -- Nome Completo --|-- Codinome --| -- Lançamento -- | -- Poder --| -- Cadastrado Em:");
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
            Console.WriteLine("\nEscolha uma das opções acima: ");
        }

        private void PesquisarHerois()
        {
            Console.WriteLine("Informe o nome do Héroi que deseja pesquisar:");
            var termoDePesquisa = Console.ReadLine();
            var heroisEncontrados = _repositorio.PesquisaCodinome(termoDePesquisa).ToList();

            if (heroisEncontrados.Count > 0)
            {
                Console.WriteLine("Selecione uma das opções abaixo para visualizar os dados dos Hérois encontrados:");
                for (var index = 0; index < heroisEncontrados.Count; index++)
                    Console.WriteLine($"{index} - {heroisEncontrados[index].NomeCodinome()}");

                if (!ushort.TryParse(Console.ReadLine(), out var indexAExibir) || indexAExibir >= heroisEncontrados.Count)
                {
                    Console.Write($"Opção inválida! ");
                    return;
                }

                if (indexAExibir < heroisEncontrados.Count)
                {
                    var heroi = heroisEncontrados[indexAExibir];

                    Console.WriteLine("Dados do Héroi:");
                    Console.WriteLine($"Nome do Héroi: {heroi.NomeCodinome()}");
                    Console.WriteLine($"Data de Criação: {heroi.Nascimento:dd/MM/yyyy}");
                    Console.WriteLine($"Poder do Héroi: {heroi.Poder}");
                }
            }
            else
            {
                Console.Write("Não foi encontrado nenhum Héroi! ");
            }
        }

        private void AdicionarHeroi(HeroisManupulador manupulador)
        {
            Console.WriteLine("Informe o nome do Héroi que deseja adicionar:");
            var nomeCompleto = Console.ReadLine();

            Console.WriteLine("Informe o codinome do Héroi que deseja adicionar:");
            var codinome = Console.ReadLine();

            Console.WriteLine("Informe a data de lançamento do Héroi (formato dd/MM/yyyy):");
            if (!DateTime.TryParse(Console.ReadLine(), out var Nascimento))
            {
                Console.Write($"Dado inválido! Dados descartados! ");
                return;
            }

            Console.WriteLine("Informe se o Héroi tem algum SuperPoder:");
            Console.WriteLine("Poderes mágicos - 1 | Super Força - 2 | Cura - 3 | Invencibilidade - 4 | Voar - 5");
            if (!byte.TryParse(Console.ReadLine(), out var poderByte) || poderByte > 6)
            {
                Console.Write($"Dado inválido! Dados descartados! ");
                return;
            }
            var poderEnum = (Poder)poderByte;

            Console.WriteLine("Dados informados: ");
            Console.WriteLine($"Nome Completo: {nomeCompleto}");
            Console.WriteLine($"Codinome: {codinome}");
            Console.WriteLine($"Data nascimento: {Nascimento:dd/MM/yyyy}");
            Console.WriteLine($"Poder: {poderEnum}");

            Console.WriteLine("Os dados acima estão corretos?");
            Console.WriteLine("1 - Sim \n2 - Não");

            var opcaoParaAdicionar = Console.ReadLine();

            var DataCadastro = DateTime.Now;

            if (opcaoParaAdicionar == "1")
            {
                _repositorio.NovoHeroi(termoDePesquisa).ToList();
                Console.Write($"Héroi adicionado com sucesso! ");
            }
            else if (opcaoParaAdicionar == "2")
            {
                Console.Write($"Dados descartados! ");
            }
            else
            {
                Console.Write($"Opção inválida! Tente de novo ");
            }
        }

        private void AlterarHeroi()
        {
            Console.WriteLine("Informe o nome do Héroi que deseja alterar:");
            var nomeCompleto = Console.ReadLine();

            Console.WriteLine("Informe o codinome do Héroi que deseja alterar:");
            var codinome = Console.ReadLine();

            Console.WriteLine("Informe a data de lançamento do Héroi (formato dd/MM/yyyy):");
            if (!DateTime.TryParse(Console.ReadLine(), out var dataNascimento))
            {
                Console.Write($"Dado inválido! Dados descartados! ");
                return;
            }

            Console.WriteLine("Informe se o Héroi tem algum Super Poder:");
            Console.WriteLine("Poderes mágicos - 1 | Super Força - 2 | Cura - 3 | Invencibilidade - 4 | Voar - 5");
            if (!byte.TryParse(Console.ReadLine(), out var poderByte) || poderByte > 6)
            {
                Console.Write($"Dado inválido! Dados descartados! ");
                return;
            }
            var poderEnum = (Poder)poderByte;

            Console.WriteLine("Dados informados: ");
            Console.WriteLine($"Nome Completo: {nomeCompleto}");
            Console.WriteLine($"Codinome: {codinome}");
            Console.WriteLine($"Data nascimento: {dataNascimento:dd/MM/yyyy}");
            Console.WriteLine($"Poder: {poderEnum}");

            Console.WriteLine("Os dados acima estão corretos?");
            Console.WriteLine("1 - Sim \n2 - Não");

            var opcaoParaAdicionar = Console.ReadLine();

            if (opcaoParaAdicionar == "1")
            {
                using (var connection = new SQLiteConnection("DataSocurce=Herois.db"))
                {
                    //Montagem da query para adicionar novo Heroi
                    var command = connection.CreateCommand();
                    string query = "UPDATE Herois SET"
                    + "NomeCompleto = '" + nomeCompleto + "'" + ","
                    + "Codinome = '" + codinome + "'" + ","
                    + "Nascimento = '" + dataNascimento + "'" + ","
                    + "Poder = '" + poderEnum + "'" + ","
                    + "WHERE NomeCompleto =" + nomeCompleto + ";";

                    command.CommandText = query;
                    connection.Open();
                    command.ExecuteNonQuery();
                    Console.Write($"Héroi alterado com sucesso! ");
                }
            }
            else if (opcaoParaAdicionar == "2")
            {
                Console.Write($"Dados não alterados! ");
            }
            else
            {
                Console.Write($"Opção inválida! Tente de novo ");
            }
        }

        private void ExcluirHeroi(Guid id, string nomeCompleto)
        {
            Console.WriteLine("Informe o codinome do Héroi que deseja excluir:");
            var comando = new RemoverHeroiComando(id, nomeCompleto);
            var manipulador = new HeroisManupulador(_repositorio);
            var resultado = manipulador.Manupulador.ExcluirHeroi(comando);
        }

        private void UltimosHerois(string codenome, bool done)
        {
            _repositorio.MostrarUltimosHeroisRegistrados(c)
        }

        private static void MostrarResultado(string resultado)
        {
            if(resultado.Contains("ERRO!"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(resultado);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nResultado: " + resultado);
                Console.ResetColor();
            }
        }
    }
}
