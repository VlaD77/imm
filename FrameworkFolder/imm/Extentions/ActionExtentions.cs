
public static class ActionExtentions
{
	public static void SafeInvoke(this System.Action invocationTarget)
	{
		if (null != invocationTarget)
		{
			invocationTarget.Invoke();
		}
	}

    public static void SafeInvoke<T>(this System.Action<T> invocationTarget, T arg)
	{
		if (null != invocationTarget)
		{
			invocationTarget.Invoke(arg);
		}
	}

    public static void SafeInvoke<T1, T2>(this System.Action<T1, T2> invocationTarget, T1 arg1, T2 arg2)
	{
		if (null != invocationTarget)
		{
			invocationTarget.Invoke(arg1, arg2);
		}
	}

    public static void SafeInvoke<T1, T2, T3>(this System.Action<T1, T2, T3> invocationTarget, T1 arg1, T2 arg2, T3 arg3)
	{
		if (null != invocationTarget)
		{
			invocationTarget.Invoke(arg1, arg2, arg3);
		}
	}
}
