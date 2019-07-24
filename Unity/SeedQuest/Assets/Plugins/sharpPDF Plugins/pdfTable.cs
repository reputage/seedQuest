using System;
using System.Collections;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements an abstract table object
	/// </summary>
	public class pdfTable : IEnumerable
	{

		private pdfTableHeader _tableHeader;
		private pdfTableRowStyle _tableHeaderStyle;
		private pdfTableRowStyle _rowStyle;
		private pdfTableRowStyle _alternateRowStyle;
		private ArrayList _rows = new ArrayList();
		private int _borderSize;
		private pdfColor _borderColor;
		private int _cellpadding;

		/// <summary>
		/// Border's Size
		/// </summary>
		public int borderSize
		{
			get
			{
				return _borderSize;
			}
			set
			{
				_borderSize = value;
			}
		}

		/// <summary>
		/// Border's color
		/// </summary>
		public pdfColor borderColor
		{
			get
			{
				return _borderColor;
			}
			set
			{
				_borderColor = value;
			}
		}

		/// <summary>
		/// Table's header
		/// </summary>
		public pdfTableHeader tableHeader
		{
			get
			{
				return _tableHeader;
			}
		}

		/// <summary>
		/// The style of the table's header
		/// </summary>
		public pdfTableRowStyle tableHeaderStyle
		{
			get
			{
				return _tableHeaderStyle;
			}
			set
			{
				_tableHeaderStyle = value;
			}
		}

		/// <summary>
		/// The style of a table's row
		/// </summary>
		public pdfTableRowStyle rowStyle
		{
			get
			{
				return _rowStyle;
			}
			set
			{
				_rowStyle = value;
			}
		}

		/// <summary>
		/// The alternate style of a table's row
		/// </summary>
		public pdfTableRowStyle alternateRowStyle
		{
			get
			{
				return _alternateRowStyle;
			}
			set
			{
				_alternateRowStyle = value;
			}
		}

		/// <summary>
		/// The cellpadding of the table
		/// </summary>
		public int cellpadding
		{
			get
			{
				return _cellpadding;
			}
			set
			{
				_cellpadding = value;
			}
		}

		/// <summary>
		/// Class's Constructor
		/// </summary>
		public pdfTable()
		{			
			_tableHeader = new pdfTableHeader();
			_tableHeaderStyle = new pdfTableRowStyle(predefinedFont.csHelvetica, 10, new pdfColor(predefinedColor.csBlack), new pdfColor(predefinedColor.csWhite));
			_rowStyle = new  pdfTableRowStyle(predefinedFont.csHelvetica, 10, new pdfColor(predefinedColor.csBlack), new pdfColor(predefinedColor.csWhite));
			_alternateRowStyle = null;
			_borderSize = 1;
			_borderColor = new pdfColor(predefinedColor.csBlack);
			_cellpadding = 5;
		}

		/// <summary>
		/// Table's rows
		/// </summary>
		public pdfTableRow this[int index]
		{
			get
			{
				return (pdfTableRow)_rows[index];
			}
		}
		
		/// <summary>
		/// The number of rows
		/// </summary>
		public int rowsCount
		{
			get
			{
				return _rows.Count;
			}
		}

		/// <summary>
		/// Method that creates a new row
		/// </summary>
		/// <returns>A new pdfTableRowObject</returns>
		public pdfTableRow createRow()
		{
			return new pdfTableRow(_tableHeader);
		}

		/// <summary>
		/// Method to add a new row into the table
		/// </summary>
		/// <param name="newRow">New row</param>
		public void addRow(pdfTableRow newRow)
		{
			if (Type.GetType("sharpPDF.pdfTableRow").IsInstanceOfType(newRow)) {
				_rows.Add(newRow);
			} else {
				throw new pdfIncorrectRowException();
			}	
			
		}

		/// <summary>
		/// Enumerator for table's rows
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return _rows.GetEnumerator();
		}

	}
}
