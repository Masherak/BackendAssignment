using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Extensions
{
	public static class DateTimeExtension
	{
		public static string ToApiFormattedString(this DateTime date)
		{
			return $"{date.Day}.{date.Month}.{date.Year}";
		}
	}
}
