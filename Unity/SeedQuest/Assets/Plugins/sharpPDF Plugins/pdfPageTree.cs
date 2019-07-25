using System;
using System.Text;
using System.Collections;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a PDF pageTree.
	/// </summary>
	internal class pdfPageTree : IWritable
	{
	
		private ArrayList _pages;
		private int _pageCount;
		private int _objectID;

		/// <summary>
		/// Pagetree's ID
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
		public pdfPageTree()
		{
			_pageCount = 0;
			_pages = new ArrayList();
		}

		/// <summary>
		/// Method that adds a page to the pageTree object
		/// </summary>
		/// <param name="pageID"></param>
		public void addPage(int pageID)
		{
			_pages.Add(pageID);
			_pageCount++;
		}

		/// <summary>
		/// Method that returns the PDF codes to write the pageTree in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getText()
		{
			if (_pages.Count > 0)
			{
				StringBuilder content = new StringBuilder();				
				content.Append(_objectID.ToString() + " 0 obj" + Convert.ToChar(13) + Convert.ToChar(10));
                content.Append("<<" + Convert.ToChar(13) + Convert.ToChar(10));
                content.Append("/Type /Pages" + Convert.ToChar(13) + Convert.ToChar(10));
                content.Append("/Count " + _pages.Count.ToString() + Convert.ToChar(13) + Convert.ToChar(10));
				content.Append("/Kids [");
				foreach( int item in _pages)
				{
					content.Append(item.ToString() + " 0 R ");
				}
				content.Append("]" + Convert.ToChar(13) + Convert.ToChar(10));
                content.Append(">>"  + Convert.ToChar(13) + Convert.ToChar(10));                
                content.Append("endobj" + Convert.ToChar(13) + Convert.ToChar(10));
                return content.ToString();                
			} else {
				return null;
			}	
	}

	}
}
