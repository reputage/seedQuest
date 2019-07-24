using System;

namespace sharpPDF.Exceptions
{
	/// <summary>
	/// Exception that represents an error during the writing of the PDF document.
	/// </summary>
	public class pdfWritingErrorException : pdfException
	{
		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="message">Message for the exception</param>
		/// <param name="ex">Inner exception</param>
		public pdfWritingErrorException(string message, Exception ex):base(message,ex)
		{
			
		}
	}
}
