using ChefKnivesCommentsDatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ChefKnivesCommentsTest
{
    internal class TestDatabase : RedditContentDatabase
    {
        public TestDatabase(string subreddit) : base(subreddit) { }

        protected override void UpsertIntoCollection(RedditComment comment)
        {
            throw new Exception("UpsertIntoCollection was hit");
        }

        protected override void UpsertIntoCollection(RedditPost comment)
        {
            throw new Exception("UpsertIntoCollection was hit");
        }
    }

    [TestClass]
    public class DatabaseTest
    {
        [TestMethod]
        public void CachePreventsUpsertComments()
        {
            RedditContentDatabase database = new TestDatabase(subreddit: "test");
            List<RedditComment> comments = new List<RedditComment>()
            {
                new RedditComment()
                {
                    Author = "test",
                    Body = "this is a comment",
                    Id = "1",
                    PostLinkId = "123"
                }
            };

            // first insert should hit upsert, that's fine
            Assert.ThrowsException<Exception>(() => database.EnsureCommentsInDatabase(comments));

            // second insert must be caught by the cache, else we have a serious problem
            database.EnsureCommentsInDatabase(comments);
        }

        [TestMethod]
        public void CachePreventsUpsertPosts()
        {
            RedditContentDatabase database = new TestDatabase(subreddit: "test");
            List<RedditPost> posts = new List<RedditPost>()
            {
                new RedditPost()
                {
                    Author = "test",
                    Title = "this is a comment",
                    Id = "1"
                }
            };

            // first insert should hit upsert, that's fine
            Assert.ThrowsException<Exception>(() => database.EnsurePostsInDatabase(posts));

            // second insert must be caught by the cache, else we have a serious problem
            database.EnsurePostsInDatabase(posts);
        }
    }
}
