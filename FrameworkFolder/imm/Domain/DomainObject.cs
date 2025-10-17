using System;
using imm.Core;
using System.Xml.Serialization;

namespace imm.Domain
{
	public abstract class DomainObject:Observable
	{
		[XmlAttribute]
		public string Id;
		
		public DomainObject ()
		{
			this.Id = Guid.NewGuid().ToString();			
		}
	}
}

