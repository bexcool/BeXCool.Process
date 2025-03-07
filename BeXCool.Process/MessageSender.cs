using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BeXCool.Process
{
    /// <summary>
    /// Send a message to the Message Receiver.
    /// </summary>
    public static class MessageSender
    {
        /// <summary>
        /// Where will packets be stored.
        /// </summary>
        public static string DataCacheLocation { get; set; } = Path.GetTempPath();

        /// <summary>
        /// Sends a message model object.
        /// </summary>
        /// <param name="id">Message ID.</param>
        /// <param name="message">Message to send.</param>
        public static void SendMessage(string id, MessageModel message)
        {
            var messageFile = $@"{DataCacheLocation}\bcpc-{id}";

            File.WriteAllText(messageFile, JsonConvert.SerializeObject(message));
        }

    }
}
