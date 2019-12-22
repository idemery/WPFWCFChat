using System.ServiceModel;

namespace SecuredChat
{
    public interface IClientService
    {
        [OperationContract(IsOneWay = true)]
        void Receive(DataModel data);
    }
}
