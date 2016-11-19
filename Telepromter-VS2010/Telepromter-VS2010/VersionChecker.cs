using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Telepromter_VS2010
{
    class VersionChecker
    {
        static String Version = "1.2";
        static String checkUrl = "https://raw.githubusercontent.com/buttilloa/Teleprompter/master/LatestVersion.txt";
        public static void checkForLatest()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(checkCallback));

        }
        static void checkCallback(object a)
        {
            try
            {
                using (var client = new WebClient())
                    if (client.DownloadString(checkUrl) != Version) Game1.isLatest = false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error");
            }
        }
    }
}
