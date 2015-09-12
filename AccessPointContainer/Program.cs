using System;
using Crossover;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace AccessPointContainer
{
	class MainClass : IAccessPointContainer
	{
		private const string CONTAINER_NAME = "/Site1/access_control_system/demo";
		public static void Main (string[] args)
		{
			var theApp = new MainClass ();
			string[] accPtIDs = {
				"/Site1",
				"/Site1/Dept_1/Bldg_1",
				"/Site1/Dept_2/Bldg_1",
				"/Site1/Dept_3/Bldg_2",
				"ws://company-intranet-url/Site3",
					// AccessPoint in a different Site / Container
					// There could be multiple AccessPoint Containers in a single site
					// In a non-demo code, this would be fetched from a config file
			};

			// Create all the Access Points
			foreach (var id in accPtIDs) {
				s_accPtMap [id] = AccessPoint.Create (theApp, id);
			}

			/*
			 * ---- Logically link all AccessPoints ----
			 * Ideally, this link-creation must be configurable through a config file but
			 * this is a demo after all.
			 * */
			s_accPtMap ["/Site1"].LinkTo ( // Site1's main access point to Site3's
				s_accPtMap ["ws://company-intranet-url/Site3"],
				LinkType.Both
			);

			// Link all Site1's non-main AccessPoints to the main one
			s_accPtMap ["/Site1/Dept_1/Bldg_1"].LinkTo (
				s_accPtMap ["/Site1"],
				LinkType.Both
			);
			s_accPtMap ["/Site1/Dept_2/Bldg_1"].LinkTo (
				s_accPtMap ["/Site1"],
				LinkType.Both
			);
			s_accPtMap ["/Site1/Dept_3/Bldg_2"].LinkTo (
				s_accPtMap ["/Site1"],
				LinkType.Both
			);
			/*
			 * ---- All AccessPoint linkings done ----
			 * */

			/* 
			 * Start the Websocket Server to listen to incoming requests
			 * */
			var wssvr = new WebSocketServer ("ws://localhost:55555");
			wssvr.AddWebSocketService<WebsocketSvr> (CONTAINER_NAME);
			wssvr.Start ();
			if (wssvr.IsListening) {
				Console.WriteLine ("Access Point Container running @ {0}\n\n", CONTAINER_NAME);

				Console.Write ("Type in the buzzword to quit: ");
				var input = Console.ReadLine ();
				while (string.Compare (input.Trim (), "shutdown", true) != 0) {
					input = Console.ReadLine ();
				} // Longevity loop
			}

			wssvr.Stop ();
		}

		#region IAccessPointContainer implementation

		private static Dictionary<string, IAccessPoint> s_accPtMap =
			new Dictionary<string, IAccessPoint>(StringComparer.CurrentCultureIgnoreCase);

		public IAccessPoint GetAccessPoint (string accPtID)
		{
			if (string.IsNullOrWhiteSpace (accPtID) ||
				!s_accPtMap.ContainsKey(accPtID.Trim()))
			{
				return null;
			}
			return s_accPtMap [accPtID.Trim ()];
		}


		public string[] GetAccessPointIDs ()
		{
			string[] IDs = new string[s_accPtMap.Keys.Count];
			s_accPtMap.Keys.CopyTo (IDs, 0);
			return IDs;
		}
		#endregion
	}
}
