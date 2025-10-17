
namespace imm.Domain
{
	public sealed class PostParametersWeb
	{
		public readonly string Link;
		public readonly string Title;
		public readonly string Description;
		public readonly string PhotoURL;

		public readonly Photo Photo;
		public readonly bool IsHtml;
				
		public PostParametersWeb(string title, Photo photo = null)
		{
			this.Title = title;
			this.Photo = photo;
			this.Description = string.Empty;
			this.IsHtml = true;
		}
		
		public PostParametersWeb(string title, string description, Photo photo = null)
		{
			this.Title = title;
			this.Photo = photo;
			this.Description = description;

			this.IsHtml = true;
		}	

		public PostParametersWeb(string title, string description, bool isHtml)
		{
			this.Title = title;
			this.Description = description;
			this.Photo = null;

			this.IsHtml = isHtml;
		}	

		public PostParametersWeb(string title, string description, string photoPath, string link)
		{
			this.Title = title;
			this.Description = description;
			this.Link = link;
			this.PhotoURL = photoPath;

			this.IsHtml = true;
		}	
	}
}

