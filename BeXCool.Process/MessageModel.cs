using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeXCool.Process
{
    public class MessageModel
    {
        public long MessageType { get; set; } = 0;
        public string Data { get; set; } = string.Empty;

        public MessageModel(long messageType, string data)
        {
            MessageType = messageType;
            Data = data;
        }

        public MessageModel(long messageType)
        {
            MessageType = messageType;
        }

        public MessageModel() { }
    }
}
