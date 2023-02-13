using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace BeXCool.Process
{
    public delegate void MessageTransfer(string id, object data);

    public class MessageListener
    {
        /// <summary>
        /// ID of the packet sender.
        /// </summary>
        public string ID { get; private set; } = string.Empty;

        /// <summary>
        /// Where to listen for packets.
        /// </summary>
        public static string DataCacheLocation { get; set; } = Path.GetTempPath();

        /// <summary>
        /// Timer for checking for packets.
        /// </summary>
        private Timer ListeningTimer = new();

        /// <summary>
        /// Event for receiving messages.
        /// </summary>
        public event MessageTransfer? MessageReceived;

        public MessageListener() { }

        /// <summary>
        /// Listen to a specific ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>New instance of the MessageListener.</returns>
        public static MessageListener Listen(string id, bool manualCheck = false)
        {
            var ml = new MessageListener();
            ml.ID = id;

            if (!manualCheck) ml.InitializeTimer();
            
            return ml;
        }

        /// <summary>
        /// Stop listening.
        /// </summary>
        public void Stop()
        {
            ListeningTimer.Stop();
        }

        /// <summary>
        /// Initialize listening to an ID.
        /// </summary>
        private void InitializeTimer()
        {
            ListeningTimer.Interval = 100;
            ListeningTimer.AutoReset = true;
            ListeningTimer.Elapsed += (o, e) =>
            {
                CheckForMessage();
            };

            ListeningTimer.Start();
        }

        /// <summary>
        /// Force checking for any received messages.
        /// </summary>
        public void ForceCheck()
        {
            CheckForMessage();
        }

        private void CheckForMessage()
        {
            var dataPath = $@"{DataCacheLocation}\bcpc-{ID}";

            if (File.Exists(dataPath))
            {
                var data = File.ReadAllText(dataPath);

                File.Delete(dataPath);

                var mm = JsonConvert.DeserializeObject<MessageModel>(data);

                if (mm != null) OnMessageReceived(mm);
            }
        }

        protected virtual void OnMessageReceived(MessageModel data)
        {
            MessageReceived?.Invoke(ID, data);
        }
    }
}
