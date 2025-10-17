
using System;
using System.Collections.Generic;

public static class ArrayUtils
{
	static public void RemoveAt<T>(ref T[] array, int index)
	{
		for(int j = index; j < array.Length - 1 ; j++)
		{
			array[j] = array[j+1];
		}
		Array.Resize(ref array, array.Length-1);
	}

	static public int IndexOf(this Array array, object value)
	{
		return Array.IndexOf(array, value);
	}

	public static T[] Shuffle<T>( this T[] instance )
	{
		System.Random r = new Random();

		for ( int i = 0 ; i < instance.Length ; ++i )
		{
			int j = r.Next( i, instance.Length );

			T x = instance[j] ;
			instance[j] = instance[i] ;
			instance[i] = x ;			
		}

		return instance;
	}

	public static T[] Shuffle<T>( this T[] instance, int randomSeed )
	{
		System.Random r = new Random(randomSeed);
		
		for ( int i = 0 ; i < instance.Length ; ++i )
		{
			int j = r.Next( i, instance.Length );
			
			T x = instance[j] ;
			instance[j] = instance[i] ;
			instance[i] = x ;			
		}
		
		return instance;
	}

	public static T GetRandom<T>( this T[] instance )
	{
		return (T)instance.GetValue(MathUtil.random(0, instance.Length-1));
	}

	public static T[] SubArray<T>(this T[] instance, int startIndex, int endIndex)
	{
		List<T> result = new List<T>();
		for (int i = startIndex; i <= endIndex; i++)
		{
			result.Add(instance[i]);
		}
		return result.ToArray();
	}

	public static T[] RemoveAt<T>(this T[] instance, int index)
	{
		List<T> result = new List<T>(instance);
		result.RemoveAt(index);
		return result.ToArray();
	}
}