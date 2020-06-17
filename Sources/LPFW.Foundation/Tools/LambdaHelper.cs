using LPFW.Foundation.SpecificationsForEntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace LPFW.Foundation.Tools
{
    /// <summary>
    /// 针对实现接口 IEntityBase 的实体模型的 Lambda 表达式的一些辅助方法
    /// </summary>
    public static class LambdaHelper
    {
        public static Expression<Func<T, TKey>> GetPropertyExpression<T, TKey>(string propertyName) where T : class, IEntityBase, new()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            return Expression.Lambda<Func<T, TKey>>(Expression.Convert(Expression.Property(parameter, propertyName), typeof(object)), parameter);
        }

        public static Expression<Func<T, bool>> GetConditionExpression<T>(string keyword) where T : class, IEntityBase, new()
        {
            Expression<Func<T, bool>> expression = _GetContains<T>("Name", keyword);
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var item in properties)
            {
                var itemTypeName = item.PropertyType.Name;
                if (itemTypeName.ToLower() == "string" && item.Name != "Name")
                {
                    expression = expression.Or(_GetContains<T>(item.Name, keyword));
                }
            }
            return expression;
        }

        public static Expression<Func<T, bool>> GetConditionExpression<T>(string keyword, string[] names) where T : class, IEntityBase, new()
        {
            Expression<Func<T, bool>> expression = _GetContains<T>("Name", keyword);
            foreach (var item in names)
            {
                if (item != "Name")
                {
                    expression = expression.Or(_GetContains<T>(item, keyword));
                }
            }
            return expression;
        }


        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        /// <summary>
        /// Lambda表达式拼接处理的扩展方法       
        /// /// </summary>        
        /// /// <typeparam name="T"></typeparam>        
        /// /// <param name="first"></param>        
        /// /// <param name="second"></param>        
        /// /// <param name="merge">拼接的方式 And/Or </param>        
        /// /// <returns></returns>        
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            // 创建参数映射器字典 (待拼接的表达式放在字典元素的第一个位置)            
            var map = first.Parameters.Select((f, i) => new { f, s = second.Parameters[i] }).ToDictionary(p => p.s, p => p.f);
            // 使用字典元素重新绑定           
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> property)
        {
            var propertyInfo = (property.Body as MemberExpression).Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("指向了非法属性。");
            }
            return propertyInfo.Name;
        }

        public static Expression<Func<T, bool>> GetEqual<T>(string propertyName, string propertyValue)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("==", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(Guid));

            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }

        /// <summary>
        /// 创建lambda表达式：x=>x.propertyName.Contains(propertyValue)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> _GetContains<T>(string propertyName, string propertyValue) where T : class, IEntityBase, new()
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            MemberExpression member = Expression.PropertyOrField(parameter, propertyName);
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            ConstantExpression constant = Expression.Constant(propertyValue, typeof(string));

            return Expression.Lambda<Func<T, bool>>(Expression.Call(member, method, constant), parameter);
        }
    }


    public class SwapVisitor : ExpressionVisitor
    {
        private readonly Expression from, to;
        public SwapVisitor(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }
        public override Expression Visit(Expression node)
        {
            return node == from ? to : base.Visit(node);
        }
    }

    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;
        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }
        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement))
            {
                p = replacement;
            }
            return base.VisitParameter(p);
        }
    }


}


