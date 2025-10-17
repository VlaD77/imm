using System;
using System.Text.RegularExpressions;

public static class StringUtil
{
	public static string[] Split(string strValue, string splitValue)
	{
		return strValue.Split(new string[]{splitValue}, StringSplitOptions.None);
	}

	public static string SplitStringByMaxChars(string strValue, int maxChars)
	{
		const string splittedChars = "...";
		maxChars += splittedChars.Length;

		if ( strValue.Length+splittedChars.Length > maxChars )
		{
			strValue = strValue.Substring(0, maxChars-splittedChars.Length )+splittedChars;
		}
		return strValue;
	}

	public static string SplitStringByStartEnd (string strValue, string start, string end)
	{
		if (strValue.Contains(start))
		{
			string[] array = strValue.Split(new string[]{start}, StringSplitOptions.None);
			strValue = array[array.Length-1];
		}

		if (strValue.Contains(end))
		{
			strValue = strValue.Split(new string[]{end}, StringSplitOptions.None)[0];
		}

		return strValue;
	}

	public static string SplitOnCapitals(string text)
	{
		Regex r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
		
		return r.Replace(text, " ");
	}
}