using System;
using System.Text;
using System.Collections;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a PDF paragraph.
	/// </summary>
	internal class paragraphElement : pdfElement
	{
		private IEnumerable _content;
		private int _fontSize;		
		private predefinedFont _fontType;		

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="newContent">Text of the paragraph</param>
		/// <param name="newFontSize">Font's size</param>
		/// <param name="newFontType">Font's type</param>
		/// <param name="newCoordX">X position in the PDF document</param>
		/// <param name="newCoordY">Y position in the PDF document</param>
		public paragraphElement(IEnumerable newContent, int newFontSize, predefinedFont newFontType, int newCoordX, int newCoordY)
		{
			_content = newContent;
			_fontSize = newFontSize;
			_fontType = newFontType;
			_coordX = newCoordX;
			_coordY = newCoordY;
		}

		/// <summary>
		/// Class's constructor [DEPRECATED]
		/// </summary>
		/// <param name="newContent">Text of the paragraph</param>
		/// <param name="newFontSize">Font's size</param>
		/// <param name="newFontType">Font's type</param>
		/// <param name="newCoordX">X position in the PDF document</param>
		/// <param name="newCoordY">Y position in the PDF document</param>
		/// <param name="newStrokeColor">Font's color</param>
		public paragraphElement(IEnumerable newContent, int newFontSize, predefinedFont newFontType, int newCoordX, int newCoordY, predefinedColor newStrokeColor)
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
		/// <param name="newContent">Text of the paragraph</param>
		/// <param name="newFontSize">Font's size</param>
		/// <param name="newFontType">Font's type</param>
		/// <param name="newCoordX">X position in the PDF document</param>
		/// <param name="newCoordY">Y position in the PDF document</param>
		/// <param name="newStrokeColor">Font's color</param>
		public paragraphElement(IEnumerable newContent, int newFontSize, predefinedFont newFontType, int newCoordX, int newCoordY, pdfColor newStrokeColor)
		{
			_content = newContent;
			_fontSize = newFontSize;
			_fontType = newFontType;
			_coordX = newCoordX;
			_coordY = newCoordY;
			_strokeColor = newStrokeColor;
		}

		/// <summary>
		/// IEnumerable interface that contains paragraph's lines
		/// </summary>
		public IEnumerable content
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
		/// Method that returns the PDF codes to write the paragraph in the document
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
            if(_strokeColor.isColor())
				hexContent.Append(_strokeColor.rColor + " " + _strokeColor.gColor + " " + _strokeColor.bColor + " rg" + Convert.ToChar(13) + Convert.ToChar(10));            
            hexContent.Append(_coordX.ToString() + " " + _coordY.ToString() + " Td" + Convert.ToChar(13) + Convert.ToChar(10));
			hexContent.Append("14 TL" + Convert.ToChar(13) + Convert.ToChar(10));
			foreach (object line in _content) {
				if (Type.GetType("sharpPDF.paragraphLine").IsInstanceOfType(line)) {
					hexContent.Append(((paragraphLine)line).getText());
				} else {
					throw new pdfIncorrectParagraghException();
				}
			}
            hexContent.Append("ET" + Convert.ToChar(13) + Convert.ToChar(10));
            hexContent.Append("Q" + Convert.ToChar(13) + Convert.ToChar(10));
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
