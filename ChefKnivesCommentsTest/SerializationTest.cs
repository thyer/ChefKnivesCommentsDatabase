using ChefKnivesCommentsDatabase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace ChefKnivesCommentsTest
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void DoesJsonDeserialize()
        {
            string jsonMessage = File.ReadAllText("HttpSampleResponse.json");
            RedditCommentQueryResponse response = JsonConvert.DeserializeObject<RedditCommentQueryResponse>(jsonMessage);

            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.data.children.Length, "Two comments should be deserialized");

            for(int i = 0; i < 2; ++i)
            {
                Assert.AreEqual($"author{i + 1}", response.data.children[i].data.author);
            }
        }

        [TestMethod]
        public void RedditCommentReaderCanRetrieveMostRecentComment()
        {
            RedditCommentReader reader = new RedditCommentReader("r/chefknives");
            RedditComment comment = reader.GetRecentComments(1).FirstOrDefault();

            Assert.IsNotNull(comment);
            Assert.IsNotNull(comment.Author);
            Assert.IsNotNull(comment.Body);
        }

        [TestMethod]
        public void RedditCommentReaderCanRetrieveMostRecent10Comments()
        {
            RedditCommentReader reader = new RedditCommentReader("r/chefknives");
            var comments = reader.GetRecentComments(10);

            Assert.AreEqual(10, comments.Count());

            foreach (RedditComment comment in comments)
            {
                Assert.IsNotNull(comment);
                Assert.IsNotNull(comment.Author);
                Assert.IsNotNull(comment.Body);
            }
        }
    }
}
