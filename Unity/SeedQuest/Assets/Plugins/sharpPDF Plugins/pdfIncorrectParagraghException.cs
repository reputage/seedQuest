using System;

namespace sharpPDF.Exceptions
{
	/// <summary>
	/// Exception that represents an error during the creation of a paragraph, when the paragraph's element is of the wrong type(different form paragraphLine object)
	/// </summary>
	public class pdfIncorrectParagraghException : pdfException
	{
		/// <summary>
		/// Class's constructor
		/// </summary>		
		public pdfIncorrectParagraghException():base("The object isn't of the right type. It MUST be a paragraphLine object!",null)
		{
		}
	}
}
