using System;
using System.Collections.Generic;
using UnityEngine;

namespace imm.Core
{
	public enum MonoLogChannel
	{
		Core,
		Debug,
		Info,
		Error,
		All,
		Sound
	}
	
	
	public static class MonoLog
	{
		public static List<MonoLogChannel> Channels
		{
			get;
			private set;
		}
		
		static MonoLog()
		{
			Channels = new List<MonoLogChannel>();
			
			Channels.Add(MonoLogChannel.Error);
			Channels.Add(MonoLogChannel.Info);
			//Channels.Add(MonoLogChannel.Debug);
			//Channels.Add(MonoLogChannel.Core);
		}
		
		public static void Log(MonoLogChannel channel, object message)
		{			
			if (Channels.Contains(channel) || Channels.Contains(MonoLogChannel.All))
			{
				string details = "[" + System.DateTime.UtcNow.ToString("HH:mm:ss.fff") + "] imm [" + channel.ToString() + "]: " + message.ToString();
								
				Debug.Log(details);								
			}
		}
		
		public static void LogWarning(MonoLogChannel channel, object message)
		{
			if (Channels.Contains(channel) || Channels.Contains(MonoLogChannel.All))
			{
				Debug.LogWarning("BILLIARD [" + channel.ToString() + "]: " + message.ToString());	
			}
		}
		
		public static void Log(MonoLogChannel channel, object message, Exception exception)
		{
			if (Channels.Contains(channel) || Channels.Contains(MonoLogChannel.Error) || Channels.Contains(MonoLogChannel.All))
			{
				Debug.LogException(new Exception("BILLIARD [" + channel.ToString() + "]: " + message.ToString(), exception));
			}
		}
	}
}

