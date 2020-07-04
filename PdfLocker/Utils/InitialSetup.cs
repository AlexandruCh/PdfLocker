using Meridian.Helper;
using NLog;
using NLog.Targets;
using System.IO;

namespace Meridian.Utils
{
    public class InitialSetup
    {
        internal InitialSetup()
        {
            FileUtils.ClearTmpFolder();
            SetupLogger();
        }

        private void SetupLogger()
        {
            var target = (FileTarget)LogManager.Configuration.FindTargetByName("logfile");
            target.FileName = ConfigManager.LogPath;
            LogManager.ReconfigExistingLoggers();

            //new LogDepthAttributeConfiguration("logger");
        }
    }
}
