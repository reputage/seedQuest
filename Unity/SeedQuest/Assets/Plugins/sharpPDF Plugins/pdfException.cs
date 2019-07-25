using System;

namespace sharpPDF.Exceptions
{
	/// <summary>
	/// Generic PDF Exception.
	/// </summary>
	public class pdfException : Exception
	{
		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="message">Message for the exception</param>
		/// <param name="ex">Inner exception</param>
		public pdfException(string message, Exception ex):base(message, ex)
		{
		
		}
	}
}
