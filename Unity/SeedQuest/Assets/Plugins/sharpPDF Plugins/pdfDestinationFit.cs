using System;

namespace sharpPDF
{
	/// <summary>
	/// Class that represents a pdfDestination of Fit type.
	/// </summary>
	internal class pdfDestinationFit : IPdfDestination
	{
	
		/// <summary>
		/// Class's constructor
		/// </summary>
		public pdfDestinationFit()
		{

		}

		/// <summary>
		/// Method that returns the PDF codes to write the destination
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getDestinationValue()
		{
			return "/Fit";
		}

	}
}
