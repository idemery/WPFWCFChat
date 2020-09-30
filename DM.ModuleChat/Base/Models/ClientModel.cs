using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace SecuredChat
{
    [DataContract]
    public class ClientModel
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string SessionId { get; set; }

        [IgnoreDataMember]
        public OperationContext Context { get; set; }
        [IgnoreDataMember]
        public IClientService Callback { get { return Context.GetCallbackChannel<IClientService>(); } }
        [IgnoreDataMember]
        public RemoteEndpointMessageProperty Endpoint { get { return Context.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty; } }
    }
}
