using System;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a PDF color.
	/// </summary>
	public class pdfColor
	{
		private string _rColor;
		private string _gColor;
		private string _bColor;
		private predefinedColor _color = predefinedColor.csNoColor;

		/// <summary>
		/// R property of RGB color
		/// </summary>
		internal string rColor
		{
			get
			{
				return _rColor;
			}
		}

		/// <summary>
		/// G property of RGB color
		/// </summary>
		internal string gColor
		{
			get
			{
				return _gColor;
			}
		}

		/// <summary>
		/// B property of RGB color
		/// </summary>
		internal string bColor
		{
			get
			{
				return _bColor;
			}				
		}

		/// <summary>
		/// The predefinedColor of the pdfColor
		/// </summary>
		internal predefinedColor baseColor
		{
			get
			{
				return _color;
			}
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="newColor">Color</param>
		public pdfColor(predefinedColor newColor)
		{
			switch (newColor)
			{
				case predefinedColor.csNoColor:
					_rColor = "";
					_gColor = "";
					_bColor = "";
					break;
				case predefinedColor.csBlack:
					_rColor = "0";
					_gColor = "0";
					_bColor = "0";
					break;
				case predefinedColor.csWhite:
					_rColor = "1";
					_gColor = "1";
					_bColor = "1";
					break;
				case predefinedColor.csRed:
					_rColor = "1";
					_gColor = "0";
					_bColor = "0";
					break;
				case predefinedColor.csLightRed:
					_rColor = "1";
					_gColor = ".75";
					_bColor = ".75";
					break;
				case predefinedColor.csDarkRed:
					_rColor = ".5";
					_gColor = "0";
					_bColor = "0";
					break;
				case predefinedColor.csOrange:
					_rColor = "1";
					_gColor = ".5";
					_bColor = "0";
					break;
				case predefinedColor.csLightOrange:
					_rColor = "1";
					_gColor = ".75";
					_bColor = "0";
					break;
				case predefinedColor.csDarkOrange:
					_rColor = "1";
					_gColor = ".25";
					_bColor = "0";
					break;
				case predefinedColor.csYellow:
					_rColor = "1";
					_gColor = "1";
					_bColor = ".25";
					break;
				case predefinedColor.csLightYellow:
					_rColor = "1";
					_gColor = "1";
					_bColor = ".75";
					break;
				case predefinedColor.csDarkYellow:
					_rColor = "1";
					_gColor = "1";
					_bColor = "0";
					break;
				case predefinedColor.csBlue:
					_rColor = "0";
					_gColor = "0";
					_bColor = "1";
					break;
				case predefinedColor.csLightBlue:
					_rColor = ".1";
					_gColor = ".3";
					_bColor = ".75";
					break;
				case predefinedColor.csDarkBlue:
					_rColor = "0";
					_gColor = "0";
					_bColor = ".5";
					break;
				case predefinedColor.csGreen:
					_rColor = "0";
					_gColor = "1";
					_bColor = "0";
					break;
				case predefinedColor.csLightGreen:
					_rColor = ".75";
					_gColor = "1";
					_bColor = ".75";
					break;
				case predefinedColor.csDarkGreen:
					_rColor = "0";
					_gColor = ".5";
					_bColor = "0";
					break;
				case predefinedColor.csCyan:
					_rColor = "0";
					_gColor = ".5";
					_bColor = "1";
					break;
				case predefinedColor.csLightCyan:
					_rColor = ".2";
					_gColor = ".8";
					_bColor = "1";
					break;
				case predefinedColor.csDarkCyan:
					_rColor = "0";
					_gColor = ".4";
					_bColor = ".8";
					break;
				case predefinedColor.csPurple:
					_rColor = ".5";
					_gColor = "0";
					_bColor = "1";
					break;
				case predefinedColor.csLightPurple:
					_rColor = ".75";
					_gColor = ".45";
					_bColor = ".95";
					break;
				case predefinedColor.csDarkPurple:
					_rColor = ".4";
					_gColor = ".1";
					_bColor = ".5";
					break;
				case predefinedColor.csGray:
					_rColor = ".5";
					_gColor = ".5";
					_bColor = ".5";
					break;
				case predefinedColor.csLightGray:
					_rColor = ".75";
					_gColor = ".75";
					_bColor = ".75";
					break;
				case predefinedColor.csDarkGray:					
					_rColor = ".25";
					_gColor = ".25";
					_bColor = ".25";
					break;
			}
			_color = newColor;
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="HEXColor">Hex Color</param>
		public pdfColor(string HEXColor) 
		{
			_rColor = formatColorComponent(int.Parse(HEXColor.Substring(0,2), System.Globalization.NumberStyles.HexNumber));
			_gColor = formatColorComponent(int.Parse(HEXColor.Substring(2,2), System.Globalization.NumberStyles.HexNumber));
			_bColor = formatColorComponent(int.Parse(HEXColor.Substring(4,2), System.Globalization.NumberStyles.HexNumber));
			_color = predefinedColor.csUserColor;
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="rColor">Red component of the color</param>
		/// <param name="gColor">Green component of the color</param>
		/// <param name="bColor">Blue component of the color</param>
		public pdfColor(int rColor, int gColor, int bColor)
		{
			_rColor = formatColorComponent(rColor);
			_gColor = formatColorComponent(gColor);
			_bColor = formatColorComponent(bColor);
			_color = predefinedColor.csUserColor;
		}

		/// <summary>
		/// Method that formats a int color value with the pdf color format
		/// </summary>
		/// <param name="colorValue">Component of the color</param>
		/// <returns>Formatted component of the color</returns>
		private string formatColorComponent(int colorValue)
		{
			int colorComponent;
			colorComponent = Convert.ToInt32(((Convert.ToSingle(colorValue) / 255) * 100));
			string resultValue;
			if (colorComponent > 100) 
			{
				colorComponent = 100;
			} 
			else 
			{
				if (colorComponent < 0) 
				{
					colorComponent = 0;
				}
			}
			if (colorComponent < 100) 
			{				
				resultValue = "." + colorComponent.ToString();
				if (resultValue[resultValue.Length - 1] == '0') 
				{
					resultValue = resultValue.Substring(0,resultValue.Length - 1);
				}
			} 
			else 
			{
				resultValue = "1";
			}
			return resultValue;
		}

		/// <summary>
		/// Method that validates the color
		/// </summary>
		/// <returns>Boolean value that represents the validity of the color</returns>
		internal bool isColor()
		{
			return (_color != predefinedColor.csNoColor);
		}
	}
}
