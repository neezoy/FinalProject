using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkLibrary
{
    public class MessageModel
    {
        public int MessageHeaderType { get; set; }

        public String TimeData { get; set; }
        public String MessageData { get; set; }
        
    }
}
