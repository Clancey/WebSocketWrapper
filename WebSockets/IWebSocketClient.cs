using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebSockets
{
	public interface IWebSocketClient
	{
		Task Connect (Uri uri, CancellationToken token);
		Action<string> MessageRecieved { get; set; }
		bool IsOpen { get; }
        Task Close(int statusCode, string reason);
    }
}
