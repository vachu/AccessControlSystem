using System;
using System.Collections.Generic;

namespace Crossover
{
	public static class AccessPointFactory
	{
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
			else
				return new AccessPoint (ID);
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
	}

	internal class AccessPoint : IAccessPoint
	{
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

		#endregion
	}
}

