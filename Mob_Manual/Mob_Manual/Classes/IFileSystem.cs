using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mob_Manual.Classes
{
    public interface IFileSystem
    {
        Task<string> GetFile(string path);

        Task WriteTextAsync(string fileName, string text);
    }
}
