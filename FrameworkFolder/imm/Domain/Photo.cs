using System.Xml.Serialization;
using System.IO;
using UnityEngine;

namespace imm.Domain
{
	public sealed class Photo: DomainObject
	{
		[XmlAttribute]
		public string FileName;
		
		//[XmlAttribute]
		//public bool IsSavedToCameraRoll = false;
		
		[XmlAttribute]
		public System.DateTime CreatedDate;
		
		[XmlIgnore]
		public bool IsTemp = false;

        public static string GetFilePath(Photo photo)
        {
            return Path.Combine(Application.dataPath, photo.FileName);
        }
	}
}

