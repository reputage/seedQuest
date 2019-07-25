using System;

namespace sharpPDF.Exceptions
{
	/// <summary>
	/// Exception that represents the insert of a non-pdfTableColumn into a columns collection
	/// </summary>
	public class pdfIncorrectColumnException : pdfException
	{
		/// <summary>
		/// Class's constructor
		/// </summary>
		public pdfIncorrectColumnException():base("The object isn't of the right type. It MUST be a pdfTableColumn object!",null)
		{			
		}
	}
}
