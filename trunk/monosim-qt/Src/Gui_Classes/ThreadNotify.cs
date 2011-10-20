using System;
using Qyoto;

namespace monosimqt
{
	
	/// <summary>
	/// Interface for gui update method
	/// </summary>
	public interface IThreadNotify: IQWidgetSignals
    {
            [Q_SIGNAL]
            void UpdateGui();
    }
	
	
	/// <summary>
	/// Class to manage Qt signal to gui update
	/// </summary>
	public class ThreadNotify: QWidget
	{
		
		
		/// <summary>
		/// Method recalled from client
		/// </summary>
		public void WakeupMain()
		{
			Emit.UpdateGui();
		}
		

		
		/// <summary>
		/// Emitter
		/// </summary>
        protected new IThreadNotify Emit
        {
	        get
	        {
	            return (IThreadNotify) Q_EMIT;
	        }
        }
		
		
		
	}
}

