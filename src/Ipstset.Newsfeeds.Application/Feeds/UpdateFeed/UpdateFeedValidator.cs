using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Feeds.UpdateFeed
{
    public class UpdateFeedValidator:AbstractValidator<UpdateFeedRequest>
    {
        public UpdateFeedValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode("feed_update_name").WithMessage("required");
        }
    }
}
