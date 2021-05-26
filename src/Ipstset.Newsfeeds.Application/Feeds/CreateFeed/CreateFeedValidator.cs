using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Feeds.CreateFeed
{
    public class CreateFeedValidator: AbstractValidator<CreateFeedRequest>
    {
        public CreateFeedValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode("feed_create_name").WithMessage("required");
        }
    }
}
