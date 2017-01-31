using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamTrack.SystemServices
{
    /// <summary>
    /// An interface to allow a common for the Business logic of the app to deal with files.
    /// </summary>
    public interface IFileService
    {
        void WriteFile(string fileName, string content);
        string ReadFile(string fileName);
    }
}
