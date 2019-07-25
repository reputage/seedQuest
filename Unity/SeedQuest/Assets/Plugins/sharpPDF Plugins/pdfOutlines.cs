using System;
using System.Text;
using System.Collections;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a PDF Outlines.
	/// </summary>
	internal class pdfOutlines : IWritable
	{
		
		private int _objectIDOutlines;
		private int _childIDFirst = 0;
		private int _childIDLast = 0;
		private int _childCount = 0;		
		private ArrayList _BookmarkRoot = new ArrayList();

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
		/// Class's constructor
		/// </summary>
		public pdfOutlines()
		{
			
		}

		/// <summary>
		/// Method that initialize
		/// </summary>
		/// <param name="counterID">Initial Object ID</param>
		/// <returns>Updated Object ID</returns>
		public int initializeOutlines(int counterID)
		{
			if (_BookmarkRoot.Count > 0)
			{		
				initializeBookmarks(counterID,_BookmarkRoot, _objectIDOutlines);	
				_childIDFirst = ((pdfBookmarkNode)_BookmarkRoot[0]).ObjectID;
				_childIDLast = ((pdfBookmarkNode)_BookmarkRoot[_BookmarkRoot.Count - 1]).ObjectID;
				counterID += _childCount;
			} else {
				_childCount = 0;
				_childIDFirst = 0;
				_childIDLast = 0;
			}
			return counterID;
		}

		/// <summary>
		/// Method that adds a bookmark to the outlines object
		/// </summary>
		/// <param name="Bookmark">BookmarkNode Object</param>
		public void addBookmark(pdfBookmarkNode Bookmark)
		{
			_BookmarkRoot.Add(Bookmark);
		}

		/// <summary>
		/// Method that initialize all bookmarks
		/// </summary>
		/// <param name="CounterID">Initial Object ID</param>
		/// <param name="StartPoint">ArrayList of BookmarkNodes of the same level</param>
		/// <param name="FatherID">Object ID of the father</param>
		/// <returns>Number of childs</returns>
		private int initializeBookmarks(int CounterID, ArrayList StartPoint, int FatherID)
		{
			int currentNodes = 0;
			if (StartPoint.Count > 0) {
				pdfBookmarkNode Node;
				for (int i = 0; i < StartPoint.Count; i++)
				{
					Node = (pdfBookmarkNode)StartPoint[i];				
					Node.ObjectID = CounterID + _childCount;				
					Node.parent = FatherID;									
					currentNodes++;
					_childCount++;
					Node.childCount = initializeBookmarks(CounterID,  Node.Childs, Node.ObjectID);					
					if (Node.childCount > 0) {
						Node.first = Node.getFirstChild().ObjectID;
						Node.last = Node.getLastChild().ObjectID;	
					}
					if (StartPoint.Count > 1) {
						if (i > 0) {
							Node.prev = ((pdfBookmarkNode)StartPoint[i - 1]).ObjectID;
						}
						if (i < (StartPoint.Count - 1)) {
							Node.next = CounterID + _childCount;;
						}
					}
					currentNodes += Node.childCount;
				}
			}
			return currentNodes;
		}

		/// <summary>
		/// Method that returns the PDF codes to write the Outlines in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getText() 
		{
			StringBuilder strOutlines = new StringBuilder();	
			strOutlines.Append(_objectIDOutlines.ToString() + " 0 obj" + Convert.ToChar(13) + Convert.ToChar(10));
            strOutlines.Append("<<" + Convert.ToChar(13) + Convert.ToChar(10));
            strOutlines.Append("/Type /Outlines" + Convert.ToChar(13) + Convert.ToChar(10));
			if (_childCount != 0) {
				strOutlines.Append("/First "+ _childIDFirst.ToString() + " 0 R" + Convert.ToChar(13) + Convert.ToChar(10));
				strOutlines.Append("/Last "+ _childIDLast.ToString() + " 0 R" + Convert.ToChar(13) + Convert.ToChar(10));
				strOutlines.Append("/Count "+ _childCount.ToString() + Convert.ToChar(13) + Convert.ToChar(10));
			} else {
				strOutlines.Append("/Count 0" + Convert.ToChar(13) + Convert.ToChar(10));
			}
            strOutlines.Append(">>" + Convert.ToChar(13) + Convert.ToChar(10));
            strOutlines.Append("endobj" + Convert.ToChar(13) + Convert.ToChar(10));			
			return strOutlines.ToString();
		}

		/// <summary>
		/// Method that returns all nodes from a start collection
		/// </summary>
		/// <param name="StartPoint">ArrayList with the start point of pdfBookmarkNodes</param>
		/// <returns>Collection of all pdfBookmarkNodes from the start point</returns>
		private	ArrayList getNodes(ArrayList StartPoint)
		{
			ArrayList resultList = new ArrayList();
			if (StartPoint.Count > 0) {
				resultList.AddRange(StartPoint);
				foreach (pdfBookmarkNode Node in StartPoint)
				{
					resultList.AddRange(getNodes(Node.Childs));
				}
			}						
			return resultList;
		}

		/// <summary>
		/// Method that returns a sorted(by objectID) collection of pdfBookmarkNodes
		/// </summary>
		/// <returns>Sorted bookmark collection</returns>
		public ArrayList getBookmarks()
		{
			ArrayList returnedList = getNodes(_BookmarkRoot);
			returnedList.Sort();
			return returnedList;
		}
		
	}
}
