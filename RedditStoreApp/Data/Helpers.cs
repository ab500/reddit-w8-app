using GalaSoft.MvvmLight.Messaging;
using RedditStoreApp.Data.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace RedditStoreApp.Data
{
    public class Helpers
    {
        private enum FailureType { NoFail, Permanent, Temporary };

        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static void DebugWrite(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public static async Task<T> EnsureCompletion<T>(Func<Task<T>> apiCall) where T: class
        {
            T result = null;
            FailureType failureMode = FailureType.NoFail;

            do
            {
                failureMode = FailureType.NoFail;

                try
                {
                    result = await apiCall();
                }
                catch (RedditApiException e)
                {
                    failureMode = e.ExceptionType == RedditApiExceptionType.Connection ?
                        FailureType.Temporary : FailureType.Permanent;
                }

                if (failureMode == FailureType.Permanent)
                {
                    var msg = new MessageDialog("A Permanent error has occurred. Please try again later.");
                    await msg.ShowAsync();
                    return null;
                }

                if (failureMode == FailureType.Temporary)
                {
                    var msg = new MessageDialog("Request failed. Would you like to retry?");
                    msg.Commands.Add(new UICommand("Retry"));
                    msg.Commands.Add(new UICommand("Cancel"));

                    IUICommand msgResult = await msg.ShowAsync();

                    if (msgResult.Label == "Cancel")
                    {
                        return null;
                    }
                }
            } 
            while (failureMode != FailureType.NoFail);

            return result;
        }
    }
}
