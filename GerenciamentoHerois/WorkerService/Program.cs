using Dominio;
using Dominio.Manipuladores;
using Infraestrutura.Contextos;
using Infraestrutura.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace WorkerService
{
    class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddDbContext<HeroiContexto>(ServiceLifetime.Scoped);

            services.AddTransient<IHeroiRepositorio, HeroiRepositorio>();
            services.AddTransient<HeroisManupulador, HeroisManupulador>();

            var serviceProvider = services.BuildServiceProvider();
            var startarAplicacao = serviceProvider.GetRequiredService<IWorker>();
            startarAplicacao.Iniciar();
        }
    }
}