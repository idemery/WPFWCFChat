using System.ServiceModel;
using System.Threading.Tasks;

namespace SecuredChat
{
    [ServiceContract(CallbackContract = typeof(IClientService), SessionMode = SessionMode.Required)]
    [ServiceKnownType(typeof(ScreenModel))]
    public interface IHostService
    {
        [OperationContract(IsInitiating = true, IsOneWay = true)]
        void Connect(ClientModel clientModel);

        [OperationContract(IsOneWay = true)]
        void Send(object data);

        [OperationContract(IsTerminating = true, IsOneWay = true)]
        void Disconnect();
    }
}
