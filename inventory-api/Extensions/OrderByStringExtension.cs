using System.Linq.Expressions;
using System.Reflection;

namespace inventory_api.Extensions
{
    public static class OrderByStringExtension
    {
        public static IOrderedQueryable<TSource> OrderByString<TSource>(this IQueryable<TSource> query, PropertyInfo propertyInfo, bool sortDesc)
        {
            //var entityType = typeof(TSource);

            //Create x=>x.PropName
            //var propertyInfo = entityType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            //Om propname ikke eksisterer, bruk CreatedAt som filtrering
            //if (propertyInfo == null)
            //    propertyInfo = entityType.GetProperty("CreatedAt");

            var entityType = typeof(TSource);

            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyInfo.Name ?? "CreatedAt");
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });




            //Get System.Linq.Queryable.OrderBy() method.
            var enumarableType = typeof(Queryable);

            //Asc or Desc sorting
            var a = (sortDesc ? "OrderByDescending" : "OrderBy");

            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == (sortDesc ? "OrderByDescending" : "OrderBy") && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                 //Put more restriction here to ensure selecting the right overload                
                     return parameters.Count == 2;//overload that has 2 parameters
                 }).Single();


            //The linq's OrderBy<TSource, TKey> has two generic types, which provided here
            var genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            /*Call query.OrderBy(selector), with query and selector: x=> x.PropName
              Note that we pass the selector as Expression to the method and we don't compile it.
              By doing so EF can extract "order by" columns and generate SQL for it.*/

            var newQuery = genericMethod.Invoke(genericMethod, new object[] { query, selector }) as IOrderedQueryable<TSource>;

            return newQuery;

        }
    }

}
