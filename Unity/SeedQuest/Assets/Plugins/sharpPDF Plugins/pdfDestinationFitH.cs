using System;

namespace sharpPDF
{	
	/// <summary>
	/// Class that represents a pdfDestination of FitH type.
	/// </summary>
	internal class pdfDestinationFitH : IPdfDestination
	{
		
		int _top;

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="top">Top margin</param>
		public pdfDestinationFitH(int top)
		{
			_top = top;
		}

		/// <summary>
		/// Method that returns the PDF codes to write the destination
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getDestinationValue()
		{
			return "/FitH " + _top.ToString();
		}

	}
}
