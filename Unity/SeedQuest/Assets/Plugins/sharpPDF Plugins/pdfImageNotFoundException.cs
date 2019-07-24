using System;

namespace sharpPDF.Exceptions
{
	/// <summary>
	/// Exception that represents an nonexistent image 
	/// </summary>
	public class pdfImageNotFoundException : pdfException
	{
		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="message">Message for the exception</param>
		/// <param name="ex">Inner exception</param>
		public pdfImageNotFoundException(string message, Exception ex):base(message,ex)
		{
			
		}
	}
}
