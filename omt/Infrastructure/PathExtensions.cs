using System.IO;

namespace omt.Infrastructure
{
	public static class PathExtensions
	{
		public static string FileName(this string path) 
		{
			return Path.GetFileName(path);
		}

		public static string Whack(this string parentPath, string childPath)
		{
			return Path.Combine(parentPath, childPath);
		}
	}
}

