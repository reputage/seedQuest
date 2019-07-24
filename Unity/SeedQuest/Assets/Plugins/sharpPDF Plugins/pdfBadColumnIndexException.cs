using System;

namespace sharpPDF.Exceptions
{
	/// <summary>
	/// Exception that represents an error during an access on the pdfTableRow's columns with a bad index
	/// </summary>
	public class pdfBadColumnIndexException : pdfException
	{
		/// <summary>
		/// Class's constructor
		/// </summary>
		public pdfBadColumnIndexException():base ("The columnd index does not exist", null)
		{
		}
	}
}
