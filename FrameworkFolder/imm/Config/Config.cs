using System;
using imm.Data.Serialization;
using UnityEngine;
using System.IO;
using imm.Core;

namespace imm.Configs
{
	public abstract class Config<TConfig,TSerializationPolicy> :Observable
		where TSerializationPolicy:SerializationPolicy,new()
		where TConfig:Config<TConfig,TSerializationPolicy>
	{
		public const string CONFIGS_FOLDER = "Configs";
		
		private String _name;

		public Config (String name)
		{
			_name = name;
		}

        public Config()
        {
            
        }
		
		public virtual void OnRestored()
		{
			
		}
		
		public static TConfig Load(String name) 
		{
			TSerializationPolicy serializer = new TSerializationPolicy();
			
			string filepath = Path.Combine(Path.Combine(Application.persistentDataPath,CONFIGS_FOLDER), name);
			
			#if UNITY_EDITOR
				filepath = Path.Combine(Path.Combine(Path.Combine(Application.dataPath,"Development"), CONFIGS_FOLDER), name);
			#endif
			
			TConfig config = null;
			
			using(Stream stream = File.OpenRead(filepath))
			{
				config = serializer.Restore<TConfig>(stream);
			}
			
			config.OnRestored();
			
			return config;
		}

		public static TConfig Load(String name, String path, bool strongPath=false)
		{
			TSerializationPolicy serializer = new TSerializationPolicy();

			string filepath = !strongPath ? Path.Combine(Path.Combine(path, CONFIGS_FOLDER), name) : Path.Combine(path,name);

#if UNITY_EDITOR
			filepath = !strongPath ? Path.Combine(Path.Combine(Path.Combine(path, "Development"), CONFIGS_FOLDER), name) : Path.Combine(path, name);
#endif

			TConfig config = null;

			using (Stream stream = File.OpenRead(filepath))
			{
				config = serializer.Restore<TConfig>(stream);
			}

			config.OnRestored();

			return config;
		}

		public static TConfig LoadFromResources(String name,string folder="") 
		{
			TSerializationPolicy serializer = new TSerializationPolicy();
			
			string path = Path.Combine(CONFIGS_FOLDER, name);
			if (!string.IsNullOrEmpty(folder)) {
				path = Path.Combine(folder, name);
			}
			path = path.Replace(Path.GetExtension(path), "");
			Debug.Log("LoadFromResources: " + path);
			MonoLog.Log(MonoLogChannel.Core, "LoadFromResources: " +  path);
			
			UnityEngine.Object textAsset = Resources.Load(path);
			
			MonoLog.Log(MonoLogChannel.Core, "textAssets: " + textAsset);
			MonoLog.Log(MonoLogChannel.Core, "textAssets: " + textAsset.GetType());
			Debug.Log("textAssets: " + textAsset.GetType());
			TConfig config = serializer.RestoreFromString<TConfig>(((TextAsset)textAsset).text);
			
			config.OnRestored();
			
			return config;
		}

		public static TConfig LoadFromString(String text) 
		{
			TSerializationPolicy serializer = new TSerializationPolicy();

			TConfig config = serializer.RestoreFromString<TConfig>( text );
			
			config.OnRestored();
			
			return config;
		}
		public static string GetStringResourceTextfile(String name, string folder = "")
		{
			string path = Path.Combine(CONFIGS_FOLDER, name);
			if (!string.IsNullOrEmpty(folder))
			{
				path = Path.Combine(folder, name);
			}
			path = path.Replace(Path.GetExtension(path), "");

			TextAsset targetFile = Resources.Load<TextAsset>(path);

			return targetFile!=null ? targetFile.text : null;
		}

		public void Save(string name)
		{
			String configFolderPath = Path.Combine(Application.persistentDataPath,CONFIGS_FOLDER);
			
			#if UNITY_EDITOR
				configFolderPath = Path.Combine(Path.Combine(Application.dataPath,"Development"), CONFIGS_FOLDER);
			#endif
			
			if(!Directory.Exists(configFolderPath))
				Directory.CreateDirectory(configFolderPath);
			
			TSerializationPolicy serializer = new TSerializationPolicy();
			
			String configFilePath = Path.Combine(configFolderPath, name);
			
			MonoLog.Log(MonoLogChannel.Info,"Saving config " + configFilePath);
			
			using(Stream stream = File.Open(Path.Combine(configFolderPath, name), FileMode.Create,FileAccess.Write))
			{
				 serializer.Store<TConfig>((TConfig)this,stream);
			}
		}


		public void Save(string name, string path)
		{
			String configFolderPath = Path.Combine(path, CONFIGS_FOLDER);

#if UNITY_EDITOR
			configFolderPath = Path.Combine(Path.Combine(path, "Development"), CONFIGS_FOLDER);
#endif

			if (!Directory.Exists(configFolderPath))
				Directory.CreateDirectory(configFolderPath);

			TSerializationPolicy serializer = new TSerializationPolicy();

			String configFilePath = Path.Combine(configFolderPath, name);

			MonoLog.Log(MonoLogChannel.Info, "Saving config " + configFilePath);

			using (Stream stream = File.Open(Path.Combine(configFolderPath, name), FileMode.Create, FileAccess.Write))
			{
				serializer.Store<TConfig>((TConfig)this, stream);
			}
		}

		public void Save()
		{
			Save(_name);
		}
	}
}

