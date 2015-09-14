using System;
using System.Collections.Generic;
using WebSocketSharp;

namespace Crossover
{
	public static class AccessPointFactory
	{
		private static List<AccessPoint> s_LocalAccPts = new List<AccessPoint> ();
		public static IAccessPoint Create(string ID)
		{
			if (string.IsNullOrWhiteSpace (ID))
				return null;

			ID = ID.Trim ().ToLower ();
			if (ID.StartsWith ("ws://") || ID.StartsWith ("wss://"))
				return new RemoteAccessPointProxy (ID);  // right now, a WebSocket client
			/*
			 * else if (...) -> we could create WCF clients for Remote AccessPoints in future
			 * based on the URL type
			 * */
			else {
				DeviceManager.Init (ID);
				var accPt = new AccessPoint (ID);
				s_LocalAccPts.Add (accPt);
				return accPt;
			}
		}

		public static void DumpAccessPointLinks(Dictionary<string, IAccessPoint> accPtMap)
		{
			foreach (var key in accPtMap.Keys) {
				Console.WriteLine ("AccessPoint ID: {0} -->", accPtMap[key].ID);

				if ((accPtMap [key]) is AccessPoint) {
					Console.WriteLine ("\tPulls:");
					foreach (var val in ((AccessPoint)accPtMap[key]).m_listPull) {
						Console.WriteLine ("\t\t{0}", val.ID);
					}
					Console.WriteLine ("\tPushes:");
					foreach (var val in ((AccessPoint)accPtMap[key]).m_listPull) {
						Console.WriteLine ("\t\t{0}", val.ID);
					}
				}
			}
		}

		/// <summary>
		/// Actuall inits and starts all AccessPoint components.  Must be called by the Container before
		/// the system becomes functional
		/// </summary>
		public static void Start()
		{
			DeviceManager.Start ();
			foreach (var accPt in s_LocalAccPts) {
				accPt.Connect2DeviceManager ("ws://localhost:44444/" + accPt.ID);
			}
		}

		/// <summary>
		/// Must be called before the Container terminates
		/// </summary>
		public static void Stop()
		{
			foreach (var accPt in s_LocalAccPts) {
				accPt.DisconnectDeviceManager ();
			}
			DeviceManager.Stop ();
		}
	}

	internal class AccessPoint : IAccessPoint
	{
		WebSocket m_wsDevMgr;

		internal void Connect2DeviceManager(string url)
		{
			m_wsDevMgr = new WebSocket (url + m_ID + "/?access_point_id=" + m_ID);
			m_wsDevMgr.OnMessage += (sender, e) => {
				ProcessDeviceManagerEvent (e.Data);
			};
			m_wsDevMgr.Connect ();
			if (!m_wsDevMgr.IsAlive) {
				throw new ApplicationException (
					"EXCEPTION: DeviceManager connect failed - " + m_wsDevMgr.Url
				);
			}
		}

		private void ProcessDeviceManagerEvent(string msg)
		{
		}

		internal void DisconnectDeviceManager()
		{
			if (m_wsDevMgr.IsAlive) {
				m_wsDevMgr.Close ();
			}
		}

		private string m_ID;
		internal AccessPoint (string ID)
		{
			m_ID = ID;
		}

		#region IAccessPoint implementation
		internal List<IAccessPoint> m_listPush = new List<IAccessPoint> ();
		internal List<IAccessPoint> m_listPull = new List<IAccessPoint> ();

		/// <summary>
		/// Links this AccessPoint object to another AccessPoint object for message / event passing
		/// </summary>
		/// <param name="otherAccPt">the other AccessPoint object to which this object has to be linked</param>
		/// <param name="linkType">Link type - whether it is a Push, Pull or Both.</param>
		/// <exception cref="ArgumentNullException">thrown when the supplied otherAccPt is null</exception>
		/// <exception cref="NotImplementedException">thrown if this AccessPoint object
		/// does not implement this method</exception>
		public void LinkTo (IAccessPoint otherAccPt, LinkType linkType)
		{
			if (otherAccPt == null) {
				throw new ArgumentNullException ("otherAccPt");
			}

			switch (linkType) {
			case LinkType.Pull:
				m_listPull.Add (otherAccPt);
				break;
			case LinkType.Push:
				m_listPush.Add (otherAccPt);
				break;
			default:
				m_listPull.Add (otherAccPt);
				m_listPush.Add (otherAccPt);
				break;
			}
		}

		public string ID {
			get {
				return m_ID;
			}
		}

		public bool Login (string mgrId)
		{
			return AccessPointDAL.Login (mgrId, m_ID);
			/*
			 * TODO in future
			 * Session Management
			 * */
		}

		public bool Logout (string mgrId)
		{
			return AccessPointDAL.Logout (mgrId, m_ID);
			/*
			 * TODO in future
			 * Session Management
			 * */
					}
		#endregion
	}

	internal class RemoteAccessPointProxy : IAccessPoint
	{
		private string m_ID;

		internal RemoteAccessPointProxy(string accPtID)
		{
			m_ID = accPtID;
		}

		public string ID {
			get {
				return m_ID;
			}
		}

		#region IAccessPoint implementation

		/// <summary>
		/// Links this AccessPoint object to another AccessPoint object for message / event passing
		/// </summary>
		/// <param name="otherAccPt">the other AccessPoint object to which this object has to be linked</param>
		/// <param name="linkType">Link type - whether it is a Push, Pull or Both.</param>
		/// <exception cref="ArgumentNullException">thrown when the supplied otherAccPt is null</exception>
		/// <exception cref="NotImplementedException">thrown if this AccessPoint object
		/// does not implement this method</exception>
		public void LinkTo (IAccessPoint otherAccPt, LinkType linkType)
		{
			throw new NotImplementedException ();
		}

		public bool Login (string mgrId)
		{
			throw new NotImplementedException ();
		}

		public bool Logout (string mgrId)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

