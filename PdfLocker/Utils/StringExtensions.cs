using System;
using System.IO;

namespace PdfLocker.Utils
{
    public static class StringExtensions
    {
        public static string MakeUnique(this string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                Guid.NewGuid(),
                Path.GetExtension(fileName)
                );
        }
    }
}
