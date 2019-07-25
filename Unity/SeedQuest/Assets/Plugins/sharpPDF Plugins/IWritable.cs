using System;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// Generic interface for pdf's objects
	/// </summary>	
	internal interface IWritable
	{
		/// <summary>
		/// Method that returns the PDF codes to write the object in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		string getText();
	}
}
