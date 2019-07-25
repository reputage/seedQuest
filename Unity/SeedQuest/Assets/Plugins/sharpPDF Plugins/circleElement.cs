using System;
using System.Text;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a PDF circle.
	/// </summary>
	internal class circleElement : pdfElement
	{
		
		private int _ray;
		private pdfLineStyle _lineStyle;

		/// <summary>
		/// Class's constructor [DEPRECATED]
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="Ray">Ray of the circle</param>
		/// <param name="strokeColor">Color of circle's border</param>
		/// <param name="fillColor">Color of the circle</param>
		public circleElement(int X, int Y, int Ray, predefinedColor strokeColor, predefinedColor fillColor)
		{
			_coordX = X;
            _coordY = Y;
            _ray = Ray;
            _strokeColor = new pdfColor(strokeColor);
            _fillColor = new pdfColor(fillColor);
			_lineStyle = new pdfLineStyle(1, predefinedLineStyle.csNormal);
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="Ray">Ray of the circle</param>
		/// <param name="strokeColor">Color of circle's border</param>
		/// <param name="fillColor">Color of the circle</param>
		public circleElement(int X, int Y, int Ray, pdfColor strokeColor, pdfColor fillColor)
		{
			_coordX = X;
			_coordY = Y;
			_ray = Ray;
			_strokeColor = strokeColor;
			_fillColor = fillColor;
			_lineStyle = new pdfLineStyle(1, predefinedLineStyle.csNormal);
		}

		/// <summary>
		/// Class's constructor [DEPRECATED]
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="Ray">Ray of the circle</param>
		/// <param name="strokeColor">Color of circle's border</param>
		/// <param name="fillColor">Color of the circle</param>
		/// <param name="newWidth">Border's size</param>
		public circleElement(int X, int Y, int Ray, predefinedColor strokeColor, predefinedColor fillColor, int newWidth)
		{
			_coordX = X;
			_coordY = Y;
			_ray = Ray;
			_strokeColor = new pdfColor(strokeColor);
			_fillColor = new pdfColor(fillColor);
			_lineStyle = new pdfLineStyle(newWidth, predefinedLineStyle.csNormal);
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="Ray">Ray of the circle</param>
		/// <param name="strokeColor">Color of circle's border</param>
		/// <param name="fillColor">Color of the circle</param>
		/// <param name="newWidth">Border's size</param>
		public circleElement(int X, int Y, int Ray, pdfColor strokeColor, pdfColor fillColor, int newWidth)
		{
			_coordX = X;
			_coordY = Y;
			_ray = Ray;
			_strokeColor = strokeColor;
			_fillColor = fillColor;
			_lineStyle = new pdfLineStyle(newWidth, predefinedLineStyle.csNormal);
		}

		/// <summary>
		/// Class's constructor [DEPRECATED]
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="Ray">Ray of the circle</param>
		/// <param name="strokeColor">Color of circle's border</param>
		/// <param name="fillColor">Color of the circle</param>
		/// <param name="newStyle">Border's style</param>
		public circleElement(int X, int Y, int Ray, predefinedColor strokeColor, predefinedColor fillColor, predefinedLineStyle newStyle)
		{
			_coordX = X;
			_coordY = Y;
			_ray = Ray;
			_strokeColor = new pdfColor(strokeColor);
			_fillColor = new pdfColor(fillColor);
			_lineStyle = new pdfLineStyle(1, newStyle);
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="Ray">Ray of the circle</param>
		/// <param name="strokeColor">Color of circle's border</param>
		/// <param name="fillColor">Color of the circle</param>
		/// <param name="newStyle">Border's style</param>
		public circleElement(int X, int Y, int Ray, pdfColor strokeColor, pdfColor fillColor, predefinedLineStyle newStyle)
		{
			_coordX = X;
			_coordY = Y;
			_ray = Ray;
			_strokeColor = strokeColor;
			_fillColor = fillColor;
			_lineStyle = new pdfLineStyle(1, newStyle);
		}

		/// <summary>
		/// Class's constructor [DEPRECATED]
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="Ray">Ray of the circle</param>
		/// <param name="strokeColor">Color of circle's border</param>
		/// <param name="fillColor">Color of the circle</param>
		/// <param name="newWidth">Border's size</param>
		/// <param name="newStyle">Border's style</param>
		public circleElement(int X, int Y, int Ray, predefinedColor strokeColor, predefinedColor fillColor, int newWidth, predefinedLineStyle newStyle)
		{
			_coordX = X;
			_coordY = Y;
			_ray = Ray;
			_strokeColor = new pdfColor(strokeColor);
			_fillColor = new pdfColor(fillColor);
			_lineStyle = new pdfLineStyle(newWidth, newStyle);
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="X">X position in the PDF document</param>
		/// <param name="Y">Y position in the PDF document</param>
		/// <param name="Ray">Ray of the circle</param>
		/// <param name="strokeColor">Color of circle's border</param>
		/// <param name="fillColor">Color of the circle</param>
		/// <param name="newWidth">Border's size</param>
		/// <param name="newStyle">Border's style</param>
		public circleElement(int X, int Y, int Ray, pdfColor strokeColor, pdfColor fillColor, int newWidth, predefinedLineStyle newStyle)
		{
			_coordX = X;
			_coordY = Y;
			_ray = Ray;
			_strokeColor = strokeColor;
			_fillColor = fillColor;
			_lineStyle = new pdfLineStyle(newWidth, newStyle);
		}

		/// <summary>
		/// Method that returns the PDF codes to write the circle in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public override string getText()
		{
			StringBuilder resultCircle = new StringBuilder();
            StringBuilder circleContent = new StringBuilder();
            circleContent.Append("q"+ Convert.ToChar(13) + Convert.ToChar(10));
			if (_strokeColor.isColor())
			{
				circleContent.Append(_strokeColor.rColor + " " + _strokeColor.gColor + " " + _strokeColor.bColor + " RG"+ Convert.ToChar(13) + Convert.ToChar(10));
			}
			if (_fillColor.isColor()) 
			{
				circleContent.Append(_fillColor.rColor + " " + _fillColor.gColor + " " + _fillColor.bColor + " rg"+ Convert.ToChar(13) + Convert.ToChar(10));
			}
            circleContent.Append(_lineStyle.getText() + Convert.ToChar(13) + Convert.ToChar(10));
            circleContent.Append((_coordX - _ray).ToString() + " " + _coordY.ToString() + " m" + Convert.ToChar(13) + Convert.ToChar(10));
            circleContent.Append((_coordX - _ray).ToString() + " " + getYBezier(_coordY, _ray, '+') + " " + (_coordX + _ray).ToString() + " " + getYBezier(_coordY, _ray, '+') + " " + (_coordX + _ray).ToString() + " " + _coordY.ToString() + " c" + Convert.ToChar(13) + Convert.ToChar(10));
            circleContent.Append((_coordX - _ray).ToString() + " " + _coordY.ToString() + " m" + Convert.ToChar(13) + Convert.ToChar(10));
            circleContent.Append((_coordX - _ray).ToString() + " " + getYBezier(_coordY, _ray, '-') + " " + (_coordX + _ray).ToString() + " " + getYBezier(_coordY, _ray, '-') + " " + (_coordX + _ray).ToString() + " " + _coordY.ToString() + " c" + Convert.ToChar(13) + Convert.ToChar(10));
            circleContent.Append("B" + Convert.ToChar(13) + Convert.ToChar(10));
            circleContent.Append("Q" + Convert.ToChar(13) + Convert.ToChar(10));
            resultCircle.Append(_objectID.ToString() + " 0 obj" + Convert.ToChar(13) + Convert.ToChar(10));
            resultCircle.Append("<<" + Convert.ToChar(13) + Convert.ToChar(10));
            resultCircle.Append("/Length " + circleContent.Length.ToString() + Convert.ToChar(13) + Convert.ToChar(10));
            resultCircle.Append(">>" + Convert.ToChar(13) + Convert.ToChar(10));
            resultCircle.Append("stream" + Convert.ToChar(13) + Convert.ToChar(10));
            resultCircle.Append(circleContent.ToString() + Convert.ToChar(13) + Convert.ToChar(10));
            resultCircle.Append("endstream" + Convert.ToChar(13) + Convert.ToChar(10));
            resultCircle.Append("endobj" + Convert.ToChar(13) + Convert.ToChar(10));
			circleContent = null;
			return resultCircle.ToString();            
		}


		/// <summary>
		/// Private method that returns the Y position with the Bezier's function
		/// </summary>
		/// <param name="coordY">Y position of the circle's center</param>
		/// <param name="ray">Circle's ray</param>
		/// <param name="operation">Type of operation : '+' or '-'</param>
		/// <returns>String that contains Y position</returns>
		private string getYBezier(int coordY, int ray, char operation)
		{
			double result = 0;
			switch (operation)
			{
				case '+':
					result = Convert.ToDouble(coordY) + (Convert.ToDouble(ray) * 1.414);
					break;
				case '-':
					result = Convert.ToDouble(coordY) - (Convert.ToDouble(ray) * 1.414);
					break;
			}
            return Convert.ToInt32(result).ToString();
		}
	}
}

