using System;

namespace sharpPDF
{
	/// <summary>
	/// Class that represents a pdfDestination of FitV type.
	/// </summary>
	internal class pdfDestinationFitV : IPdfDestination
	{
		int _left;

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="left">Left margin</param>
		public pdfDestinationFitV(int left)
		{
			_left = left;
		}

		/// <summary>
		/// Method that returns the PDF codes to write the destination
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getDestinationValue()
		{
			return "/FitV " + _left.ToString();
		}
	}
}
