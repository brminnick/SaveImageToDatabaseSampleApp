using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Crashes;
using Microsoft.Azure.Mobile.Analytics;

namespace SaveImageToDatabaseSampleApp
{
    public static class AnalyticsHelpers
    {
        public static void Start(string APIKey)
        {
            MobileCenter.Configure(APIKey);
            MobileCenter.Start(typeof(Analytics), typeof(Crashes));
        }

        public static void TrackEvent(string trackIdentifier, IDictionary<string, string> table = null) =>
            Analytics.TrackEvent(trackIdentifier, table);

        public static void Log(string tag, string message, Exception exception = null, MobileCenterLogType type = MobileCenterLogType.Warn)
        {
            switch (type)
            {
                case MobileCenterLogType.Info:
                    MobileCenterLog.Info(tag, message, exception);
                    break;
                case MobileCenterLogType.Warn:
                    MobileCenterLog.Warn(tag, message, exception);
                    break;
                case MobileCenterLogType.Error:
                    MobileCenterLog.Error(tag, message, exception);
                    break;
                case MobileCenterLogType.Assert:
                    MobileCenterLog.Assert(tag, message, exception);
                    break;
                case MobileCenterLogType.Verbose:
                    MobileCenterLog.Verbose(tag, message, exception);
                    break;
                case MobileCenterLogType.Debug:
                    MobileCenterLog.Debug(tag, message, exception);
                    break;
                default:
                    throw new Exception("MobileCenterLogType Does Not Exist");
            }
        }
    }

    public enum MobileCenterLogType
    {
        Assert, Debug, Error, Info, Verbose, Warn
    }
}
