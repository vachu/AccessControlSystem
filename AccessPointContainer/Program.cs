using System;
using Crossover;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace AccessPointContainer
{
	class MainClass
	{
		private const string CONTAINER_NAME = "/site1/demo";

		internal static Dictionary<string, IAccessPoint> s_accPtMap =
			new Dictionary<string, IAccessPoint>(StringComparer.CurrentCultureIgnoreCase);

		public static void Main (string[] args)
		{
			string[] accPtIDs = {
				"/site1",
				"/site1/dept1/bldg1",
				"/site1/dept2/bldg1",
				"/site1/dept3/bldg2",
				"ws://company-intranet-url/Site3",
					// AccessPoint in a different Site / Container
					// There could be multiple AccessPoint Containers in a single site
					// In a non-demo code, this would be fetched from a config file
			};

			// Create all the Access Points
			foreach (var id in accPtIDs) {
				s_accPtMap [id] = AccessPointFactory.Create (id);
			}

			/*
			 * ---- Logically link all AccessPoints ----
			 * Ideally, this link-creation must be configurable through a config file or
			 * a Configuration Utlity but this is a demo after all.
			 * */
			s_accPtMap ["/site1"].LinkTo ( // Site1's main access point to Site3's
				s_accPtMap ["ws://company-intranet-url/Site3"],
				LinkType.Both
			);

			// Link all Site1's non-main AccessPoints to the main one
			s_accPtMap ["/Site1/Dept1/Bldg1"].LinkTo (
				s_accPtMap ["/Site1"],
				LinkType.Both
			);
			s_accPtMap ["/Site1/Dept2/Bldg1"].LinkTo (
				s_accPtMap ["/Site1"],
				LinkType.Both
			);
			s_accPtMap ["/Site1/Dept3/Bldg2"].LinkTo (
				s_accPtMap ["/Site1"],
				LinkType.Both
			);
			AccessPointFactory.Start ();
			/*
			 * ---- All AccessPoint linkings done ----
			 * */

			/* 
			 * Start the Container's Websocket Server to listen to incoming requests.
			 * This WS server would be the interface to other external AccessPoints
			 * */
			var wssvr = new WebSocketServer ("ws://localhost:55555");
			wssvr.AddWebSocketService<Behaviour> (CONTAINER_NAME);
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
			AccessPointFactory.Stop ();
		}
	}
}
