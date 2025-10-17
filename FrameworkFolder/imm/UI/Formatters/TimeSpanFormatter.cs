using System;

namespace imm.UI.Formatters
{
	public static class TimeSpanFormatter
	{		
		
		public static int MonthDifference(this DateTime lValue, DateTime rValue)
		{
		    return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
		}		
		
		public static string Format(TimeSpan timeSpan)
		{
			if(timeSpan.Days > 0)
			{
				return string.Format("{0:D1} DAY{4} {1:D2}:{2:D2}:{3:D2}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, 
					timeSpan.Seconds, timeSpan.Days > 1 ? "S" : string.Empty);
			}
			else
			{
				return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);				
			}
		}
	}
}

