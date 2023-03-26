using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N5Challenge.Api.Core.Interfaces;
using N5Challenge.Api.Core.Persistence;
using N5Challenge.Api.Infraestructure.Repositories;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Infraestructure.ServiceExtension
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["ELS:Url"];
            var defaultIndex = configuration["ELS:index"];

            var settings = new ConnectionSettings(new Uri(url))
                .PrettyJson()
                .DefaultIndex(defaultIndex);

            // Add mapping (with ignores)
            AddDefaultMappings(settings);

            // Create a client
            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<Permissions>(p => p);

            // Ignore fields
            settings.
                DefaultMappingFor<Permissions>(p => p
                    .Ignore(p => p.FechaPermiso)
                    .Ignore(p => p.Id)
            );
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<Permissions>(x => x.AutoMap())
            );
        }
    }
}
