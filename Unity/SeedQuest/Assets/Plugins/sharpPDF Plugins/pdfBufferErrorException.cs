using System;

namespace sharpPDF.Exceptions
{
	/// <summary>
	/// Exception that represents an error during the I/O on the buffer.
	/// </summary>
	public class pdfBufferErrorException : pdfException
	{
		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="message">Message for the exception</param>
		/// <param name="ex">Inner exception</param>
		public pdfBufferErrorException(string message, Exception ex):base(message, ex)
		{
			
		}
	}
}
