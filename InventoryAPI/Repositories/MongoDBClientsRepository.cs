using System;
using System.Collections.Generic;
using InventoryAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InventoryAPI.Repositories
{
    public class MongoDBClientsRepository : IClientRepository
    {
        private const string _databaseName = "catalog";
        private const string _collectionName = "clients";
        
        private readonly IMongoCollection<Client> _clientsCollection;
        private readonly FilterDefinitionBuilder<Client> _filterBuilder = Builders<Client>.Filter;
        
        public MongoDBClientsRepository( IMongoClient mongoClient )
        {
            IMongoDatabase database = mongoClient.GetDatabase( _databaseName );
            _clientsCollection = database.GetCollection<Client>( _collectionName );
        }
        
        public Client GetClient(Guid clientID)
        {
            var filter = _filterBuilder.Eq(client => client.Id, clientID);
            return _clientsCollection.Find(filter).SingleOrDefault();
        }

        public IEnumerable<Client> GetClients()
        {
            return _clientsCollection.Find(new BsonDocument()).ToList();
        }

        public void CreateClient(Client client)
        {
            _clientsCollection.InsertOne( client );
        }

        public void UpdateClient(Client client)
        {
            var filter = _filterBuilder.Eq(existingClient => existingClient.Id, client.Id);
            _clientsCollection.ReplaceOne(filter, client);
        }

        public void DeleteClient(Guid id)
        {
            var filter = _filterBuilder.Eq(client => client.Id, id);
            _clientsCollection.DeleteOne(filter);
        }
    }
}