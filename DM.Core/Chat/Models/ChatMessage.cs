using System;
using System.Runtime.Serialization;

namespace SecuredChat
{
    [DataContract]
    public class ChatMessage : DataModel
    {
        [IgnoreDataMember]
        public string Message { get { return Convert.ToString(Data); } set { Data = value; } }

        public DateTime Time { get; set; }
    }

    [DataContract]
    public class ChatTyping : DataModel
    {

    }
}
