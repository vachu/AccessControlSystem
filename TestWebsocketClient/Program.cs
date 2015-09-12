using System;
using WebSocketSharp;
using System.Threading;

namespace TestWebsocketClient
{
	class MainClass
	{
		private const string CONTAINER_NAME = "/Site1/access_control_system/demo";

		public static void Main (string[] args)
		{
			using (var ws = new WebSocket ("ws://localhost:55555" + CONTAINER_NAME)) {
				Int64 syncCtr = 0;
				ws.OnMessage += (sender, e) => {
					Console.WriteLine (e.Data);
					Interlocked.Decrement (ref syncCtr);
				};
				ws.Connect ();
				if (!ws.IsAlive) {
					Console.Error.WriteLine ("ERROR: Could not connect to Websocket Server");
					return;
				}

				var input = "";
				while (string.Compare (input.Trim (), "shutdown", true) != 0) {
					if (!string.IsNullOrWhiteSpace (input)) {
						ws.Send (input.Trim());
						Interlocked.Increment (ref syncCtr);

						while (Interlocked.Read (ref syncCtr) > 0) {
							Thread.Sleep (1);
						}
					}
					Console.Write ("Type in a message or 'shutdown' to quit: ");
					input = Console.ReadLine ();
				} // Longevity loop
			} // using
		}
	}
}
