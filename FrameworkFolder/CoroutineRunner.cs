using System;
using System.Collections;
using imm.Core;
using UnityEngine;

namespace imm
{
	public class CoroutineRunner : imm.Core.MonoSingleton<CoroutineRunner>
	{
		public static Coroutine Run(IEnumerator coroutine)
		{
			return Instance.StartCoroutine(coroutine);
		}

		public static Coroutine Wait(Single seconds, Action callback)
		{
			return Instance.StartCoroutine(WaitEnumerator(seconds, callback));
		}

		public static Coroutine EveryFrame(Single totalTime, Action<Single> callback, Action onFinish = null)
		{
			return Instance.StartCoroutine(EveryFrameEnumerator(totalTime, callback, onFinish));
		}

		private static IEnumerator WaitEnumerator(Single seconds, Action callback)
		{
			yield return new WaitForSeconds(seconds);

			callback();
		}

		private static IEnumerator EveryFrameEnumerator(Single totalTime, Action<Single> callback, Action onFinish = null)
		{
			Single time = totalTime;

			while (time >= 0)
			{
				yield return new WaitForEndOfFrame();

				time -= Time.smoothDeltaTime;

				callback( 1 - Mathf.Clamp01( time / totalTime ) );
			}

			if (onFinish != null)
				onFinish();
		}
	}
}