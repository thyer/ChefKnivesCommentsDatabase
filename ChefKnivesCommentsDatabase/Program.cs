using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ChefKnivesCommentsDatabase
{
    class Program
    {
        static void Main()
        {
            RedditCommentReader redditReader = new RedditCommentReader(subreddit: "chefknives");
            using (RedditCommentDatabase redditDatabase = new RedditCommentDatabase(subreddit: "chefknives"))
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    IEnumerable<RedditComment> recentComments = redditReader.GetRecentComments(numComments: 100);
                    redditDatabase.AddCollection(recentComments);
                    Console.WriteLine($"I added {recentComments.Count()} comments");
                }
            }
        }
    }
}
