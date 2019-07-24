using System;

namespace sharpPDF.Exceptions
{
	/// <summary>
	/// Exception that represents the insert of a non-pdfTableRow into a rows collection
	/// </summary>
	public class pdfIncorrectRowException : pdfException
	{
		/// <summary>
		/// Class's constructor
		/// </summary>
		public pdfIncorrectRowException():base("The object isn't of the right type. It MUST be a pdfTableRow object!",null)
		{			
		}
	}
}
