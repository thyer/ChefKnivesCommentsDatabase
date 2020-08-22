using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace ChefKnivesCommentsDatabase
{
    public class RedditCommentReader
    {
        private readonly string subreddit;
        private const string RedditUrlPrefix = "https://reddit.com/r/";
        private const string RedditUrlCommentsRequest = "/comments.json";
        private const string RedditUrlLimitString = "?limit=";
        private readonly HttpClient httpClient = new HttpClient();

        public RedditCommentReader(string subreddit)
        {
            this.subreddit = subreddit;
        }

        private string GetTopCommentsRequestUrl(int numComments)
        {
            return $"{ RedditUrlPrefix}{subreddit}{RedditUrlCommentsRequest}{RedditUrlLimitString}{numComments}";
        }

        public IEnumerable<RedditComment> GetRecentComments(int numComments)
        {
            List<RedditComment> output = new List<RedditComment>();
            using (var httpResponse = httpClient.GetAsync(GetTopCommentsRequestUrl(numComments)).Result)
            {
                HttpContent content = httpResponse.Content;
                if (httpResponse.StatusCode != HttpStatusCode.OK || content == null)
                {
                    return output;
                }

                RedditCommentQueryResponse parsedContent = JsonConvert.DeserializeObject<RedditCommentQueryResponse>(content.ReadAsStringAsync().Result);
                foreach(Comment redditComment in parsedContent?.data?.children)
                {
                    RedditComment toAdd = new RedditComment(redditComment.data);
                    output.Add(toAdd);
                }
            }

            return output;
        }
    }
}