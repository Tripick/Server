using System;
using System.IO;

namespace TripickServer.Utils
{
    public static class Images
    {
        public static string ImageToBase64(string path)
        {
            return Convert.ToBase64String(File.ReadAllBytes(path));
        }
    }
}
