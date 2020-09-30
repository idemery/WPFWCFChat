using idemery.Remoot.ClientHelper;
using System.Runtime.Serialization;

namespace SecuredChat
{
    [DataContract]
    [KnownType(typeof(ChatMessage))]
    [KnownType(typeof(ChatTyping))]
    [KnownType(typeof(ChatJoin))]
    [KnownType(typeof(ChatLeave))]
    [KnownType(typeof(ScreenModel))]
    public class DataModel
    {
        [DataMember]
        public ClientModel Sender { get; set; }
        [DataMember]
        public object Data { get; set; }
    }

    [DataContract]
    public class ScreenModel : DataModel
    {
        [DataMember]
        public bool Stopped { get; set; }
        [DataMember]
        public bool Reference { get; set; }
        [DataMember]
        public CompressedDesktop Desktop { get; set; }
    }
}
