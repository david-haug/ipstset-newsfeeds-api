using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts.CreatePost
{
    public class CreatePostValidator : AbstractValidator<CreatePostRequest>
    {
        public CreatePostValidator()
        {
            RuleFor(x => x.FeedId).NotEmpty().WithErrorCode("post_create_feedId").WithMessage("required");
            RuleFor(x => x.FeedId).NotEqual(Guid.Empty.ToString()).WithErrorCode("post_create_feedId").WithMessage("invalid");
            RuleFor(x => x.Title).NotEmpty().WithErrorCode("post_create_title").WithMessage("required");
            RuleFor(x => x.Content).NotEmpty().WithErrorCode("post_create_content").WithMessage("required");
        }
    }
}
