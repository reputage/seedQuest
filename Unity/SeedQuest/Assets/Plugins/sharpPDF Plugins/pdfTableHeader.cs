using System;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements an abstract table's header object
	/// </summary>
	public class pdfTableHeader : pdfTableRow
	{
		/// <summary>
		/// Class's constructor
		/// </summary>
		internal pdfTableHeader()
		{			
		}		

		/// <summary>
		/// Method to add a template column to the table's header
		/// </summary>
		/// <param name="newColumn"></param>
		public void addColumn(pdfTableColumn newColumn)
		{
			if (Type.GetType("sharpPDF.pdfTableColumn").IsInstanceOfType(newColumn)) {
				_cols.Add(newColumn);
			} else {
				throw new pdfIncorrectColumnException();
			}			
		}
	}
}
