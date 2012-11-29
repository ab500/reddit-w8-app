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
            return epoch.AddSeconds(unixTime).ToLocalTime();
        }

        public static void DebugWrite(string msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
        }

        public static async Task EnsureCompletion(Func<Task> task)
        {
            // We use interger as a dumby type for now...
            var result = await EnsureCompletion<int>(async () => {
                await task();
                return 0;
            });
        }

        public static async Task<T> EnsureCompletion<T>(Func<Task<T>> apiCall, string customErrorMessage = null)
        {
            T result = default(T);
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
                    string errorMessage = customErrorMessage == null ? 
                        (string)App.Current.Resources["Error_Permanent"] : customErrorMessage;
                    var msg = new MessageDialog(errorMessage);
                    await msg.ShowAsync();
                    return default(T);
                }

                if (failureMode == FailureType.Temporary)
                {
                    var msg = new MessageDialog((string)App.Current.Resources["Error_Temporary"]);
                    string cancelWord = (string)App.Current.Resources["Cancel"];
                    msg.Commands.Add(new UICommand((string)App.Current.Resources["Retry"]));
                    msg.Commands.Add(new UICommand(cancelWord));

                    IUICommand msgResult = await msg.ShowAsync();

                    if (msgResult.Label == cancelWord)
                    {
                        return default(T);
                    }
                }
            } 
            while (failureMode != FailureType.NoFail);

            return result;
        }
    }
}
