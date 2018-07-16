using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Abstractions.Extension
{
    public static class MemberExpressionExtension
    {
        public static T GetMemberInstance<T, TMember>(this Expression<Func<TMember>> expression)
        {
            var me = (MemberExpression)((MemberExpression)expression.Body).Expression;
            var ce = (ConstantExpression)me.Expression;
            var fieldInfo = ce.Value.GetType().GetField(me.Member.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var instance = (T)fieldInfo.GetValue(ce.Value);

            return instance;
        }

        public static string GetMemberName<T, TMember>(this Expression<Func<TMember>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                return ((MemberExpression)expression.Body).Member.Name;
            }
            else
            {
                var op = ((UnaryExpression)expression.Body).Operand;
                return ((MemberExpression)op).Member.Name;
            }
        }

        public static object GetMemberValue<T, TMember>(this Expression<Func<TMember>> expression)
        {
            var instance = GetMemberInstance<T, TMember>(expression);
            return instance.GetType().GetProperty(GetMemberName<T, TMember>(expression)).GetValue(instance);
        }
    }
}