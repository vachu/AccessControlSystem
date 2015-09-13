using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace AccessPointContainer
{
	internal static class ManagerOps
	{
		internal static string Login(string[] fields)
		{
			if (fields.Length == 3) {
				var mgrId = fields [1].Trim();
				var siteId = fields [2].Trim();
				if (MainClass.s_accPtMap.ContainsKey(siteId) &&
					MainClass.s_accPtMap[siteId].Login(mgrId)) {
					return mgrId + " logged in";
				}
				else {
					return "ERROR: Invalid Site ID " + siteId;
				}
			}
			return "ERROR: invalid no. of args";
		}

		internal static string Logout(string[] fields)
		{
			if (fields.Length == 3) {
				var mgrId = fields [1].Trim();
				var siteId = fields [2].Trim();
				if (MainClass.s_accPtMap.ContainsKey(siteId) &&
					MainClass.s_accPtMap[siteId].Logout(mgrId)) {
					return mgrId + " logged out";
				}
				else {
					return "ERROR: Invalid Site ID " + siteId;
				}
			}
			return "ERROR: invalid no. of args";
		}
	}
}

