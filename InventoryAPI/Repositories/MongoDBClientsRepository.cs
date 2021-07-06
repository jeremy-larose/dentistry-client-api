using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        
        public async Task<Client> GetClientAsync(Guid clientID)
        {
            var filter = _filterBuilder.Eq(client => client.Id, clientID);
            return await _clientsCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            return await _clientsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task CreateClientAsync(Client client)
        {
            await _clientsCollection.InsertOneAsync( client );
        }

        public async Task UpdateClientAsync(Client client)
        {
            var filter = _filterBuilder.Eq(existingClient => existingClient.Id, client.Id);
            await _clientsCollection.ReplaceOneAsync(filter, client);
        }

        public async Task DeleteClientAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(client => client.Id, id);
            await _clientsCollection.DeleteOneAsync(filter);
        }
    }
}