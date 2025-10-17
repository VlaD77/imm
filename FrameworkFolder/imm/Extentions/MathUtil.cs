using System;
using UnityEngine;

public static class MathUtil
{
	static public int sign(double value)
	{
		return (value >= 0)? 1 : -1;
	}

	static public int random(int min, int max)
	{
		return UnityEngine.Random.Range(min, max+1);
	}

	static public float random(float min, float max)
	{
		return UnityEngine.Random.Range(min, max+.0001f);
	}

	static public float random(float min, float max, System.Random random)
	{
		return (float)(min + random.NextDouble()*(max - min));
	}

	static public int randomWithProbabilityTable(System.Random random, int min, int max, int[] probabilityTable)
	{
		int sumProbabilities = 0;
		int[] probabilities = new int[max+1];
		for (int i = min; i <= max; i++)
		{
			sumProbabilities += probabilityTable[i];
			probabilities[i] += sumProbabilities;
		}

		int result = random.Next(0, sumProbabilities+1);
		for (int i = min; i <= max; i++)
		{
			if (result < probabilities[i])
				return i;
		}

		return min;
	}

	static public int randomWithProbabilityTable(int[] probabilityTable)
	{
		int sumProbabilities = 0;
		int[] probabilities = new int[probabilityTable.Length];
		for (int i = 0; i < probabilityTable.Length; i++)
		{
			sumProbabilities += probabilityTable[i];
			probabilities[i] += sumProbabilities;
		}

		int result = random(0, sumProbabilities+1);
		for (int i = 0; i < probabilityTable.Length; i++)
		{
			if (result < probabilities[i])
				return i;
		}
		
		return 0;
	}

	static public int randomWithProbabilityTable(System.Random random, int[] probabilityTable)
	{
		int sumProbabilities = 0;
		int[] probabilities = new int[probabilityTable.Length];
		for (int i = 0; i < probabilityTable.Length; i++)
		{
			sumProbabilities += probabilityTable[i];
			probabilities[i] += sumProbabilities;
		}

		int result = random.Next(0, sumProbabilities+1);
		for (int i = 0; i < probabilityTable.Length; i++)
		{
			if (result < probabilities[i])
				return i;
		}
		
		return 0;
	}

	static public string IntToHex(uint crc)
	{
		return string.Format("{0:X}", crc);
	}

	static public uint HexToInt(string crc)
	{
		return uint.Parse(crc, System.Globalization.NumberStyles.AllowHexSpecifier);
	}
}