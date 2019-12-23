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

        [OperationContract(IsInitiating = false, IsOneWay = true)]
        void Send(DataModel dataModel);

        [OperationContract(IsInitiating = false, IsOneWay = true)]
        void Disconnect();
    }
}
