﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using SCommon;
using SDB;

namespace PlayServer
{
    //=======================MongoDB InsertQuery Base ==========================
    public class InsertQuery : IMongoDBQuery
    {
        public string m_DBName;
        public string m_Collection;
        public BsonDocument m_Doc = new BsonDocument();

        public InsertQuery(string dbName, string colletionName, BsonDocument doc)
        {
            m_DBName = dbName;
            m_Collection = colletionName;
            m_Doc = doc;
        }

        public void Run(SMongoDB client)
        {
            try
            {
                if (!client.IsDatabaseExists(m_DBName))
                    return;

                var db = client.GetDataBase(m_DBName);

                if (!client.IsCollectionExists(db, m_Collection))
                    return;

                var collection = db.GetCollection<BsonDocument>(m_Collection);
                collection.InsertOne(m_Doc);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Complete()
        {
            Console.WriteLine("InsertQuery Complete!");
        }
    }

    public class InsertQueryMany : IMongoDBQuery
    {
        public string m_DBName;
        public List<BsonDocument> m_Docs = new List<BsonDocument>();

        public InsertQueryMany(string dbName, List<BsonDocument> doc)
        {
            m_DBName = dbName;
            m_Docs = doc;
        }

        public void Run(SMongoDB client)
        {
            try
            {
                if (!client.IsDatabaseExists(m_DBName))
                    return;

                DateTime utc_now = DateTime.UtcNow;
                var collectionName = $"log_{utc_now:yyyyMM}";
                var collection = client.DataBase.GetCollection<BsonDocument>(collectionName);
                collection.InsertMany(m_Docs);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Complete()
        {
        }
    }


    //=======================MongoDB FindQuery Base ==========================
    public class FindQuery : IMongoDBQuery
    {
        private string m_DBName;
        private string m_Collection;
        private FilterDefinition<BsonDocument> m_Filter;
        private List<BsonDocument> m_OutList = new List<BsonDocument>();

        public FindQuery(string dbname, string colletionname, FilterDefinition<BsonDocument> filter)
        {
            m_DBName = dbname;
            m_Collection = colletionname;
            m_Filter = filter;
        }

        public void Run(SMongoDB client)
        {
            try
            {
                if (!client.IsDatabaseExists(m_DBName))
                    return;

                var db = client.GetDataBase(m_DBName);

                if (!client.IsCollectionExists(db, m_Collection))
                    return;

                var collection = db.GetCollection<BsonDocument>(m_Collection);

                m_OutList = collection.Find(m_Filter).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        public void Complete()
        {
            //find after query
        }
    }

}
