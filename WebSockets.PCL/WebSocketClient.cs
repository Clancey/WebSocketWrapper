using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSockets
{
    public class WebSocketClient : IWebSocketClient
    {
        public bool IsOpen
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Action<string> MessageRecieved
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public Task Close(int statusCode, string reason)
        {
            throw new NotImplementedException();
        }

        public Task Connect(Uri uri, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
