using System;
using System.Linq;
using System.Reflection;

namespace Core.Data
{
    /// <summary>
    /// Library for generating SQL Queries with better syntax checking and refactoring (Work in Progress)
    /// </summary>
    internal static class SqlGenerator
    {

        public static string Select(object parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var p = parameters.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var str = "SELECT ";
            for (var i = 0; i < p.Length; i++)
            {
                var o = p[i];
                str += $"{o.Name} '{o.GetValue(parameters, null)}' {(i == p.Length - 1 ? " " : ", ")}";
            }
            return str;
        }

        public static string OrderBy(this string str, object parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var p = parameters.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            str += "ORDER BY ";
            for (var i = 0; i < p.Length; i++)
            {
                var o = p[i];
                str += $"{o.Name} '{o.GetValue(parameters, null)}' {(i == p.Length - 1 ? " " : ", ")}";
            }
            return str;
        }

        public static string Limit(this string str, int limit, int offset)
        {
            str += $"LIMIT {offset}, {limit}";
            return str;
        }

        public static string Update(string tablename)
        {
            return $"UPDATE {tablename} ";
        }

        public static string From(this string str, string tablename, object joins = null)
        {
            if (tablename == null) throw new ArgumentNullException(nameof(tablename));
            str += $"FROM {tablename} ";
            if (joins == null) return str;
            var j = joins.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var o in j)
            {
                str += $"JOIN {o.Name} USING({o.GetValue(joins, null)}) ";
            }
            return str;
        }

        public static string Subquery(string query)
        {
            return $"({query})";
        }

        public static string SetIfNotNull(this string str, object parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var p = parameters.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            // Remove null parameters from set query if null not allowed
            p = p.Where(t => t.GetValue(p, null) != null).ToArray();
            if (!p.Any()) return str;
            str += "SET ";
            for (var i = 0; i < p.Length; i++)
            {
                var o = p[i];
                str += $"{o.Name} = {o.GetValue(parameters, null)} {(i == p.Length - 1 ? " " : ", ")}";
            }
            return str;
        }

        public static string Set(this string str, object parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var p = parameters.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            // Remove null parameters from set query if null not allowed
            if (!p.Any()) return str;
            str += "SET ";
            for (var i = 0; i < p.Length; i++)
            {
                var o = p[i];
                str += $"{o.Name} = {o.GetValue(parameters, null)} {(i == p.Length - 1 ? " " : ", ")}";
            }
            return str;
        }

        public static string Where(this string str, object conditions)
        {
            if (conditions == null) throw new ArgumentNullException(nameof(conditions));
            var c = conditions.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (!c.Any()) return str;

            for (var i = 0; i < c.Length; i++)
            {
                var o = c[i];
                str += $"{o.Name} = {o.GetValue(conditions, null)} {(i == c.Length - 1 ? " " : "AND ")}";
            }
            return str;
        }
    }
}
