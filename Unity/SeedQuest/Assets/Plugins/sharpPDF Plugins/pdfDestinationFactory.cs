using System;

using sharpPDF;
using sharpPDF.Enumerators;
using sharpPDF.Exceptions;

namespace sharpPDF
{
	/// <summary>
	/// Class that represent a destination into a pdf document
	/// </summary>
	public abstract class pdfDestinationFactory
	{

		/// <summary>
		/// Method that creates a pdfDestination with XYZ type
		/// </summary>
		/// <param name="left">Left margin</param>
		/// <param name="top">Top margin</param>
		/// <param name="zoom">Page's zoom</param>
		/// <returns>pdfDestination object</returns>
		public static IPdfDestination createPdfDestinationXYZ(int left, int top, int zoom)
		{			
			return new pdfDestinationXYZ(left, top, zoom);
		}

		/// <summary>
		/// Method that creates a pdfDestination with Fit type
		/// </summary>
		/// <returns>pdfDestination object</returns>
		public static IPdfDestination createPdfDestinationFit()
		{			
			return new pdfDestinationFit();
		}

		/// <summary>
		/// Method that creates a pdfDestination with FitH type
		/// </summary>
		/// <param name="top">Top margin</param>
		/// <returns>pdfDestination object</returns>
		public static IPdfDestination createPdfDestinationFitH(int top)
		{
			return new pdfDestinationFitH(top);
		}

		/// <summary>
		/// Method that creates a pdfDestination with FitV type
		/// </summary>
		/// <param name="left">Left margin</param>
		/// <returns>pdfDestination object</returns>
		public static IPdfDestination createPdfDestinationFitV(int left)
		{
			return new pdfDestinationFitV(left);
		}

		/// <summary>
		/// Mathod that creates a pdfDestination with FitR type
		/// </summary>
		/// <param name="left">Left margin</param>
		/// <param name="top">Top margin</param>
		/// <param name="bottom">Bottom margin</param>
		/// <param name="right">Right margin</param>
		/// <returns>pdfDestination object</returns>
		public static IPdfDestination createPdfDestinationFitR(int left, int top, int bottom, int right)
		{
			return new pdfDestinationFitR(left,top,bottom,right);
		}

	}
}
