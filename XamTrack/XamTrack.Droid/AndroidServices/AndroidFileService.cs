using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XamTrack.SystemServices;

namespace XamTrack.Droid.AndroidServices
{
    public class AndroidFileService : IFileService
    {
        #region IFileService Members

        /// <summary>
        /// Reads in the file with the matching name.
        /// </summary>
        /// <param name="fileName">The name of the file to read</param>
        /// <returns>The contents of the file or an emptry string if no file is found.</returns>
        public string ReadFile(string fileName)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string file = Path.Combine(path, fileName);
            

            try
            {
                using (var streamReader = new StreamReader(file))
                {
                    string content = streamReader.ReadToEnd();
                    return content;
                }
            }
            catch(FileNotFoundException fnfEx)
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
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string file = Path.Combine(path, fileName);
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