using System;
using Mob_Manual.iOS;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Mob_Manual.Classes;
using Xamarin.Forms;

[assembly: Dependency(typeof(Mob_Manual.iOS.FileSystem))]

namespace Mob_Manual.iOS
{
    public class FileSystem : IFileSystem
    {
        async Task<string> IFileSystem.GetFile(string path)
        {
            var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var pathFile = Path.Combine(docsPath, path);

            byte[] result;

            using (FileStream SourceStream = File.Open(pathFile, FileMode.Open))
            {
                result = new byte[SourceStream.Length];
                await SourceStream.ReadAsync(result, 0, (int)SourceStream.Length);
            }

            return Encoding.ASCII.GetString(result);
        }

        async Task IFileSystem.WriteTextAsync(string fileName, string text)
        {
            var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(docsPath,fileName);

            using (var writer = File.CreateText(path))
            {
                await writer.WriteAsync(text);
            }
        }
    }
}
