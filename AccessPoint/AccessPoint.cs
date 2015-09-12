using System;
using System.Collections.Generic;

namespace Crossover
{
	public class AccessPoint : IAccessPoint
	{
		public static IAccessPoint Create(IAccessPointContainer cont, string ID)
		{
			if (string.IsNullOrWhiteSpace (ID) || cont == null)
				return null;

			ID = ID.Trim ().ToLower ();
			if (ID.StartsWith ("ws://") || ID.StartsWith ("wss://"))
				return new RemoteAccessPointProxy (ID);  // right now, a WebSocket client
			/*
			 * else if (...) -> we could create WCF clients for Remote AccessPoints in future
			 * based on the URL type
			 * */
			else
				return new AccessPoint (cont, ID);
		}

		private IAccessPointContainer m_container;
		private string m_ID;

		internal AccessPoint (IAccessPointContainer cont, string ID)
		{
			m_container = cont;
			m_ID = ID;
		}

		#region IAccessPoint implementation
		private List<IAccessPoint> m_listPush = new List<IAccessPoint> ();
		private List<IAccessPoint> m_listPull = new List<IAccessPoint> ();

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

			if ((linkType & LinkType.Both) == LinkType.Pull) {
				m_listPull.Add (otherAccPt);
			}
			if ((linkType & LinkType.Both) == LinkType.Push) {
				m_listPush.Add (otherAccPt);
			}
		}

		#endregion
	}

	internal class RemoteAccessPointProxy : IAccessPoint
	{
		private string m_accPtID;

		internal RemoteAccessPointProxy(string accPtID)
		{
			m_accPtID = accPtID;
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

