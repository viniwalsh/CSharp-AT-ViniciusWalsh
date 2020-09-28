﻿using Dominio;
using Dominio.Manipuladores;
using Infraestrutura.Contextos;
using Infraestrutura.Repositorios;
using Microsoft.Extensions.DependencyInjection;

namespace WorkerService.Configuracao
{
    public class DIConfiguracao
    {
        public static void Registrar(IServiceCollection services)
        {
            services.AddDbContext<HeroiContexto>(ServiceLifetime.Scoped);

            services.AddTransient<IHeroiRepositorio, HeroiRepositorio>();
            services.AddTransient<HeroisManupulador, HeroisManupulador>();
        }
    }
}