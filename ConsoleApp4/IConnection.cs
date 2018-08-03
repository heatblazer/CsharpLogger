
namespace ConsoleApp4
{
    public interface IConnection
    {
        bool Create();
        bool Connect();
        bool Disconnect();
        bool Destroy();
        void SendMessage(object msg); // object can be any custom defined messaage type 
        object GetMessage();
    }
}
