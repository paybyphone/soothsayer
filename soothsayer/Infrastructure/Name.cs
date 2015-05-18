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

            return body.Member.Name;
        }
    }
}
