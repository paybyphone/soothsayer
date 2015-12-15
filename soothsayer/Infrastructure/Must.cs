using System;
using System.Linq.Expressions;

namespace soothsayer.Infrastructure
{
    public static class Must
    {
        public static void Be(Func<bool> predicate, string message = null)
        {
            var result = predicate();

            if (!result)
            {
                throw new ArgumentException(message ?? "Expected condition failed");
            }
        }

        public static void NotBeNull(Expression<Func<object>> objectExpression, string message = null)
        {
            EvaluateExpressionAgainstCondition(objectExpression, o => o == null, v => message.IsNotNull() ? new ArgumentNullException(v, message) : new ArgumentNullException(v));
        }

        private static void EvaluateExpressionAgainstCondition<T>(Expression<Func<T>> expression, Func<T, bool> predicate, Func<string, Exception> exceptionFactoryMethod)
        {
            var value = expression.Compile().Invoke();

            if (predicate(value))
            {
                var memberExpression = expression.Body as MemberExpression;
                var variableName = memberExpression.Member.Name;

                throw exceptionFactoryMethod(variableName);
            }
        }
    }
}