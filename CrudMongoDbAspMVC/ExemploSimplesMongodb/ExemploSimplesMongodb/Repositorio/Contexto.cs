﻿using System;
using System.Collections.Generic;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver;

namespace ExemploSimplesMongodb.Repositorio
{
    public class Contexto<T> where T : class
    {
        public static string GetMongoDbConnectionString()
        {
            return ConfigurationManager.AppSettings.Get("MONGOLAB_URI") ??
                   ConfigurationManager.ConnectionStrings["ExemploMongo"].ConnectionString;
        }


        public Contexto()
        {
            var url = new MongoUrl(GetMongoDbConnectionString());
            var client = new MongoClient(url);
            var server = client.GetServer();
            var database = server.GetDatabase(url.DatabaseName);
            Collection = database.GetCollection<T>(typeof(T).Name.ToLower());

            //TODO: Metodo obsoleto, corrigir isto para setar a data correta no servidor
            DateTimeSerializationOptions.Defaults = new DateTimeSerializationOptions(DateTimeKind.Local, BsonType.Document);

            var conventions = new ConvensoesMongo();
            ConventionRegistry.Register("Convensoes", conventions, t => true);

        }

        public MongoCollection<T> Collection { get; private set; }
    }

    public class ConvensoesMongo : IConventionPack
    {
        public IEnumerable<IConvention> Conventions
        {
            get
            {
                return new List<IConvention>
                             {
                                 new IgnoreExtraElementsConvention(true)
                             };
            }
        }
    }
}