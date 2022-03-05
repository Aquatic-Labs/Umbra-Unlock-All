using System;
using System.IO;
using Octokit;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace UmbraInjector
{
    static class Program
    {

        public static bool updateAvailable;
        public static string latestVersion;
        public static bool devBuild;
        public static bool upToDate = true;
        public static bool rateLimited = false;

        public const string
            VERSION = "1.0.0";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new MainForm());
        }

        public static async void CheckForUpdate()
        {

            var client = new GitHubClient(new ProductHeaderValue("UmbraUnlockUpdateCheck"));

            try
            {
                var releases = await client.Repository.Release.GetAll("Acher0ns", "Umbra-Unlock-All").ConfigureAwait(false);
                var latest = releases[0];
                latestVersion = latest.TagName;

                string[] versionSplit = VERSION.Split('.');
                string[] latestVersionSplit = latestVersion.Split('.');

                for (int i = 0; i < versionSplit.Length; i++)
                {
                    int versionNumber = int.Parse(versionSplit[i]);
                    int latestVersionNumber = int.Parse(latestVersionSplit[i]);
                    if (versionNumber < latestVersionNumber)
                    {
                        upToDate = false;
                        devBuild = false;
                        updateAvailable = true;
                        break;
                    }
                    else if (versionNumber > latestVersionNumber)
                    {
                        upToDate = false;
                        devBuild = true;
                        updateAvailable = false;
                        break;
                    }
                }
            }
            catch (RateLimitExceededException)
            {
                rateLimited = true;
                upToDate = true;
            }
        }
    }
}
