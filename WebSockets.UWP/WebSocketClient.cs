using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace WebSockets
{
    public class WebSocketClient : IWebSocketClient
    {
        MessageWebSocket socket = new MessageWebSocket();
        public WebSocketClient()
        {
            socket.Closed += Socket_Closed;
            socket.MessageReceived += Socket_MessageReceived;
        }

        private void Socket_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            var reader = args.GetDataReader();
            reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
            var message = reader.ReadString(reader.UnconsumedBufferLength);
            if(!string.IsNullOrWhiteSpace(message))
                MessageRecieved?.Invoke(message);
        }

        private void Socket_Closed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            IsOpen = false;
        }

        public bool IsOpen { get; protected set; }

        public Action<string> MessageRecieved { get; set; }

        public async Task Connect(Uri uri, CancellationToken token)
        {
            await socket.ConnectAsync(uri);
            IsOpen = true;
        }

        public Task Close(int statusCode,string reason)
        {
            socket.Close((ushort)statusCode,reason);
            return Task.FromResult(true);
        }
    }
}
