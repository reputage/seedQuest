using System;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements an abstract table's column object
	/// </summary>
	public class pdfTableColumn : IWritable
	{

		private string _columnValue = "";
		private predefinedAlignment _columnAlign = predefinedAlignment.csLeft;
		private int _columnSize;

		/// <summary>
		/// Column's content
		/// </summary>
		public string columnValue
		{			
			get
			{
				return _columnValue;
			}
			set
			{
				_columnValue = value;
			}
		}

		/// <summary>
		/// Column's text align
		/// </summary>
		public predefinedAlignment columnAlign
		{
			get
			{
				return _columnAlign;
			}
			set
			{
				_columnAlign = value;
			}
		}

		/// <summary>
		/// Column's size
		/// </summary>
		public int columnSize
		{
			get
			{
				return _columnSize;
			}
			set
			{
				_columnSize = value;
			}
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		public pdfTableColumn()
		{			
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="columnValue">Text value of the column</param>
		/// <param name="columnAlign">Alignment of the column</param>
		/// <param name="columnSize">Size of the column</param>
		public pdfTableColumn(string columnValue, predefinedAlignment columnAlign, int columnSize)
		{
			_columnValue = columnValue;
			_columnAlign = columnAlign;
			_columnSize = columnSize;
		}

		/// <summary>
		/// Method that returns the PDF codes to write the tableColumn in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getText() {
			return null;
		}

	}
}
