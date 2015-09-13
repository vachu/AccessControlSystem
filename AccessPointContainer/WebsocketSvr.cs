using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Text;
using System.Collections.Generic;

namespace AccessPointContainer
{
	public class Behaviour : WebSocketBehavior
	{
		delegate string Handler(string[] fields);
		Dictionary<string, Handler> m_msgHandlerMap =
			new Dictionary<string, Handler>(StringComparer.CurrentCultureIgnoreCase);

		public Behaviour()
		{
			m_msgHandlerMap ["login"] = ManagerOps.Login;
			m_msgHandlerMap ["logout"] = ManagerOps.Logout;
		}

		protected override void OnMessage(MessageEventArgs e)
		{
			var res = DispatchMessage (e);
			Send (res);
		}

		private string DispatchMessage(MessageEventArgs e) {
			if (e.Type != Opcode.Text) {
				return "ERROR: not a string message";
			}

			char[] WHITESPACE_CHARS = { '\b', '\t', ' ', '\n', '\v', '\f', '\a' };
			var msg = e.Data.Trim ();
			var fields = msg.Split(WHITESPACE_CHARS);
			if (fields.Length == 0) {
				return "ERROR: empty command line";
			}

			if (fields[0].ToLower() == "who") {
				return (this.Context.ToString ());
			}

			if (m_msgHandlerMap.ContainsKey (fields [0])) {
				return m_msgHandlerMap [fields [0]] (fields);
			}
			return "ERROR: Could not handle unknown command";
		}
	}
}

