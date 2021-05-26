using System;
using System.Collections.Generic;
using System.Text;

namespace Ipstset.Newsfeeds.Application.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisifedBy(T entity);
    }
}
