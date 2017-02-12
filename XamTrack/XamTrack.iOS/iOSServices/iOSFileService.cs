using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XamTrack.SystemServices;

namespace XamTrack.iOS.iOSServices
{
	public class iOSFileService : IFileService
	{
		#region IFileService Members

		/// <summary>
		/// Reads in the file with the matching name.
		/// </summary>
		/// <param name="fileName">The name of the file to read</param>
		/// <returns>The contents of the file or an emptry string if no file is found.</returns>
		public string ReadFile(string fileName)
		{
			var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var libPath = Path.Combine(docsPath, "..", "Library");
			string file = Path.Combine(libPath, fileName);


			try
			{
				using (var streamReader = new StreamReader(file))
				{
					string content = streamReader.ReadToEnd();
					return content;
				}
			}
			catch (FileNotFoundException fnfEx)
			{
				return "";
			}
		}

		/// <summary>
		/// Writes the passed in contents to the file passed in.
		/// Overwrites any existing files.
		/// </summary>
		/// <param name="fileName">The filename to write to.</param>
		/// <param name="content">The string payload of the file.</param>
		public void WriteFile(string fileName, string content)
		{
			var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var libPath = Path.Combine(docsPath, "..", "Library");
			string file = Path.Combine(libPath, fileName);
			File.Delete(file);

			using (var streamWriter = new StreamWriter(file, true))
			{
				streamWriter.Write(content);
				streamWriter.Flush();
				streamWriter.Close();
			}
		}

		#endregion
	}
}