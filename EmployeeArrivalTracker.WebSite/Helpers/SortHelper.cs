using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace EmployeeArrivalTracker.WebSite.Helpers
{
    public static class SortHelper
    {
        public static IQueryable<T> SortByProperty<T>(IQueryable<T> source, string propertyName, bool descending = false)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Property name must be specified.");
            }

            var entityType = typeof(T);
            var propertyInfo = entityType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Invalid property name: '{propertyName}'");
            }

            var parameter = Expression.Parameter(entityType, "e");
            var propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);

            if (descending)
            {
                return source.OrderByDescending(e => orderByExp);
            }
            else
            {
                return source.OrderBy(e => orderByExp);
            }
        }
    }
}
