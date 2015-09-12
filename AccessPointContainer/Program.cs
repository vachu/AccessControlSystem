using System;
using Crossover;
using System.Collections.Generic;

namespace AccessPointContainer
{
	class MainClass : IAccessPointContainer
	{
		private const string CONTAINER_NAME = "ws://company/access_control_system/demo";
		public static void Main (string[] args)
		{
			string[] accPtIDs = {
				"/Site1/Dept_1/Bldg_1",
				"/Site1/Dept_1/Bldg_2",
				"/Site1/Dept_2/Bldg_1",
				"/Site1/Dept_2/Bldg_2",
				"/Site1/Dept_3/Bldg_1",
				"/Site1/Dept_3/Bldg_2",
				"ws://company-intranet-url/Site3/Dept_4/Bldg_3",
					// AccessPoint in a different site / Container
					// There could be multiple AccessPoint Containers in a single site
			};
			var theApp = new MainClass ();

			foreach (var id in accPtIDs) {
				s_accPtMap [id] = AccessPoint.Create (theApp, id);
			}
			Console.WriteLine ("Access Point Container is now running @ {0}\n\n", CONTAINER_NAME);

			Console.Write ("Press the buzzword to quit: ");
			var input = Console.ReadLine ();
			while (string.Compare (input.Trim (), "shutdown", true) != 0) {
				input = Console.ReadLine ();
			} // Longevity loop
		}

		#region IAccessPointContainer implementation

		private static Dictionary<string, IAccessPoint> s_accPtMap =
			new Dictionary<string, IAccessPoint> ();

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
