using System.IO;

namespace Meridian.Helper
{
    public static class ConfigManager
    {
        private static XmlUtils XmlUtil;
        static ConfigManager()
        {
            var configPath = Path.Combine(FileUtils.GetExeFolderPath(), "Config.xml");
            XmlUtil = new XmlUtils(configPath);
        }

        public static string RootFolderName =>
            XmlUtil.GetElementVal("RootFolderName");

        public static string LogPath
            => XmlUtil.GetElementVal("PdfLogger");
    }
}
