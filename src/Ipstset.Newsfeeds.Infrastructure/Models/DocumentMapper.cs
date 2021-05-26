using Ipstset.Newsfeeds.Application.Feeds;
using Ipstset.Newsfeeds.Application.Posts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Infrastructure.Models
{
    public class DocumentMapper
    {
        public static PostResponse ToPostResponse(Document document)
        {
            try
            {
                //parse data
                var data = JsonConvert.DeserializeObject<PostDocument>(document.Data);
                if (data != null)
                    return new PostResponse
                    {
                        Id = data.Id,
                        FeedId = data.FeedId,
                        Title = data.Title,
                        Content = data.Content,
                        CreatedByUserId = data.CreatedByUserId,
                        DateCreated = data.DateCreated,
                        DatePublished = data.DatePublished,
                        Tags = data.Tags
                    };

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static FeedResponse ToFeedResponse(Document document)
        {
            try
            {
                //parse data
                var data = JsonConvert.DeserializeObject<FeedDocument>(document.Data);
                if (data != null)
                    return new FeedResponse
                    {
                        Id = data.Id,
                        Name = data.Name,
                        IsPublic = data.IsPublic,
                        CreatedUserId = data.CreatedUserId,
                        DateCreated = data.DateCreated
                    };

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

