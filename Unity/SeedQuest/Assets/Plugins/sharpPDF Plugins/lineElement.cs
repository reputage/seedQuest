using System;
using System.Text;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a PDF line.
	/// </summary>
	internal class lineElement : pdfElement
	{
		
		private int _coordX1;
		private int _coordY1;
		private pdfLineStyle _lineStyle;

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="X1">X1 position in the PDF document</param>
		/// <param name="Y1">Y1 position in the PDF document</param>
		public lineElement(int X, int Y, int X1, int Y1)
		{
			_coordX = X;
			_coordY = Y;
			_coordX1 = X1;
			_coordY1 = Y1;
			_lineStyle = new pdfLineStyle(1,predefinedLineStyle.csNormal);
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="X1">X1 position in the PDF document</param>
		/// <param name="Y1">Y1 position in the PDF document</param>
		/// <param name="newWidth">Line's size</param>
		public lineElement(int X, int Y, int X1, int Y1, int newWidth) 
		{
			_coordX = X;
			_coordY = Y;
			_coordX1 = X1;
			_coordY1 = Y1;
			_lineStyle = new pdfLineStyle(newWidth,predefinedLineStyle.csNormal);
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="X1">X1 position in the PDF document</param>
		/// <param name="Y1">Y1 position in the PDF document</param>		
		/// <param name="newStyle">Line's style</param>
		public lineElement(int X, int Y, int X1, int Y1, predefinedLineStyle newStyle) 
		{
			_coordX = X;
			_coordY = Y;
			_coordX1 = X1;
			_coordY1 = Y1;
			_lineStyle = new pdfLineStyle(newStyle);
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="X1">X1 position in the PDF document</param>
		/// <param name="Y1">Y1 position in the PDF document</param>		
		/// <param name="newWidth">Line's size</param>
		/// /// <param name="newStyle">Line's style</param>
		public lineElement(int X, int Y, int X1, int Y1, int newWidth, predefinedLineStyle newStyle) 
		{
			_coordX = X;
			_coordY = Y;
			_coordX1 = X1;
			_coordY1 = Y1;
			_lineStyle = new pdfLineStyle(newWidth, newStyle);
		}

		/// <summary>
		/// Class's constructor [DEPRECATED]
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="X1">X1 position in the PDF document</param>
		/// <param name="Y1">Y1 position in the PDF document</param>	
		/// <param name="newColor">Line's color</param>
		public lineElement(int X, int Y, int X1, int Y1, predefinedColor newColor)
		{
			_coordX = X;
			_coordY = Y;
			_coordX1 = X1;
			_coordY1 = Y1;
			_strokeColor = new pdfColor(newColor);
			_lineStyle = new pdfLineStyle(1,predefinedLineStyle.csNormal);
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="X1">X1 position in the PDF document</param>
		/// <param name="Y1">Y1 position in the PDF document</param>	
		/// <param name="newColor">Line's color</param>
		public lineElement(int X, int Y, int X1, int Y1, pdfColor newColor)
		{
			_coordX = X;
			_coordY = Y;
			_coordX1 = X1;
			_coordY1 = Y1;
			_strokeColor = newColor;
			_lineStyle = new pdfLineStyle(1,predefinedLineStyle.csNormal);
		}

		/// <summary>
		/// Class's constructor [DEPRECATED]
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="X1">X1 position in the PDF document</param>
		/// <param name="Y1">Y1 position in the PDF document</param>	
		/// <param name="newStyle">Line's style</param>
		/// <param name="newColor">Line's color</param>
		public lineElement(int X, int Y, int X1, int Y1, predefinedLineStyle newStyle, predefinedColor newColor)
		{
			_coordX = X;
			_coordY = Y;
			_coordX1 = X1;
			_coordY1 = Y1;
			_strokeColor = new pdfColor(newColor);
			_lineStyle = new pdfLineStyle(1, newStyle);
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="X1">X1 position in the PDF document</param>
		/// <param name="Y1">Y1 position in the PDF document</param>	
		/// <param name="newStyle">Line's style</param>
		/// <param name="newColor">Line's color</param>
		public lineElement(int X, int Y, int X1, int Y1, predefinedLineStyle newStyle, pdfColor newColor)
		{
			_coordX = X;
			_coordY = Y;
			_coordX1 = X1;
			_coordY1 = Y1;
			_strokeColor = newColor;
			_lineStyle = new pdfLineStyle(1, newStyle);
		}

		/// <summary>
		/// Class's constructor [DEPRECATED]
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="X1">X1 position in the PDF document</param>
		/// <param name="Y1">Y1 position in the PDF document</param>	
		/// <param name="newWidth">Line's width</param>
		/// <param name="newColor">Line's color</param>
		public lineElement(int X, int Y, int X1, int Y1, int newWidth, predefinedColor newColor)
		{
			_coordX = X;
			_coordY = Y;
			_coordX1 = X1;
			_coordY1 = Y1;
			_strokeColor = new pdfColor(newColor);
			_lineStyle = new pdfLineStyle(newWidth, predefinedLineStyle.csNormal);
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="X1">X1 position in the PDF document</param>
		/// <param name="Y1">Y1 position in the PDF document</param>	
		/// <param name="newWidth">Line's width</param>
		/// <param name="newColor">Line's color</param>
		public lineElement(int X, int Y, int X1, int Y1, int newWidth, pdfColor newColor)
		{
			_coordX = X;
			_coordY = Y;
			_coordX1 = X1;
			_coordY1 = Y1;
			_strokeColor = newColor;
			_lineStyle = new pdfLineStyle(newWidth, predefinedLineStyle.csNormal);
		}

		/// <summary>
		/// Class's constructor [DEPRECATED]
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="X1">X1 position in the PDF document</param>
		/// <param name="Y1">Y1 position in the PDF document</param>	
		/// <param name="newWidth">Line's size</param>
		/// <param name="newStyle">Line's style</param>
		/// <param name="newColor">Line's color</param>
		public lineElement(int X, int Y, int X1, int Y1, int newWidth, predefinedLineStyle newStyle, predefinedColor newColor)
		{
			_coordX = X;
			_coordY = Y;
			_coordX1 = X1;
			_coordY1 = Y1;
			_strokeColor = new pdfColor(newColor);
			_lineStyle = new pdfLineStyle(newWidth, newStyle);
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="X1">X1 position in the PDF document</param>
		/// <param name="Y1">Y1 position in the PDF document</param>	
		/// <param name="newWidth">Line's size</param>
		/// <param name="newStyle">Line's style</param>
		/// <param name="newColor">Line's color</param>
		public lineElement(int X, int Y, int X1, int Y1, int newWidth, predefinedLineStyle newStyle, pdfColor newColor)
		{
			_coordX = X;
			_coordY = Y;
			_coordX1 = X1;
			_coordY1 = Y1;
			_strokeColor = newColor;
			_lineStyle = new pdfLineStyle(newWidth, newStyle);
		}

		/// <summary>
		/// Method that returns the PDF codes to write the line in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public override string getText()
		{
			StringBuilder resultLine = new StringBuilder();
			StringBuilder lineContent = new StringBuilder();
			if (_strokeColor.isColor()) 
			{
				lineContent.Append(_strokeColor.rColor + " " + _strokeColor.gColor + " " + _strokeColor.bColor + " RG" + Convert.ToChar(13) + Convert.ToChar(10));
			}
			lineContent.Append("q" + Convert.ToChar(13) + Convert.ToChar(10));
			lineContent.Append(_lineStyle.getText() + Convert.ToChar(13) + Convert.ToChar(10));
			lineContent.Append(_coordX.ToString() + " " + _coordY.ToString() + " m" + Convert.ToChar(13) + Convert.ToChar(10));
			lineContent.Append(_coordX1.ToString() + " " + _coordY1.ToString() + " l" + Convert.ToChar(13) + Convert.ToChar(10));
			lineContent.Append("S" + Convert.ToChar(13) + Convert.ToChar(10));
			lineContent.Append("Q" + Convert.ToChar(13) + Convert.ToChar(10));
			resultLine.Append(_objectID.ToString() + " 0 obj" + Convert.ToChar(13) + Convert.ToChar(10));
			resultLine.Append("<<" + Convert.ToChar(13) + Convert.ToChar(10));
			resultLine.Append("/Length " + lineContent.Length.ToString() + Convert.ToChar(13) + Convert.ToChar(10));
			resultLine.Append(">>" + Convert.ToChar(13) + Convert.ToChar(10));
			resultLine.Append("stream" + Convert.ToChar(13) + Convert.ToChar(10));
			resultLine.Append(lineContent.ToString() + Convert.ToChar(13) + Convert.ToChar(10));
			resultLine.Append("endstream" + Convert.ToChar(13) + Convert.ToChar(10));
			resultLine.Append("endobj" + Convert.ToChar(13) + Convert.ToChar(10));
			lineContent = null;
			return  resultLine.ToString();			
		}


	}
}