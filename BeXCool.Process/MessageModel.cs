using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeXCool.Process
{
    public class MessageModel
    {
        public Enum MessageType { get; set; }
        public string Data { get; set; } = string.Empty;

        public MessageModel(Enum messageType, string data)
        {
            MessageType = messageType;
            Data = data;
        }

        public MessageModel(Enum messageType)
        {
            MessageType = messageType;
        }
    }
}
