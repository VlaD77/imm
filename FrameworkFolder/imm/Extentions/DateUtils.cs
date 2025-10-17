using System;

public static class DateUtils
{
	static public string ToHHmmssFFF(this DateTime time)
	{
		return time.ToString("HH:mm:ss.FFF");
	}

	static public string ToHHmmss(this DateTime time)
	{
		return time.ToString("HH:mm:ss");
	}

	static public string Tommss(this DateTime time)
	{
		return time.ToString("mm:ss");
	}

	static public string ToAHAHmmss(this DateTime time)
	{
		int totalHours = (int)new TimeSpan(time.Ticks).TotalHours;
		return string.Format("{0}:{1:mm:ss}", totalHours.ToString("D2"), time);
	}

	static public DateTime Min(DateTime date1, DateTime date2)
	{
		return (date1.Ticks <= date2.Ticks)? date1 : date2;
	}

	static public DateTime Max(DateTime date1, DateTime date2)
	{
		return (date1.Ticks >= date2.Ticks)? date1 : date2;
	}

	public static DateTime SubstractPositive (DateTime date1, DateTime date2)
	{
		return (date1.Ticks > date2.Ticks)? new DateTime(date1.Subtract(date2).Ticks) : new DateTime(0);
	}

	public static DateTime FirstDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
	{
		DateTime firstDayInWeek = dayInWeek.Date;
		while (firstDayInWeek.DayOfWeek != firstDay)
		{
			firstDayInWeek = firstDayInWeek.AddDays(-1);
		}
        return firstDayInWeek;
	}
	
	public static DateTime LastDateOfWeek(DateTime dayInWeek, DayOfWeek firstDay)
	{
		DateTime dt = FirstDateOfWeek(dayInWeek, firstDay).AddDays( 6 );
		return new DateTime( dt.Year, dt.Month, dt.Day, 23, 59, 59, 999 );
    }

	public static DateTime GetTimeForEndWeek(DateTime date)
	{
		DateTime startTime = date;
		DateTime time = startTime;
		
		int countDaysToEndOfFriday = (DayOfWeek.Friday - time.DayOfWeek + 1);
		
		time = time.AddDays(countDaysToEndOfFriday);
		time = time.Subtract(new TimeSpan(time.Hour, time.Minute, time.Second));
		
		if (startTime >= time)
		{
			time.AddDays(7);
		}
		return time;
	}
}