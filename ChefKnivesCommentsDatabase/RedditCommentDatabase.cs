using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace ChefKnivesCommentsDatabase
{
    internal class RedditCommentDatabase : IDisposable
    {
        private readonly string subreddit;
        private static readonly string ConnectionString = Environment.GetEnvironmentVariable("connectionString");
        private readonly MongoClient mongoClient = new MongoClient(ConnectionString);
        private const string commentsCollectionName = "comments";

        public RedditCommentDatabase(string subreddit)
        {
            this.subreddit = subreddit;
            if (!mongoClient.GetDatabase(subreddit).ListCollections(new ListCollectionsOptions { Filter = new BsonDocument("name", commentsCollectionName) }).Any())
            {
                GetMongoCollection().InsertOne(new BsonDocument()); // completely empty, but ensures the database and collection is set up the first time
            }
        }

        /// <summary>
        /// Ensures each comment in the collection is in the database
        /// </summary>
        internal void AddCollection(IEnumerable<RedditComment> comments)
        {
            foreach (RedditComment comment in comments)
            {
                UpsertIntoCollection(comment);
                Console.WriteLine($"Wrote comment id {comment.Id} by {comment.Author} to database");
            }
        }

        private IMongoCollection<BsonDocument> GetMongoCollection()
        {
            return mongoClient.GetDatabase(subreddit).GetCollection<BsonDocument>(commentsCollectionName);
        }

        private void UpsertIntoCollection(RedditComment comment)
        {
            var collection = GetMongoCollection();
            collection.ReplaceOne(
                filter: new BsonDocument("id", comment.Id),
                options: new ReplaceOptions { IsUpsert = true },
                replacement: comment.ToBsonDocument());
        }

        public void Dispose()
        {
            // Maybe cleanup some of the database?
        }
    }
}