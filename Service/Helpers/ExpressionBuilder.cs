using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Service.Helpers
{
    public enum OperatorComparer
    {
        Contains,
        StartsWith,
        EndsWith,
        Equals = ExpressionType.Equal,
        GreaterThan = ExpressionType.GreaterThan,
        GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
        LessThan = ExpressionType.LessThan,
        LessThanOrEqual = ExpressionType.LessThan,
        NotEqual = ExpressionType.NotEqual
    }

    public class ExpressionBuilder
    {
        public static Expression<Func<T, bool>> BuildPredicate<T>(object value, OperatorComparer comparer, params string[] properties)
        {
            var parameterExpression = Expression.Parameter(typeof(T), typeof(T).Name);
            return (Expression<Func<T, bool>>)BuildNavigationExpression(parameterExpression, comparer, value, properties);
        }

        private static Expression BuildNavigationExpression(Expression parameter, OperatorComparer comparer, object value, params string[] properties)
        {
            Expression resultExpression = null;
            Expression childParameter, predicate;
            Type childType = null;

            if (properties.Count() > 1)
            {
                //build path
                parameter = Expression.Property(parameter, properties[0]);
                var isCollection = typeof(IEnumerable).IsAssignableFrom(parameter.Type);
                //if it´s a collection we later need to use the predicate in the methodexpressioncall
                if (isCollection)
                {
                    childType = parameter.Type.GetGenericArguments()[0];
                    childParameter = Expression.Parameter(childType, childType.Name);
                }
                else
                {
                    childParameter = parameter;
                }
                //skip current property and get navigation property expression recursivly
                var innerProperties = properties.Skip(1).ToArray();
                predicate = BuildNavigationExpression(childParameter, comparer, value, innerProperties);
                if (isCollection)
                {
                    //build subquery
                    resultExpression = BuildSubQuery(parameter, childType, predicate);
                }
                else
                {
                    resultExpression = predicate;
                }
            }
            else
            {
                //build final predicate
                resultExpression = BuildCondition(parameter, properties[0], comparer, value);
            }
            return resultExpression;
        }

        private static Expression BuildSubQuery(Expression parameter, Type childType, Expression predicate)
        {
            var anyMethod = typeof(Enumerable).GetMethods().Single(m => m.Name == "Any" && m.GetParameters().Length == 2);
            anyMethod = anyMethod.MakeGenericMethod(childType);
            predicate = Expression.Call(anyMethod, parameter, predicate);
            return MakeLambda(parameter, predicate);
        }

        private static Expression BuildCondition(Expression parameter, string property, OperatorComparer comparer, object value)
        {
            var childProperty = parameter.Type.GetProperty(property);
            var left = Expression.Property(parameter, childProperty);
            var right = Expression.Constant(value);
            var predicate = BuildComparsion(left, comparer, right);
            return MakeLambda(parameter, predicate);
        }

        private static Expression BuildComparsion(Expression left, OperatorComparer comparer, Expression right)
        {
            var mask = new List<OperatorComparer>{
            OperatorComparer.Contains,
            OperatorComparer.StartsWith,
            OperatorComparer.EndsWith
        };
            if (mask.Contains(comparer) && left.Type != typeof(string))
            {
                comparer = OperatorComparer.Equals;
            }
            if (!mask.Contains(comparer))
            {
                return Expression.MakeBinary((ExpressionType)comparer, left, Expression.Convert(right, left.Type));
            }
            return BuildStringCondition(left, comparer, right);
        }

        private static Expression BuildStringCondition(Expression left, OperatorComparer comparer, Expression right)
        {
            var compareMethod = typeof(string).GetMethods().Single(m => m.Name.Equals(Enum.GetName(typeof(OperatorComparer), comparer)) && m.GetParameters().Count() == 1);
            //we assume ignoreCase, so call ToLower on paramter and memberexpression
            var toLowerMethod = typeof(string).GetMethods().Single(m => m.Name.Equals("ToLower") && m.GetParameters().Count() == 0);
            left = Expression.Call(left, toLowerMethod);
            right = Expression.Call(right, toLowerMethod);
            return Expression.Call(left, compareMethod, right);
        }

        private static Expression MakeLambda(Expression parameter, Expression predicate)
        {
            var resultParameterVisitor = new ParameterVisitor();
            resultParameterVisitor.Visit(parameter);
            var resultParameter = resultParameterVisitor.Parameter;
            return Expression.Lambda(predicate, (ParameterExpression)resultParameter);
        }

        private class ParameterVisitor : ExpressionVisitor
        {
            public Expression Parameter
            {
                get;
                private set;
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                Parameter = node;
                return node;
            }
        }



    }


    public class ExpressionRebinder : ExpressionVisitor
    {
        /// <summary>
        /// The map
        /// </summary>
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionRebinder"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public ExpressionRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        /// <summary>
        /// Replacements the expression.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <param name="exp">The exp.</param>
        /// <returns>Returns replaced expression</returns>
        public static Expression ReplacementExpression(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ExpressionRebinder(map).Visit(exp);
        }

        /// <summary>
        /// Visits the <see cref="T:System.Linq.Expressions.ParameterExpression" />.
        /// </summary>
        /// <param name="node">The expression to visit.</param>
        /// <returns>
        /// The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.
        /// </returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            ParameterExpression replacement;
            if (this.map.TryGetValue(node, out replacement))
            {
                node = replacement;
            }

            return base.VisitParameter(node);
        }
    }


    public static class LambdaExtensions
    {
        /// <summary>
        /// Composes the specified left expression.
        /// </summary>
        /// <typeparam name="T">Param Type</typeparam>
        /// <param name="leftExpression">The left expression.</param>
        /// <param name="rightExpression">The right expression.</param>
        /// <param name="merge">The merge.</param>
        /// <returns>Returns the expression</returns>
        public static Expression<T> Compose<T>(this Expression<T> leftExpression, Expression<T> rightExpression, Func<Expression, Expression, Expression> merge)
        {
            var map = leftExpression.Parameters.Select((left, i) => new
            {
                left,
                right = rightExpression.Parameters[i]
            }).ToDictionary(p => p.right, p => p.left);

            var rightBody = ExpressionRebinder.ReplacementExpression(map, rightExpression.Body);

            return Expression.Lambda<T>(merge(leftExpression.Body, rightBody), leftExpression.Parameters);
        }

        /// <summary>
        /// Performs an "AND" operation
        /// </summary>
        /// <typeparam name="T">Param Type</typeparam>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Returns the expression</returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return left.Compose(right, Expression.And);
        }

        /// <summary>
        /// Performs an "OR" operation
        /// </summary>
        /// <typeparam name="T">Param Type</typeparam>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>Returns the expression</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            return left.Compose(right, Expression.Or);
        }
    }
}
