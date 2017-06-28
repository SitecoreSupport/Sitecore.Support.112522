using Sitecore.Diagnostics;
using Sitecore.EDS.Core.Dispatch;
using Sitecore.ExM.Framework.Diagnostics;
using Sitecore.ExM.Framework.Distributed.Tasks.TaskPools.ShortRunning;
using Sitecore.Modules.EmailCampaign.Core.Dispatch;
using Sitecore.Modules.EmailCampaign.Core.Pipelines;

namespace Sitecore.Support.EmailCampaign.Cm.Pipelines.SendEmail
{
    public class CreateTask
    {
        // Fields
        private readonly ILogger _logger;
        private readonly ShortRunningTaskPool _taskPool;

        // Methods
        public CreateTask(ShortRunningTaskPool taskPool, ILogger logger)
        {
            Assert.ArgumentNotNull(taskPool, "taskPool");
            Assert.ArgumentNotNull(logger, "logger");
            this._taskPool = taskPool;
            this._logger = logger;
        }

        public void Process(SendMessageArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            EmailMessage message = args.CustomData["EmailMessage"] as EmailMessage;
            if (message == null)
            {
                this._logger.LogDebug("Failed to create sent message task as messageItem is null");
            }
            else if (args.Task is TestMessageTask)
            {
                this._logger.LogDebug("Sitecore Support patch applied for Test Dispatch: Bug:112522");
            }
            else
            {
                string str = message.MessageId;
                ShortRunningTask task = new ShortRunningTask(null);
                task.Data.SetAs<string>("message_id", message.MessageId);
                task.Data.SetAs<string>("instance_id", str);
                task.Data.SetAs<string>("contact_id", message.ContactId);
                this._taskPool.AddTasks(new ShortRunningTask[] { task });
            }
        }
    }


}