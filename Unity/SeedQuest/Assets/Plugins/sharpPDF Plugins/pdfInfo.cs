using System;
using System.Text;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a PDF info.
	/// </summary>
	internal class pdfInfo : IWritable
	{
		
		private int _objectIDInfo;
		private string _title;
		private string _author;

		/// <summary>
		/// Info'sID
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
		/// Class's constructor
		/// </summary>
		/// <param name="title">Document's title</param>
		/// <param name="author">Document's author</param>
		public pdfInfo(string title, string author)
		{
			_title = title;
			_author = author;
		}

		/// <summary>
		/// Method that returns the PDF codes to write the Info in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getText()
		{
			StringBuilder strInfo = new StringBuilder();						
			strInfo.Append(_objectIDInfo.ToString() + " 0 obj" + Convert.ToChar(13) + Convert.ToChar(10));
			strInfo.Append("<<" + Convert.ToChar(13) + Convert.ToChar(10));
			strInfo.Append("/Title (" + _title + ")" + Convert.ToChar(13) + Convert.ToChar(10));
			strInfo.Append("/Author (" + _author + ")" + Convert.ToChar(13) + Convert.ToChar(10));
			strInfo.Append("/Creator (sharpPDF)" + Convert.ToChar(13) + Convert.ToChar(10));
			strInfo.Append("/CreationDate (" + DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Day.ToString() + ")" + Convert.ToChar(13) + Convert.ToChar(10));			
			strInfo.Append(">>" + Convert.ToChar(13) + Convert.ToChar(10));
			strInfo.Append("endobj" + Convert.ToChar(13) + Convert.ToChar(10));
			return strInfo.ToString();
		}
		
	}
}
