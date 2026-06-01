using SyncScript.Core;
using Microsoft.AspNetCore.SignalR;
namespace SyncScript.API.Hubs
{
    public class SyncHub : Hub
    {
        private readonly CRDTDocument _document;
        public SyncHub(CRDTDocument document)
        {
            _document = document;
        }

        public async Task SendOperation(CRDTOperation operation)
        {
            _document.Apply(operation);
            await Clients.Others.SendAsync("ReceiveOperation", operation);
        }
    }
}
