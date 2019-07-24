using System;
using System.Text;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a PDF font.
	/// </summary>
	internal class pdfFont : IWritable
	{		
		private predefinedFont _fontStyle;
		private int _objectID;
		private int _fontNumber;

		/// <summary>
		/// Font's style
		/// </summary>
		public predefinedFont fontStyle
		{
			get
			{
				return _fontStyle;
			}
		}

		/// <summary>
		/// Font's ID
		/// </summary>
		public int objectID
		{
			get
			{
				return _objectID;
			}
			set
			{
				_objectID = value;
			}
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="newFontStyle">Font's style</param>
		/// <param name="newFontNumber">Font's number in the PDF </param>
		public pdfFont(predefinedFont newFontStyle, int newFontNumber)
		{
			_fontStyle = newFontStyle;
			_fontNumber = newFontNumber;
		}

		/// <summary>
		/// Method that returns the PDF codes to write the Font in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getText()
		{
			StringBuilder content  = new StringBuilder();
			content.Append(_objectID.ToString() + " 0 obj" + Convert.ToChar(13) + Convert.ToChar(10));
			content.Append("<<" + Convert.ToChar(13) + Convert.ToChar(10));
			content.Append("/Type /Font" + Convert.ToChar(13) + Convert.ToChar(10));
			content.Append("/Subtype /Type1" + Convert.ToChar(13) + Convert.ToChar(10));
			content.Append("/Name /F" + _fontNumber.ToString() + Convert.ToChar(13) + Convert.ToChar(10));
			content.Append("/BaseFont /" + pdfFont.getFontName(_fontStyle) + Convert.ToChar(13) + Convert.ToChar(10));
			content.Append("/Encoding /WinAnsiEncoding" + Convert.ToChar(13) + Convert.ToChar(10));
			content.Append(">>" + Convert.ToChar(13) + Convert.ToChar(10));
			content.Append("endobj" + Convert.ToChar(13) + Convert.ToChar(10));
			return content.ToString();
		}

		/// <summary>
		/// Static Mehtod that returns the name of the font
		/// </summary>
		/// <param name="fontType">Font's Type</param>
		/// <returns>String that contains the name of the font</returns>
		public static string getFontName(predefinedFont fontType)
		{
			switch (fontType)
			{
				case predefinedFont.csHelvetica:
					return "Helvetica";
				case predefinedFont.csHelveticaBold:
					return "Helvetica-Bold";
				case predefinedFont.csHelveticaOblique:
					return "Helvetica-Oblique";
				case predefinedFont.csHelvetivaBoldOblique:
					return "Helvetica-BoldOblique";
				case predefinedFont.csCourier:
					return "Courier";
				case predefinedFont.csCourierBold:
					return "Courier-Bold";
				case predefinedFont.csCourierOblique:
					return "Courier-Oblique";
				case predefinedFont.csCourierBoldOblique:
					return "Courier-BoldOblique";
				case predefinedFont.csTimes:
					return "Times-Roman";
				case predefinedFont.csTimesBold:
					return "Times-Bold";
				case predefinedFont.csTimesOblique:
					return "Times-Italic";
				case predefinedFont.csTimesBoldOblique:
					return "Times-BoldItalic";
				default:
					return "";
			}
		}

	}
}
