#if __MOBILE__
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Net.WebSockets;
using System.Text;

namespace WebSockets
{
    public class WebSocketClient : IWebSocketClient
	{
		ClientWebSocket socket = new ClientWebSocket ();
		public WebSocketClient ()
		{

		}
		public async Task Connect (Uri uri, CancellationToken token)
		{
			await socket.ConnectAsync (uri, token);
			StartListening ();
		}

		public bool IsOpen => socket.State == WebSocketState.Open;

		Task pollingTask;
		CancellationTokenSource cancelSource;
		void StartListening ()
		{
			if (pollingTask?.IsCompleted ?? true)
				pollingTask = Task.Run (Listen);
		}

		async Task Listen ()
		{
			cancelSource = new CancellationTokenSource ();
			var buffer = new byte [1024];
			while (socket.State == WebSocketState.Open) {
				try {
					var segment = new ArraySegment<byte> (buffer);
					WebSocketReceiveResult result;
					string message = "";
					bool hasMore = true;
					while (hasMore) {
						result = await socket.ReceiveAsync (segment, cancelSource.Token);
						if (result.MessageType == WebSocketMessageType.Close) {
							await socket.CloseAsync (WebSocketCloseStatus.InvalidMessageType, "", CancellationToken.None);
							hasMore = false;
						}
						else {
							message += Encoding.UTF8.GetString (buffer, 0, result.Count);
							hasMore = !result.EndOfMessage;
						}
					}
					if (!string.IsNullOrWhiteSpace (message));
						MessageRecieved?.Invoke (message);

				} catch (Exception ex) {
					Console.WriteLine (ex);
				}

			}
		}

		public Action<string> MessageRecieved { get; set; }

        public async Task Close(int statusCode, string reason)
        {
            await socket.CloseAsync((WebSocketCloseStatus)statusCode, reason,CancellationToken.None);
        }

	}
}

#endif
