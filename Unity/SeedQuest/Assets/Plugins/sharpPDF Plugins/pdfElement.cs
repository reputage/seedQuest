using System;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a generic PDF element.
	/// </summary>
	internal abstract class pdfElement : IWritable
	{

		/// <summary>
		/// Element's color
		/// </summary>
		protected pdfColor _strokeColor = new pdfColor(predefinedColor.csNoColor);
		/// <summary>
		/// Border's color
		/// </summary>
		protected pdfColor _fillColor = new pdfColor(predefinedColor.csNoColor);
		/// <summary>
		/// X position of the element
		/// </summary>
		protected int _coordX;
		/// <summary>
		/// Y position of the element
		/// </summary>
		protected int _coordY;
		/// <summary>
		/// Element's ID
		/// </summary>
		protected int _objectID;

		/// <summary>
		/// Border's color
		/// </summary>
		public pdfColor strokeColor 
		{
			get
			{
				return _strokeColor;
			}

			set
			{
				_strokeColor = value;
			}
		}

		/// <summary>
		/// Element's Color
		/// </summary>
		public pdfColor fillColor 
		{
			get
			{
				return _fillColor;
			}

			set
			{
				_fillColor = value;
			}
		}

		/// <summary>
		/// X position in the PDF document
		/// </summary>
		public int coordX
		{
			get
			{
				return _coordX;
			}
			set
			{
				_coordX = value;
			}
		}

		/// <summary>
		/// Y position in the PDF document
		/// </summary>
		public int coordY
		{
			get
			{
				return _coordY;
			}
			set
			{
				_coordY = value;
			}
		}

		/// <summary>
		/// Element's ID
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
		/// Method that returns the PDF codes to write the generic element in the document. It must be implemented by the derived class
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public abstract string getText();

	}
}
