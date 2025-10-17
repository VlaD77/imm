using System;
using System.IO;
using imm.Data.Serialization;
using UnityEngine;

namespace imm.Data.Serialization
{
	public sealed class JsonSerializationPolicy:SerializationPolicy
	{
		public JsonSerializationPolicy ()
		{
		}
		
		public override T Restore<T> (System.IO.Stream stream)
		{
			using(StreamReader streamReader = new StreamReader(stream))
			{
				return JsonUtility.FromJson<T>(streamReader.ReadToEnd());
			}
		}

		public override string StoreToString<T>(T item)
		{
			return JsonUtility.ToJson(item);
		}

		public override T RestoreFromString<T>(string json)
		{
			return JsonUtility.FromJson<T>(json);
		}

		public override void Store<T> (T item, Stream stream)
		{
			using(TextWriter textWriter = new StreamWriter(stream))
			{
				textWriter.Write(JsonUtility.ToJson(item));
			}
		}
		
		public override string FileExtention 
		{
			get 
			{
				return ".json";
			}
		}
	}
}

