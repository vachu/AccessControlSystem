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
