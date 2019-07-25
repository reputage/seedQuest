using System;

using sharpPDF.Enumerators;
using sharpPDF.Exceptions;

namespace sharpPDF
{
	/// <summary>
	/// Interface for a pdfDestination
	/// </summary>
	public interface IPdfDestination
	{
		/// <summary>
		/// Method that returns the PDF codes to write the destination
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		string getDestinationValue();
	}
}
