﻿using System.Linq;
using System.Net;
using System.Net.Sockets;
using Convey;
using Convey.Discovery.Consul;
using Convey.HTTP;

namespace Spirebyte.Services.Repositories.Infrastructure.ServiceDiscovery;

public static class Extensions
{
    public static IConveyBuilder AddCustomConsul(
        this IConveyBuilder builder,
        string sectionName = "consul",
        string httpClientSectionName = "httpClient")
    {
        if (string.IsNullOrWhiteSpace(sectionName))
            sectionName = "consul";
        
        var consulOptions = builder.GetOptions<ConsulOptions>(sectionName);
        var httpClientOptions = builder.GetOptions<HttpClientOptions>(httpClientSectionName);

        if (consulOptions.Address == "[hostname]")
        {
            var name = Dns.GetHostName(); // get container id
            var ip = Dns.GetHostEntry(name).AddressList
                .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);

            consulOptions.Address = ip.ToString();
        }
        
        builder.AddConsul(consulOptions, httpClientOptions);
        
        return builder;
    }
}