using System;
using System.Collections.Generic;
using System.Net.Http;
using Windows.ApplicationModel.Email;
using Windows.UI.Popups;

namespace NetworkClient
{
    public abstract class BaseHttpClient : HttpClient
    {
        public string AppName { get; set; }

        public BaseHttpClient(string appName) : base()
        {
            AppName = appName;
        }

        public BaseHttpClient(string appName, string key, string value) : this(appName)
        {
            this.DefaultRequestHeaders.Add(key, value);
        }

        public BaseHttpClient(string appName, KeyValuePair<string, string> header) : this(appName, header.Key, header.Value) { }

        public BaseHttpClient(string appName, Dictionary<string, string> headers) : this(appName)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                this.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        protected async void ShowMessageDialog(string content)
        {
            MessageDialog messageDialog = new MessageDialog(content);
            await messageDialog.ShowAsync();
        }

        protected async void ShowErrorDialog(string content)
        {
            MessageDialog messageDialog = new MessageDialog(content, "Unexpected Error Occured On Server");
            messageDialog.Commands.Add(new UICommand("Report", async (action) =>
            {
                EmailMessage emailMsg = new EmailMessage();
                emailMsg.Subject = AppName + " Application Error";
                emailMsg.Body = content;
                emailMsg.To.Add(new EmailRecipient("bryantylai_app_dev@outlook.com"));

                await EmailManager.ShowComposeNewEmailAsync(emailMsg);
            }));
            messageDialog.Commands.Add(new UICommand("Cancel"));
            await messageDialog.ShowAsync();
        }
    }
}
