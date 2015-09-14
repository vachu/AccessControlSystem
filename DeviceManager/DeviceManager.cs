using System;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Collections.Generic;

namespace Crossover
{
	public class DeviceManager : WebSocketBehavior
	{
		private static WebSocketServer s_wssvr;

		public static void Init(string ID)
		{
			if (s_wssvr == null) {
				s_wssvr = new WebSocketServer("ws://localhost:44444");
			}
			s_wssvr.AddWebSocketService<DeviceManager> (ID);
			Console.WriteLine ("DeviceManagerID={0}", ID);
		}

		public static void Start()
		{
			s_wssvr.Start ();
			if (!s_wssvr.IsListening) {
				throw new ApplicationException ("EXCEPTION: DeviceManager Initialization FAILED");
			}
			Console.WriteLine ("===========================================");
			Console.Write ("DeviceManager --> ");
			Console.WriteLine("IPAddress={0}; Port={1}", s_wssvr.Address.ToString(), s_wssvr.Port);
			/*
			 * In reality, Hardware Device Initializations could be done here
			 * */
		}

		public static void Stop()
		{
			if (s_wssvr.IsListening) {
				s_wssvr.Stop ();
			}
		}

		delegate string Handler(string[] fields);
		Dictionary<string, Handler> m_msgHandlerMap =
			new Dictionary<string, Handler>(StringComparer.CurrentCultureIgnoreCase);

		public DeviceManager()
		{
			m_msgHandlerMap ["checkin"] = DeviceHandler.Checkin;
			m_msgHandlerMap ["checkout"] = DeviceHandler.Checkout;
		}

		private static object s_lockObj = new object();
		private static Dictionary<string, WebSocket> s_accPtClients =
			new Dictionary<string, WebSocket>();

		internal static WebSocket GetClientWS(string ID)
		{
			if (s_accPtClients.ContainsKey (ID)) {
				return s_accPtClients [ID];
			}
			return null;
		}

		private string m_accPtClientID;
		protected override void OnOpen()
		{
			m_accPtClientID = Context.QueryString ["access_point_id"];
			if (!string.IsNullOrWhiteSpace (m_accPtClientID)) {
				m_accPtClientID = m_accPtClientID.Trim ();
				lock (s_lockObj) {
					s_accPtClients [m_accPtClientID] = Context.WebSocket;
				}
			}
		}

		protected override void OnMessage(MessageEventArgs e)
		{
			if (e.Type == Opcode.Close) {
				lock (s_lockObj) {
					if (s_accPtClients.ContainsKey (m_accPtClientID)) {
						s_accPtClients.Remove (m_accPtClientID);
					}
				}
			} else {
				var res = DispatchMessage (e);
				Send (res);
			}
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
			return "ERROR: Unknown command - " + fields[0];
		}
	}

	internal static class DeviceHandler
	{
		public static string Checkin(string[] fields)
		{
			// Expected cmd line: checkin <emp-id> <access-point-id>
			if (fields.Length == 3) {
				var ws = DeviceManager.GetClientWS (fields [2]);
				if (ws != null) {
					ws.Send (string.Join (" ", fields));
					return "OK: checkin relayed";
				}
			}
			return "ERROR: Illegal args for 'checkin'";
		}

		public static string Checkout(string[] fields)
		{
			// Expected cmd line: checkout <emp-id> <access-point-id>
			if (fields.Length == 3) {
				var ws = DeviceManager.GetClientWS (fields [2]);
				if (ws != null) {
					ws.Send (string.Join (" ", fields));
					return "OK: checkout relayed";
				}
			}
			return "ERROR: Illegal args for 'checkout'";
		}
	}
}

