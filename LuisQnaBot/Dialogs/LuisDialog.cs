using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.LuisQnaBot
{
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        public LuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"],
            ConfigurationManager.AppSettings["LuisAPIKey"],
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Greeting" with the name of your newly created intent in the following handler
        [LuisIntent("GA")]
        public async Task GAIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"GA Intent");
            await context.Forward(new GADialog(), ResumeAfterQnA, context.Activity, CancellationToken.None);
        }

        [LuisIntent("HR")]
        public async Task HRIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"HR Intent");
            await context.Forward(new HRDialog(), ResumeAfterQnA, context.Activity, CancellationToken.None);
        }
        private async Task ShowLuisResult(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }
        private async Task ResumeAfterQnA(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Done<object>(null);
        }
    }
}