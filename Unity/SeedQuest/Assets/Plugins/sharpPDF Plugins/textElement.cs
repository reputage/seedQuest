using System;
using System.Text;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// Descrizione di riepilogo per textElement.
	/// </summary>
	internal class textElement : pdfElement
	{
		private string _content;
		private int _fontSize;		
		private predefinedFont _fontType;		

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="newContent">Text's content</param>
		/// <param name="newFontSize">Font's size</param>
		/// <param name="newFontType">Font's type</param>
		/// <param name="newCoordX">X position of the text in the page</param>
		/// <param name="newCoordY">Y position of the text in the page</param>
		public textElement(string newContent, int newFontSize, predefinedFont newFontType, int newCoordX, int newCoordY)
		{
			_content = newContent;
			_fontSize = newFontSize;
			_fontType = newFontType;
			_coordX = newCoordX;
			_coordY = newCoordY;
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="newContent">Text's content</param>
		/// <param name="newFontSize">Font's size</param>
		/// <param name="newFontType">Font's type</param>
		/// <param name="newCoordX">X position of the text in the page</param>
		/// <param name="newCoordY">Y position of the text in the page</param>
		/// <param name="newStrokeColor">Font's color</param>
		public textElement(string newContent, int newFontSize, predefinedFont newFontType, int newCoordX, int newCoordY, predefinedColor newStrokeColor)
		{
			_content = newContent;
			_fontSize = newFontSize;
			_fontType = newFontType;
			_coordX = newCoordX;
			_coordY = newCoordY;
			_strokeColor = new pdfColor(newStrokeColor);
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="newContent">Text's content</param>
		/// <param name="newFontSize">Font's size</param>
		/// <param name="newFontType">Font's type</param>
		/// <param name="newCoordX">X position of the text in the page</param>
		/// <param name="newCoordY">Y position of the text in the page</param>
		/// <param name="newStrokeColor">Font's color</param>
		public textElement(string newContent, int newFontSize, predefinedFont newFontType, int newCoordX, int newCoordY, pdfColor newStrokeColor)
		{
			_content = newContent;
			_fontSize = newFontSize;
			_fontType = newFontType;
			_coordX = newCoordX;
			_coordY = newCoordY;
			_strokeColor = newStrokeColor;
		}

		/// <summary>
		/// Text's content
		/// </summary>
		public string content
		{
			get
			{
				return _content;
			}

			set
			{
				_content = value;
			}

		}

		/// <summary>
		/// Font's size
		/// </summary>
		public int fontSize
		{
			get
			{
				return _fontSize;
			}

			set
			{
				_fontSize = value;
			}
		}

		/// <summary>
		/// Font's type
		/// </summary>
		public predefinedFont fontType
		{
			get
			{
				return fontType;
			}

			set
			{
				_fontType = value;
			}
		}

		/// <summary>
		/// Method that returns the PDF codes to write the text in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public override string getText()
		{
			StringBuilder resultText = new StringBuilder();
			StringBuilder hexContent = new StringBuilder();
			resultText.Append(_objectID.ToString() + " 0 obj" + Convert.ToChar(13) + Convert.ToChar(10));
            resultText.Append("<<" + Convert.ToChar(13) + Convert.ToChar(10));
            resultText.Append("/Filter [/ASCIIHexDecode]" + Convert.ToChar(13) + Convert.ToChar(10));
			hexContent.Append("q" + Convert.ToChar(13) + Convert.ToChar(10));
            hexContent.Append("BT" + Convert.ToChar(13) + Convert.ToChar(10));
            hexContent.Append("/F" + Convert.ToInt32(_fontType).ToString() + " " + _fontSize.ToString() + " Tf" + Convert.ToChar(13) + Convert.ToChar(10));
            if(_strokeColor.isColor()) {
				hexContent.Append(_strokeColor.rColor + " " + _strokeColor.gColor + " " + _strokeColor.bColor + " rg" + Convert.ToChar(13) + Convert.ToChar(10));            
			}
            hexContent.Append(_coordX.ToString() + " " + _coordY.ToString() + " Td" + Convert.ToChar(13) + Convert.ToChar(10));
            hexContent.Append("(" + textAdapter.checkText(_content) + ") Tj" + Convert.ToChar(13) + Convert.ToChar(10));
            hexContent.Append("ET" + Convert.ToChar(13) + Convert.ToChar(10));
            hexContent.Append("Q");
			resultText.Append("/Length " + ((hexContent.Length * 2) + 1).ToString() + Convert.ToChar(13) + Convert.ToChar(10));
			resultText.Append(">>" + Convert.ToChar(13) + Convert.ToChar(10));
            resultText.Append("stream" + Convert.ToChar(13) + Convert.ToChar(10));
            resultText.Append(textAdapter.encodeHEX(hexContent.ToString()) + ">" + Convert.ToChar(13) + Convert.ToChar(10));			
            resultText.Append("endstream" + Convert.ToChar(13) + Convert.ToChar(10));            
            resultText.Append("endobj" + Convert.ToChar(13) + Convert.ToChar(10));
			hexContent = null;
			return resultText.ToString();
		}
	}
}
