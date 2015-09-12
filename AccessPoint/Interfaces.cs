namespace Crossover
{
	/// <summary>
	/// For the use of the Department Manager Client app
	/// </summary>
	public interface IManager
	{
	}

	/// <summary>
	/// For the use by Administrator / Configurator app, of the whole Access Control System
	/// </summary>
	public interface IAdmin
	{
	}

	/// <summary>
	/// Main for use by the Security / Control-room Personnel App
	/// </summary>
	public interface IMonitorControl
	{
	}

	/// <summary>
	/// For "pushing" messages / events to other AccessPoint component.  A response is not expected
	/// </summary>
	public interface IPush
	{
	}

	/// <summary>
	/// For "pulling" information from other AccessPoint component. A response is very much expected here
	/// </summary>
	public interface IPull
	{
	}

	/// <summary>
	/// All AccessPoint components implement this interface
	/// </summary>
	public interface IAccessPoint : IManager, IAdmin, IMonitorControl, IPull, IPush
	{
		/// <summary>
		/// Links this AccessPoint object to another AccessPoint object for message / event passing
		/// </summary>
		/// <param name="otherAccPt">the other AccessPoint object to which this object has to be linked</param>
		/// <param name="linkType">Link type - whether it is a Push, Pull or Both.</param>
		/// <exception cref="ArgumentNullException">thrown when the supplied otherAccPt is null</exception>
		/// <exception cref="NotImplementedException">thrown if this AccessPoint object
		/// does not implement this method</exception>
		void LinkTo(IAccessPoint otherAccPt, LinkType linkType);
	}

	/// <summary>
	/// The type of link between 2 AccessPoint objects.
	/// 'Push' denotes the "src" AccessPoint would just push messages / events without expecting a response.
	/// 'Pull' denotes the "src" AccessPoint would very much expect a response for its earlier message / response
	/// 'Both' denotes 'Push' AND 'Pull'
	/// </summary>
	public enum LinkType {
		Push = 1,
		Pull,
		Both
	}
	/// <summary>
	/// This interface must be implemented by the "hosting" app / module - could be
	/// a simple Console App or a Service
	/// </summary>
	public interface IAccessPointContainer
	{
		IAccessPoint GetAccessPoint(string accPtID);
		string[] GetAccessPointIDs();
	}
}
