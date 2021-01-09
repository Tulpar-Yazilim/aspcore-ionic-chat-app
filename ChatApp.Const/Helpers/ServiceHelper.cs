using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.Const
{
    public static class ServiceHelper
    {
        public static DateTime CalculateDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(-1);
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(-2);
            }
            else if (date.Month == 2 && date.Day == 29 || date.Day == 30)
            {
                date = new DateTime(date.Year, date.Month, 28, date.Hour, date.Minute, date.Second, date.Millisecond);
            }

            return date;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> known = new HashSet<TKey>();
            return source.Where(element => known.Add(keySelector(element)));
        }

        public static string MoneyFormat(decimal num)
        {

            if (num == 0)
            {
                return num.ToString("0");
            }

            if (num >= Convert.ToDecimal(100000000))
            {
                return (num / Convert.ToDecimal(1000000)).ToString("#,0Mr");
            }

            if (num >= Convert.ToDecimal(10000000))
            {
                return (num / Convert.ToDecimal(1000000)).ToString("0.#") + "M";
            }

            if (num >= Convert.ToDecimal(100000))
            {
                return (num / Convert.ToDecimal(1000)).ToString("#,0B");
            }

            if (num >= Convert.ToDecimal(10000))
            {
                return (num / Convert.ToDecimal(1000)).ToString("0.#") + "B";
            }

            return num.ToString("#.##");
        }
    }

}
