using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace dm_backend.Utilities{
    public static class Sorting{
        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> source, 
            string property,
            string dir)
        {
            string methodName = dir.ToLower().StartsWith("asc") ? "OrderBy" : "OrderByDescending";
            return ApplyOrder<T>(source, property, methodName);
        }

        public static IOrderedQueryable<T> ThenBy<T>(
            this IOrderedQueryable<T> source, 
            string property,
            string dir)
        {
            string methodName = dir.ToLower().StartsWith("asc") ? "ThenBy" : "ThenByDescending";
            return ApplyOrder<T>(source, property, "ThenBy");
        }
        public static IOrderedQueryable<T> ApplyOrder<T>(
            IQueryable<T> source, 
            string property, 
            string methodName) 
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach(string prop in props) {
                PropertyInfo pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] {source, lambda});
            return (IOrderedQueryable<T>)result;
        }
    }
}