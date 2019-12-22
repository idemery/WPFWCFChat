using System.Runtime.Serialization;

namespace SecuredChat
{
    [DataContract]
    public class ChatLeave : DataModel
    {
        public override string ToString()
        {
            return "left chat.";
        }
    }
}
