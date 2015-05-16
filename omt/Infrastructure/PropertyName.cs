using System;
using System.Linq.Expressions;

namespace omt.Infrastructure
{
    public static class PropertyName
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