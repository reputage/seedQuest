using System;

namespace sharpPDF
{
	/// <summary>
	/// Class that represents a pdfDestination of XYZ type.
	/// </summary>
	internal class pdfDestinationXYZ : IPdfDestination
	{

		private int _left;
		private int _top;
		private int _zoom;		

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="left">Left margin</param>
		/// <param name="top">Top margin</param>
		/// <param name="zoom">Zoom</param>
		public pdfDestinationXYZ(int left, int top, int zoom)
		{
			_left = left;
			_top = top;
			_zoom = zoom;
		}

		/// <summary>
		/// Method that returns the PDF codes to write the destination
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getDestinationValue()
		{
			return "/XYZ " + _left.ToString() + " " + _top.ToString() + " " + (((float)_zoom)/100).ToString().Replace(",",".");
		}
		
		/// <summary>
		/// Method that format the zoom value
		/// </summary>
		/// <returns>String with zoom value</returns>
		private string getFormattedZoom()
		{
			string returnValue;
			if (_zoom >= 100) {
				returnValue = _zoom.ToString();
				returnValue = returnValue.Substring(0,returnValue.Length - 2) + "." + returnValue.Substring(returnValue.Length - 2 ,2);
			} else {
				returnValue = "." + _zoom.ToString();
			}
			return returnValue;
		}
	}
}
