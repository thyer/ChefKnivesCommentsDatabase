using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ChefKnivesCommentsDatabase
{
    class Program
    {
        /// <summary>
        /// How long to wait between cycles of retrieving posts/comments
        /// </summary>
        private static readonly TimeSpan cycleLength = TimeSpan.FromMinutes(30);

        static void Main()
        {
            RedditHttpsReader redditReader = new RedditHttpsReader(subreddit: "chefknives");
            using (RedditContentDatabase redditDatabase = new RedditContentDatabase(subreddit: "chefknives"))
            {
                while (true)
                {
                    IEnumerable<RedditPost> recentPosts = redditReader.GetRecentPosts(numPosts: 30);
                    redditDatabase.EnsurePostsInDatabase(recentPosts);
                    Console.WriteLine($"I added recent posts");

                    IEnumerable<RedditComment> recentComments = redditReader.GetRecentComments(numComments: 100);
                    redditDatabase.EnsureCommentsInDatabase(recentComments);
                    Console.WriteLine($"I added recent comments");
                    Thread.Sleep(cycleLength);
                }
            }
        }
    }
}
