using System;
using UnityEngine;

public static class VectorUtils
{
	static public Vector2 ToVector2(this Vector3 vector)
	{
		return new Vector2(vector.x, vector.y);
	}

	static public Vector3 ToVector3(this Vector2 vector)
	{
		return new Vector3(vector.x, vector.y, 0);
	}

	static public Boolean EqualsByVector2(this Vector2 vector, Vector2 anotherVector)
	{
		return vector.x.Equals(anotherVector.x) && vector.y.Equals(anotherVector.y);
	}

	static public Boolean EqualsByVector2(this Vector3 vector, Vector2 anotherVector)
	{
		return vector.x.Equals(anotherVector.x) && vector.y.Equals(anotherVector.y);
	}

	static public Vector2 Project2 (Vector2 vector, Vector2 onNormal, out float num)
	{
		num = Vector2.Dot (onNormal, onNormal);
		if (num < 1.401298E-45f)
		{
			return Vector2.zero;
		}
		num = Vector2.Dot (vector, onNormal) / num;
		return onNormal * num;
	}
}