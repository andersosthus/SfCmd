﻿using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.CommandLineUtils;

namespace ServiceFabricUploader.Models
{
    public class AppOptionsRaw
    {
        public AppOptionsRaw(CommandLineApplication app)
        {
            ClusterHostname = app.Option("--hostname", "Cluster hostname", CommandOptionType.SingleValue);
            ClusterPort =
                app.Option("--port", "Cluster API port (defaults to 19080)",
                    CommandOptionType.SingleValue);
            CertificateThumbprint =
                app.Option("--certThumb", "Certificate thumbprint for secure cluster",
                    CommandOptionType.SingleValue);
            CertificateStore =
                app.Option("--certStore", "Certificate store for thumbprint (defaults to My)",
                    CommandOptionType.SingleValue);
            CertificateLocation =
                app.Option("--certLocation",
                    "Certificate store location for thumbprint (defaults to CurrentUser)",
                    CommandOptionType.SingleValue);
            Verbose = app.Option("--verbose", "Verbose output", CommandOptionType.NoValue);
        }

        public CommandOption ClusterHostname { get; set; }
        public CommandOption ClusterPort { get; set; }
        public CommandOption CertificateThumbprint { get; set; }
        public CommandOption CertificateStore { get; set; }
        public CommandOption CertificateLocation { get; set; }
        public CommandOption Verbose { get; set; }
    }

    public class AppOptions
    {
        public bool SecureCluster { get; set; }
        public string ClusterHostname { get; set; }
        public int ClusterPort { get; set; }
        public string CertificateThumbprint { get; set; }
        public StoreName CertificateStore { get; set; }
        public StoreLocation CertificateLocation { get; set; }
        public bool Verbose { get; set; }

        public static AppOptions ValidateAndCreate(AppOptionsRaw rawOptions)
        {
            if (!rawOptions.ClusterHostname.HasValue())
                return new InvalidAppOptions("No ClusterHostname specified");

            var config = new AppOptions
            {
                SecureCluster = rawOptions.CertificateThumbprint.HasValue(),
                ClusterHostname = rawOptions.ClusterHostname.Value(),
                ClusterPort = OptionsHelper.GetIntOrDefaultValue(rawOptions.ClusterPort, 19080),
                CertificateThumbprint = OptionsHelper.GetStringOrDefaultValue(rawOptions.CertificateThumbprint, string.Empty),
                CertificateStore = OptionsHelper.GetEnumValueOrDefault(rawOptions.CertificateStore, StoreName.My),
                CertificateLocation = OptionsHelper.GetEnumValueOrDefault(rawOptions.CertificateLocation, StoreLocation.CurrentUser),
                Verbose = rawOptions.Verbose.HasValue()
            };

            return config;
        }
    }

    internal class InvalidAppOptions : AppOptions
    {
        public InvalidAppOptions(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}