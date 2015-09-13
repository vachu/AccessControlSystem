using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace AccessPointContainer
{
	public class Behaviour : WebSocketBehavior
	{
		protected override void OnMessage(MessageEventArgs e)
		{
			if (!DispatchMessage (e)) {
				Send ("ERROR: Invalid Message");
			}
		}

		private bool DispatchMessage(MessageEventArgs e) {
			if (e.Type != Opcode.Text) {
				return false;
			}

			var msg = e.Data.Trim ();
			if (msg.ToLower() == "who") {
				Send (this.Context.ToString ());
				return true;
			}
			return false;
		}
	}
}

