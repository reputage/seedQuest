using System;
using System.Text;
using System.Collections;

using sharpPDF.Enumerators;
using sharpPDF.Exceptions;

namespace sharpPDF
{
	/// <summary>
	/// Class that representa a single bookmark element
	/// </summary>
	public class pdfBookmarkNode : IWritable , IComparable
	{

		/// <summary>
		/// Method that allows to compare pdfBookmarkNodes (Inherited from IComparable)
		/// </summary>
		/// <param name="obj">Object to compare</param>
		/// <returns>Compare result</returns>
		public int CompareTo(object obj)
		{
			if (obj is pdfBookmarkNode) {
				pdfBookmarkNode myComparableNode = (pdfBookmarkNode)obj;
				return _ObjectID.CompareTo(myComparableNode.ObjectID);
			} else {
				throw new ArgumentException("Object is not a pdfBookmarkNode");
			}
		}	
	

		private string _Title;
		private pdfPage _Page;
		private IPdfDestination _Destination;
		private bool _open;
		private int _ObjectID;
		private int _prev;
		private int _next;
		private int _first;
		private int _last;
		private int _parent;
		private int _childCount;
		private ArrayList _Childs = new ArrayList();		

		/// <summary>
		/// Bookmark's title
		/// </summary>
		internal string Title
		{
			get
			{
				return _Title;
			}
		}

		/// <summary>
		/// Page's reference for the bookmark
		/// </summary>
		internal pdfPage Page
		{
			get
			{	
				return _Page;
			}
		}

		/// <summary>
		/// Destination of the bookmark
		/// </summary>
		internal IPdfDestination Destination
		{
			get
			{
				return _Destination;
			}
		}

		/// <summary>
		/// The visibility of bookmark's childs
		/// </summary>
		internal bool open
		{
			get
			{
				return _open;
			}
		}

		/// <summary>
		/// Bookmark's ID
		/// </summary>
		internal int ObjectID
		{
			get
			{
				return _ObjectID;
			}
			set
			{
				_ObjectID = value;
			}
		}

		/// <summary>
		/// Prev bookmark ID
		/// </summary>
		internal int prev
		{
			get
			{
				return _prev;
			}
			set
			{
				_prev = value;
			}
		}

		/// <summary>
		/// Next bookmark ID
		/// </summary>
		internal int next
		{
			get
			{
				return _next;
			}
			set
			{
				_next = value;
			}
		}

		/// <summary>
		/// First child ID
		/// </summary>
		internal int first
		{
			get
			{
				return _first;
			}
			set
			{
				_first = value;
			}
		}

		/// <summary>
		/// Last child ID
		/// </summary>
		internal int last
		{
			get
			{
				return _last;
			}
			set
			{
				_last = value;
			}
		}

		/// <summary>
		/// Bokkmark's partent ID
		/// </summary>
		internal int parent
		{
			get
			{
				return _parent;
			}
			set
			{
				_parent = value;
			}
		}

		/// <summary>
		/// Number of childs
		/// </summary>
		internal int childCount
		{
			get
			{
				return _childCount;
			}
			set
			{
				_childCount = value;
			}
		}

		/// <summary>
		/// Bookmark's childs
		/// </summary>
		internal ArrayList Childs
		{
			get
			{
				return _Childs;
			}
		}		

		/// <summary>
		/// Method that returns the first child
		/// </summary>
		/// <returns>Object that represent the first child</returns>
		internal pdfBookmarkNode getFirstChild()
		{
			if (_childCount > 0) {				
				return (pdfBookmarkNode)_Childs[0];
			} else {
				return null;
			}
		}

		/// <summary>
		/// Method that returns the last child
		/// </summary>
		/// <returns>Object that represent the last child</returns>
		internal pdfBookmarkNode getLastChild()
		{
			if (_childCount > 0) {				
				return (pdfBookmarkNode)_Childs[_Childs.Count - 1];
			} else {
				return null;
			}
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="Title">Bookmark's title</param>
		/// <param name="Page">Destination Page</param>
		/// <param name="openBookmark">The visibility of bookmark's childs</param>
		public pdfBookmarkNode(string Title, pdfPage Page, bool openBookmark)
		{
			_Title = Title;
			_Page = Page;
			_Destination = null;
			_prev = 0;
			_next = 0;
			_first = 0;
			_last = 0;
			_parent = 0;
			_childCount = 0;
			_open = openBookmark;
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="Title">Bookmark's title</param>
		/// <param name="Page">Destination Page</param>
		/// <param name="openBookmark">The visibility of bookmark's childs</param>
		/// <param name="Destination">Destination object</param>
		public pdfBookmarkNode(string Title, pdfPage Page, bool openBookmark, IPdfDestination Destination)
		{
			_Title = Title;
			_Page = Page;
			_Destination = Destination;
			_prev = 0;
			_next = 0;
			_first = 0;
			_last = 0;
			_parent = 0;
			_childCount = 0;
			_open = openBookmark;
		}

		/// <summary>
		/// Method that add a child to the bookmark
		/// </summary>
		/// <param name="Child">Child object</param>
		public void addChildNode(pdfBookmarkNode Child)
		{
			_Childs.Add(Child);
		}		

		/// <summary>
		/// Method that returns the PDF codes to write the object in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getText()
		{
			StringBuilder strBookmark = new StringBuilder();
			strBookmark.Append(_ObjectID.ToString() + " 0 obj" + Convert.ToChar(13) + Convert.ToChar(10));
			strBookmark.Append("<<" + Convert.ToChar(13) + Convert.ToChar(10));
			strBookmark.Append("/Title (" + textAdapter.checkText(_Title) + ")" + Convert.ToChar(13) + Convert.ToChar(10));				
			if (_prev > 0) {
				strBookmark.Append("/Prev "+ _prev.ToString() + " 0 R" + Convert.ToChar(13) + Convert.ToChar(10));
			}
			if (_next > 0) {
				strBookmark.Append("/Next "+ _next.ToString() + " 0 R" + Convert.ToChar(13) + Convert.ToChar(10));
			}
			if (_parent > 0) {
				strBookmark.Append("/Parent "+ _parent.ToString() + " 0 R" + Convert.ToChar(13) + Convert.ToChar(10));
			}
			if (_childCount > 0) {
				strBookmark.Append("/First "+ _first.ToString() + " 0 R" + Convert.ToChar(13) + Convert.ToChar(10));
				strBookmark.Append("/Last "+ _last.ToString() + " 0 R" + Convert.ToChar(13) + Convert.ToChar(10));				
				if (_open) {
					strBookmark.Append("/Count "+ _childCount.ToString() + Convert.ToChar(13) + Convert.ToChar(10));				
				}
			}			
			if (_Destination != null) {
				strBookmark.Append("/Dest [" + _Page.objectID.ToString() +" 0 R " + _Destination.getDestinationValue() + "]" + Convert.ToChar(13) + Convert.ToChar(10));
			}
			strBookmark.Append(">>" + Convert.ToChar(13) + Convert.ToChar(10));
			strBookmark.Append("endobj" + Convert.ToChar(13) + Convert.ToChar(10));
			return strBookmark.ToString();
		}


	}
}
