using System;

namespace Revanche
{
	/// <summary>
	/// Interface for fields that edit values.
	/// </summary>
	public interface ValuedField
	{
		/// <summary>
		/// Gets the current value from the UI.
		/// </summary>
		/// <value>The value.</value>
		object Value{ get; }

		/// <summary>
		/// Occurs when the user presses Tab
		/// </summary>
		event EventHandler Tabbed;
		/// <summary>
		/// Occurs when the user presses Shift + Tab
		/// </summary>
		event EventHandler BackTabbed;

		/// <summary>
		/// Grabs focus into the editable component of this field.
		/// </summary>
		void Focus();
	}
}

