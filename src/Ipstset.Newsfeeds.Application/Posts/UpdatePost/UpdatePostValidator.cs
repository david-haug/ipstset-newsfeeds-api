using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Posts.UpdatePost
{
    public class UpdatePostValidator: AbstractValidator<UpdatePostRequest>
    {
        public UpdatePostValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithErrorCode("post_update_title").WithMessage("required");
            RuleFor(x => x.Content).NotEmpty().WithErrorCode("post_update_content").WithMessage("required");
        }
    }
}
