using System;
using System.Text;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a PDF header.
	/// </summary>
	internal class pdfHeader : IWritable
	{
		
		private int _objectIDHeader;
		private int _objectIDInfo;
		private int _objectIDOutlines;		
		private int _pageTreeID;
		private bool _openBookmark;

		/// <summary>
		/// Header's ID
		/// </summary>
		public int objectIDHeader
		{
			get
			{
				return _objectIDHeader;
			}

			set
			{
				_objectIDHeader = value;
			}
		}
	
		/// <summary>
		/// Outlines's ID
		/// </summary>
		public int objectIDOutlines
		{
			get
			{
				return _objectIDOutlines;
			}

			set
			{
				_objectIDOutlines = value;
			}
		}

		/// <summary>
		/// Info's ID
		/// </summary>
		public int objectIDInfo
		{
			get
			{
				return _objectIDInfo;
			}

			set
			{
				_objectIDInfo = value;
			}
		}

		/// <summary>
		/// PageTree's ID
		/// </summary>
		public int pageTreeID
		{
			get
			{
				return _pageTreeID;
			}

			set
			{
				_pageTreeID = value;
			}
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="openBookmark">Allows to show directly bookmarks near the document</param>
		public pdfHeader(bool openBookmark)
		{
			_openBookmark = openBookmark;
		}

		/// <summary>
		/// Method that returns the PDF codes to write the Header in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getText()
		{
			StringBuilder strHeader = new StringBuilder();
            strHeader.Append(_objectIDHeader.ToString() + " 0 obj" + Convert.ToChar(13) + Convert.ToChar(10));
            strHeader.Append("<<" + Convert.ToChar(13) + Convert.ToChar(10));
            strHeader.Append("/Type /Catalog" + Convert.ToChar(13) + Convert.ToChar(10));
			strHeader.Append("/Version /1.4" + Convert.ToChar(13) + Convert.ToChar(10));
            strHeader.Append("/Pages " + _pageTreeID.ToString() + " 0 R" + Convert.ToChar(13) + Convert.ToChar(10));
            strHeader.Append("/Outlines "+ _objectIDOutlines.ToString() +" 0 R" + Convert.ToChar(13) + Convert.ToChar(10));
			if (_openBookmark) {
				strHeader.Append("/PageMode /UseOutlines" + Convert.ToChar(13) + Convert.ToChar(10));
			}
            strHeader.Append(">>" + Convert.ToChar(13) + Convert.ToChar(10));
            strHeader.Append("endobj" + Convert.ToChar(13) + Convert.ToChar(10));                                   
			return strHeader.ToString();
		}
	}
}
