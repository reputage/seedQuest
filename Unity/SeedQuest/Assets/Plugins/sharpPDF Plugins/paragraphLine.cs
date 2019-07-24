using System;
using System.Text;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a PDF paragraph's line.
	/// </summary>
	public class paragraphLine : IWritable
	{
		private string _strLine;
		private int _lineLeftMargin;
		private int _lineTopMargin;

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="strLine">Text of the line</param>
		/// <param name="lineTopMargin">Top margin</param>
		/// <param name="lineLeftMargin">Left margin</param>
		public paragraphLine(string strLine, int lineTopMargin, int lineLeftMargin)
		{
			_strLine = strLine;
			_lineTopMargin = lineTopMargin;
			_lineLeftMargin = lineLeftMargin;
		}

		/// <summary>
		/// Method that returns the PDF codes to write the paragraph's line in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getText()
		{
			StringBuilder resultString = new StringBuilder();			
			resultString.Append(_lineLeftMargin.ToString() + " -"+ _lineTopMargin.ToString() + " Td" + Convert.ToChar(13) + Convert.ToChar(10));
			resultString.Append("(" + textAdapter.checkText(_strLine) + ") Tj" + Convert.ToChar(13) + Convert.ToChar(10));			
			resultString.Append("-" + _lineLeftMargin.ToString().Replace(",",".") + " 0 Td" + Convert.ToChar(13) + Convert.ToChar(10));			
			return resultString.ToString();
		}
	}
}
