using System;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace MyWebSocket {
    class Program {
        static void Main(string[] args) {

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://+:5000")
                .Configure(app => {
                    app.UseWebSockets();
                    app.Run(async context => {
                        if (!context.WebSockets.IsWebSocketRequest) {
                            context.Response.StatusCode = 400;
                            return;
                        }
                        using (var webRequest = await context.WebSockets.AcceptWebSocketAsync()) {
                            var data = "Hello, world";
                            var bytes = System.Text.UTF32Encoding.UTF8.GetBytes(data);
                            var segment = new ArraySegment<byte>(bytes);
                            var source = new System.Threading.CancellationTokenSource();
                            await webRequest.SendAsync(segment, WebSocketMessageType.Text, true, source.Token);
                        }
                    });
                }).Build();
            host.Run();
        }
    }
}
