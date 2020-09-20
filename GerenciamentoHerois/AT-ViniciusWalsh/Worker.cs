using Dominio;
using Microsoft.Extensions.Hosting;
using System;
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
                        Console.Write("Saindo do programa! ");
                        break;
                    default:
                        Console.Write("Opcao inválida! Escolha uma opção válida. ");
                        break;
                }

                Console.WriteLine(pressioneQualquerTecla);
                Console.ReadKey();
            }
            while (opcao != "3");
        }

        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("*** Walsh Enterprise Apresenta*** ");
            Console.WriteLine("*** Gerenciador de Herois *** ");
            Console.WriteLine("1 - Pesquisar Herois:");
            Console.WriteLine("2 - Adicionar Herois:");
            Console.WriteLine("3 - Sair:");
            Console.WriteLine("\nEscolha uma das opções acima: ");
        }

        static void PesquisarHerois()
        {
            Console.WriteLine("Informe o nome do Héroi que deseja pesquisar:");
            var termoDePesquisa = Console.ReadLine();
            var heroisEncontrados = _repositorio.Pesquisar(termoDePesquisa).ToList();

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

                    var qtdeDiasParaOProximoAniversario = heroi.ObterQtdeDeDiasParaOProximoAniversario();
                    Console.Write(ObterMensagemAniversario(qtdeDiasParaOProximoAniversario));
                }
            }
            else
            {
                Console.Write("Não foi encontrado nenhum Héroi! ");
            }
        }

        static void AdicionarHeroi()
        {
            Console.WriteLine("Informe o nome do Héroi que deseja adicionar:");
            var nomeCompleto = Console.ReadLine();

            Console.WriteLine("Informe o codinome do Héroi que deseja adicionar:");
            var codinome = Console.ReadLine();

            Console.WriteLine("Informe a data de lançamento do Héroi (formato dd/MM/yyyy):");
            if (!DateTime.TryParse(Console.ReadLine(), out var dataNascimento))
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
            Console.WriteLine($"Data nascimento: {dataNascimento:dd/MM/yyyy}");
            Console.WriteLine($"Poder: {poderEnum}");

            Console.WriteLine("Os dados acima estão corretos?");
            Console.WriteLine("1 - Sim \n2 - Não");

            var opcaoParaAdicionar = Console.ReadLine();

            if (opcaoParaAdicionar == "1")
            {
                var newHero = new Heroi(nomeCompleto, codinome, dataNascimento, poderEnum);
                _repositorio.Adicionar(newHero);
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

        static string ObterMensagemAniversario(double qtdeDias)
        {
            if (double.IsNegative(qtdeDias))
                return $"Este Héroi já fez aniversário neste ano! ";
            else if (qtdeDias.Equals(0d))
                return $"Este Héroi faz aniversário hoje! ";
            else
                return $"Faltam {qtdeDias:N0} dia(s) para o aniversário deste Héroi! ";
        }
    }
}
