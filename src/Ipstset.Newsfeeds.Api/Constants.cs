using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ipstset.Newsfeeds.Api
{
    public class Constants
    {
        public const string HashKey = "test_hash_key";
        public const string ClientCredentialsClientIdForSystemUser = "dev_client";
        public const string SystemUserId = "7c685c1a-e6be-408e-bbea-7fa35f1c074b";

        public const int MaxRequestLimit = 100;

        public class Routes
        {
            public class Feeds
            {
                public const string GetFeed = "GetFeed";
                public const string CreateFeed = "CreateFeed";
                public const string UpdateFeed = "UpdateFeed";
                public const string DeleteFeed = "DeleteFeed";
            }

            public class Posts
            {
                public const string GetPost = "GetPost";
                public const string CreatePost = "CreatePost";
                public const string UpdatePost = "UpdatePost";
                public const string DeletePost = "DeletePost";

                public const string PublishPost = "PublishPost";
                public const string UnpublishPost = "UnpublishPost";
            }


        }
    }
}
