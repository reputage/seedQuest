using System;

namespace sharpPDF.Exceptions
{
	/// <summary>
	/// Exception that represents an error during the I/O of an image file.
	/// </summary>
	public class pdfImageIOException : pdfException
	{
		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="message">Message for the exception</param>
		/// <param name="ex">Inner exception</param>
		public pdfImageIOException(string message, Exception ex):base(message,ex)
		{
			
		}
	}
}
