using System;
using System.Linq.Expressions;

namespace soothsayer.Infrastructure
{
    public static class Name
    {
        public static string For(Expression<Func<object>> exp)
        {
            var body = exp.Body as MemberExpression;

            if (body == null)
            {
                var ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            if (body.IsNull())
            {
                throw new InvalidOperationException("Could not determine the Name for the given expression");
            }

            return body.Member.Name;
        }
    }
}
