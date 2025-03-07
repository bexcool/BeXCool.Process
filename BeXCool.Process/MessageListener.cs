using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace BeXCool.Process
{
    public delegate void MessageTransfer(string id, MessageModel message);

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
        /// <param name="id">ID to listen to.</param>
        /// <returns>New instance of the MessageListener.</returns>
        public static MessageListener Listen(string id)
        {
            var ml = new MessageListener();
            ml.ID = id;

            ml.InitializeTimer();

            return ml;
        }

        /// <summary>
        /// Listen to a specific ID.
        /// </summary>
        /// <param name="id">ID to listen to.</param>
        /// <param name="manualCheck">Don't use timer for checking for messages.</param>
        /// <returns>New instance of the MessageListener.</returns>
        public static MessageListener Listen(string id, bool manualCheck)
        {
            var ml = new MessageListener();
            ml.ID = id;

            if (!manualCheck) ml.InitializeTimer();

            return ml;
        }

        /// <summary>
        /// Listen to a specific ID.
        /// </summary>
        /// <param name="id">ID to listen to.</param>
        /// <param name="dispatcher">Message transfer event dispatcher.</param>
        /// <returns></returns>
        public static MessageListener Listen(string id, MessageTransfer dispatcher)
        {
            var ml = new MessageListener();
            ml.ID = id;

            if (dispatcher != null) ml.InitializeDispatcher(dispatcher);
            ml.InitializeTimer();

            return ml;
        }

        /// <summary>
        /// Listen to a specific ID.
        /// </summary>
        /// <param name="id">ID to listen to.</param>
        /// <param name="dispatcher">Message transfer event dispatcher.</param>
        /// <param name="manualCheck">Don't use timer for checking for messages.</param>
        /// <returns></returns>
        public static MessageListener Listen(string id, MessageTransfer dispatcher, bool manualCheck)
        {
            var ml = new MessageListener();
            ml.ID = id;

            if (dispatcher != null) ml.InitializeDispatcher(dispatcher);
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

        private void InitializeDispatcher(MessageTransfer d)
        {
            MessageReceived += d;
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
        /// <returns>True if a message was received.</returns>

        public bool ForceCheck()
        {
            return CheckForMessage();
        }

        private bool CheckForMessage()
        {
            var dataPath = $@"{DataCacheLocation}\bcpc-{ID}";

            if (File.Exists(dataPath))
            {
                var message = File.ReadAllText(dataPath);

                File.Delete(dataPath);

                var mm = JsonConvert.DeserializeObject<MessageModel>(message);

                if (mm != null) OnMessageReceived(mm);

                return true;
            }

            return false;
        }

        protected virtual void OnMessageReceived(MessageModel message)
        {
            MessageReceived?.Invoke(ID, message);
        }
    }
}
