using System;
using System.Collections;
using System.Text;
using UnityEngine;
//using System.Drawing;

using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a PDF page.
	/// </summary>
	public class pdfPage : IWritable
	{
	
		private int _height;
		private int _width;
		private int _objectID;
		private int _pageTreeID;
		private string _fontObjectsReference;
		private ArrayList _elements;

		/// <summary>
		/// Page's ID
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
		/// Page's height
		/// </summary>
		public int height
		{
			get
			{
				return _height;
			}
		}

		/// <summary>
		/// Page's width
		/// </summary>
		public int width
		{
			get
			{
				return _width;
			}
		}

		/// <summary>
		/// Page's elements
		/// </summary>
		internal ArrayList elements
		{
			get
			{
				return _elements;
			}
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		public pdfPage()
		{
			_height = 792;
            _width = 612;
			_elements = new ArrayList();
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="newHeight">Page's height</param>
		/// <param name="newWidth">Page's width</param>
		public pdfPage(int newHeight, int newWidth)
		{
			_height = newHeight;
            _width = newWidth;
			_elements = new ArrayList();
		}

		/// <summary>
		/// Class's distructor
		/// </summary>
		~pdfPage()
		{
			_elements = null;
		}

		/// <summary>
		/// Method that adds an image to the page object
		/// </summary>
		/// <param name="newImgSource">Image's name</param>
		/// <param name="X">X position of the image in the page</param>
		/// <param name="Y">Y position of the image in the page</param>		
		public void addImage(Byte[] myImage, int X, int Y, int imageHeight, int imageWidth)
		{
			try {
				imageElement objImage = new imageElement(myImage, X, Y, imageHeight, imageWidth );
				_elements.Add(objImage);
				objImage = null;
			} catch (pdfImageNotFoundException ex) {
				throw new pdfImageNotFoundException(ex.Message,ex);
			} catch (pdfImageIOException ex) {
				throw new pdfImageIOException(ex.Message,ex);
			}
		}
		
		public IEnumerator newAddImage ( string imageName, int X, int Y ) {
			WWW www = new WWW(imageName);
			yield return www;
			addImage(www.bytes,X,Y,www.texture.height,www.texture.width);
			Debug.Log ( "Adding Image Length "+www.bytes.Length+" HEIGTH :"+www.texture.height+" WIDTH :"+www.texture.width );
		}
		
		/// <summary>
		/// Method that adds an image to the page object
		/// </summary>
		/// <param name="newImgObject">Image Object</param>
		/// <param name="X">X position of the image in the page</param>
		/// <param name="Y">Y position of the image in the page</param>		
//		public void addImage( newImgObject, int X, int Y)
//		{
//			try {
//				imageElement objImage = new imageElement(newImgObject, X, Y);
//				_elements.Add(objImage);
//				objImage = null;
//			} catch (pdfImageNotFoundException ex) {
//				throw new pdfImageNotFoundException(ex.Message,ex);
//			} catch (pdfImageIOException ex) {
//				throw new pdfImageIOException(ex.Message,ex);
//			}
//		}

		/// <summary>
		/// Method that adds an image to the page object
		/// </summary>
		/// <param name="newImgSource">Image's name</param>
		/// <param name="X">X position of the image in the page</param>
		/// <param name="Y">Y position of the image in the page</param>
		/// <param name="height">New height of the image</param>
		/// <param name="width">New width of the image</param>
//		public void addImage(string newImgSource, int X, int Y, int height, int width)
//		{
//			try {
//				imageElement objImage = new imageElement(newImgSource, X, Y, height, width);
//				_elements.Add(objImage);
//				objImage = null;
//			} catch (pdfImageNotFoundException ex) {
//				throw new pdfImageNotFoundException(ex.Message,ex);
//			} catch (pdfImageIOException ex) {
//				throw new pdfImageIOException(ex.Message,ex);
//			}
//		}

		/// <summary>
		/// Method that adds an image to the page object
		/// </summary>
		/// <param name="newImgObject">Image Object</param>
		/// <param name="X">X position of the image in the page</param>
		/// <param name="Y">Y position of the image in the page</param>
		/// <param name="height">New height of the image</param>
		/// <param name="width">New width of the image</param>
//		public void addImage(Image newImgObject, int X, int Y, int height, int width)
//		{
//			try {
//				imageElement objImage = new imageElement(newImgObject, X, Y, height, width);
//				_elements.Add(objImage);
//				objImage = null;
//			} catch (pdfImageNotFoundException ex) {
//				throw new pdfImageNotFoundException(ex.Message,ex);
//			} catch (pdfImageIOException ex) {
//				throw new pdfImageIOException(ex.Message,ex);
//			}
//		}

		/// <summary>
		/// Method that adds a text element to the page object
		/// </summary>
		/// <param name="newText">Text</param>
		/// <param name="X">X position of the text in the page</param>
		/// <param name="Y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		public void addText(string newText, int X, int Y, predefinedFont fontType, int fontSize)
		{
			textElement objText = new textElement(newText, fontSize, fontType, X, Y);
			_elements.Add(objText);
			objText = null;
		}

		/// <summary>
		/// Method that adds a text element to the page object [DEPRECATED]
		/// </summary>
		/// <param name="newText">Text</param>
		/// <param name="X">X position of the text in the page</param>
		/// <param name="Y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="fontColor">Font's color</param>
		public void addText(string newText, int X, int Y, predefinedFont fontType, int fontSize, predefinedColor fontColor)
		{
			textElement objText = new textElement(newText, fontSize, fontType, X, Y, fontColor);
			_elements.Add(objText);
			objText = null;
		}

		/// <summary>
		/// Method that adds a text element to the page object
		/// </summary>
		/// <param name="newText">Text</param>
		/// <param name="X">X position of the text in the page</param>
		/// <param name="Y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="fontColor">Font's color</param>
		public void addText(string newText, int X, int Y, predefinedFont fontType, int fontSize, pdfColor fontColor)
		{
			textElement objText = new textElement(newText, fontSize, fontType, X, Y, fontColor);
			_elements.Add(objText);
			objText = null;
		}

		/// <summary>
		/// Method that adds a paragraph to the page object
		/// </summary>
		/// <param name="newText">Text</param>
		/// <param name="x">X position of the text in the page</param>
		/// <param name="y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="parWidth">Paragraph's width</param>
		public void addParagraph(string newText, int x, int y, predefinedFont fontType, int fontSize, int parWidth)
		{
			paragraphElement objParagraph = new paragraphElement(textAdapter.formatParagraph(newText, fontSize, fontType, parWidth), fontSize, fontType, x, y);
			_elements.Add(objParagraph);			
			objParagraph = null;
		}

		/// <summary>
		/// Method that adds a paragraph to the page object [DEPRECATED]
		/// </summary>
		/// <param name="newText">Text</param>
		/// <param name="x">X position of the text in the page</param>
		/// <param name="y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="parWidth">Paragraph's width</param>
		/// <param name="fontColor">Font's color</param>
		public void addParagraph(string newText, int x, int y, predefinedFont fontType, int fontSize, int parWidth, predefinedColor fontColor)
		{
			paragraphElement objParagraph = new paragraphElement(textAdapter.formatParagraph(newText, fontSize, fontType, parWidth), fontSize, fontType, x, y, fontColor);
			_elements.Add(objParagraph);			
			objParagraph = null;
		}

		/// <summary>
		/// Method that adds a paragraph to the page object
		/// </summary>
		/// <param name="newText">Text</param>
		/// <param name="x">X position of the text in the page</param>
		/// <param name="y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="parWidth">Paragraph's width</param>
		/// <param name="fontColor">Font's color</param>
		public void addParagraph(string newText, int x, int y, predefinedFont fontType, int fontSize, int parWidth, pdfColor fontColor)
		{
			paragraphElement objParagraph = new paragraphElement(textAdapter.formatParagraph(newText, fontSize, fontType, parWidth), fontSize, fontType, x, y, fontColor);
			_elements.Add(objParagraph);			
			objParagraph = null;
		}

		/// <summary>
		/// Method that adds a paragraph to the page object
		/// </summary>
		/// <param name="newText">Text</param>
		/// <param name="x">X position of the text in the page</param>
		/// <param name="y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="parWidth">Paragraph's width</param>
		/// <param name="lineHeight">Line's height</param>
		public void addParagraph(string newText, int x, int y, predefinedFont fontType, int fontSize, int parWidth, int lineHeight)
		{
			paragraphElement objParagraph = new paragraphElement(textAdapter.formatParagraph(newText, fontSize, fontType, parWidth, lineHeight), fontSize, fontType, x, y);
			_elements.Add(objParagraph);			
			objParagraph = null;
		}

		/// <summary>
		/// Method that adds a paragraph to the page object [DEPRECATED]
		/// </summary>
		/// <param name="newText">Text</param>
		/// <param name="x">X position of the text in the page</param>
		/// <param name="y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="parWidth">Paragraph's width</param>
		/// <param name="lineHeight">Line's height</param>
		/// <param name="fontColor">Font's color</param>
		public void addParagraph(string newText, int x, int y, predefinedFont fontType, int fontSize, int parWidth, int lineHeight, predefinedColor fontColor)
		{
			paragraphElement objParagraph = new paragraphElement(textAdapter.formatParagraph(newText, fontSize, fontType, parWidth, lineHeight), fontSize, fontType, x, y, fontColor);
			_elements.Add(objParagraph);			
			objParagraph = null;
		}

		/// <summary>
		/// Method that adds a paragraph to the page object
		/// </summary>
		/// <param name="newText">Text</param>
		/// <param name="x">X position of the text in the page</param>
		/// <param name="y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="parWidth">Paragraph's width</param>
		/// <param name="lineHeight">Line's height</param>
		/// <param name="fontColor">Font's color</param>
		public void addParagraph(string newText, int x, int y, predefinedFont fontType, int fontSize, int parWidth, int lineHeight, pdfColor fontColor)
		{
			paragraphElement objParagraph = new paragraphElement(textAdapter.formatParagraph(newText, fontSize, fontType, parWidth, lineHeight), fontSize, fontType, x, y, fontColor);
			_elements.Add(objParagraph);			
			objParagraph = null;
		}

		/// <summary>
		/// Method that adds a paragraph to the page object
		/// </summary>
		/// <param name="newText">Text</param>
		/// <param name="x">X position of the text in the page</param>
		/// <param name="y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="parWidth">Paragraph's width</param>
		/// <param name="lineHeight">Line's height</param>
		/// <param name="parAlign">Paragraph's alignment</param>
		public void addParagraph(string newText, int x, int y, predefinedFont fontType, int fontSize, int parWidth, int lineHeight, predefinedAlignment parAlign)
		{
			paragraphElement objParagraph = new paragraphElement(textAdapter.formatParagraph(newText, fontSize, fontType, parWidth, lineHeight, parAlign), fontSize, fontType, x, y);
			_elements.Add(objParagraph);			
			objParagraph = null;
		}

		/// <summary>
		/// Method that adds a paragraph to the page object [DEPRECATED]
		/// </summary>
		/// <param name="newText">Text</param>
		/// <param name="x">X position of the text in the page</param>
		/// <param name="y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="parWidth">Paragraph's width</param>
		/// <param name="lineHeight">Line's height</param>
		/// <param name="parAlign">Paragraph's alignment</param>
		/// <param name="fontColor">Font's color</param>
		public void addParagraph(string newText, int x, int y, predefinedFont fontType, int fontSize, int parWidth, int lineHeight, predefinedAlignment parAlign, predefinedColor fontColor)
		{
			paragraphElement objParagraph = new paragraphElement(textAdapter.formatParagraph(newText, fontSize, fontType, parWidth, lineHeight, parAlign), fontSize, fontType, x, y, fontColor);
			_elements.Add(objParagraph);			
			objParagraph = null;
		}

		/// <summary>
		/// Method that adds a paragraph to the page object
		/// </summary>
		/// <param name="newText">Text</param>
		/// <param name="x">X position of the text in the page</param>
		/// <param name="y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="parWidth">Paragraph's width</param>
		/// <param name="lineHeight">Line's height</param>
		/// <param name="parAlign">Paragraph's alignment</param>
		/// <param name="fontColor">Font's color</param>
		public void addParagraph(string newText, int x, int y, predefinedFont fontType, int fontSize, int parWidth, int lineHeight, predefinedAlignment parAlign, pdfColor fontColor)
		{
			paragraphElement objParagraph = new paragraphElement(textAdapter.formatParagraph(newText, fontSize, fontType, parWidth, lineHeight, parAlign), fontSize, fontType, x, y, fontColor);
			_elements.Add(objParagraph);			
			objParagraph = null;
		}

		/// <summary>
		/// Method that adds a paragraph to the page object
		/// </summary>
		/// <param name="newText">Interface IEnumerable that contains paragraphLine objects</param>
		/// <param name="x">X position of the text in the page</param>
		/// <param name="y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		public void addParagraph(IEnumerable newText, int x, int y, predefinedFont fontType, int fontSize)
		{
			try {
			paragraphElement objParagraph = new paragraphElement(newText, fontSize, fontType, x, y);
			_elements.Add(objParagraph);
			objParagraph = null;
			} catch (pdfIncorrectParagraghException ex) {
				Debug.Log ( ex );
				throw new pdfIncorrectParagraghException();
			}
		}

		/// <summary>
		/// Method that adds a paragraph to the page object [DEPRECATED]
		/// </summary>
		/// <param name="newText">Interface IEnumerable that contains paragraphLine objects</param>
		/// <param name="x">X position of the text in the page</param>
		/// <param name="y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="fontColor">Font's color</param>
		public void addParagraph(IEnumerable newText, int x, int y, predefinedFont fontType, int fontSize, predefinedColor fontColor)
		{
			try {
			paragraphElement objParagraph = new paragraphElement(newText, fontSize, fontType, x, y, fontColor);
			_elements.Add(objParagraph);
			objParagraph = null;
			} catch (pdfIncorrectParagraghException ex) {
				Debug.Log ( ex );
				throw new pdfIncorrectParagraghException();
			}
		}

		/// <summary>
		/// Method that adds a paragraph to the page object
		/// </summary>
		/// <param name="newText">Interface IEnumerable that contains paragraphLine objects</param>
		/// <param name="x">X position of the text in the page</param>
		/// <param name="y">Y position of the text in the page</param>
		/// <param name="fontType">Font's type</param>
		/// <param name="fontSize">Font's size</param>
		/// <param name="fontColor">Font's color</param>
		public void addParagraph(IEnumerable newText, int x, int y, predefinedFont fontType, int fontSize, pdfColor fontColor)
		{
			try {
			paragraphElement objParagraph = new paragraphElement(newText, fontSize, fontType, x, y, fontColor);
			_elements.Add(objParagraph);
			objParagraph = null;
			} catch (pdfIncorrectParagraghException ex) {
				Debug.Log ( ex );
				throw new pdfIncorrectParagraghException();
			}
		}

		/// <summary>
		/// Method that adds a table to the page object
		/// </summary>
		/// <param name="newTable">The table object</param>
		/// <param name="x">X position of the table in the page</param>
		/// <param name="y">Y position of the table in the page</param>
		public void addTable(pdfTable newTable, int x, int y)
		{
			int headerHeight = newTable.tableHeaderStyle.fontSize + (newTable.cellpadding * 2);
			int rowHeight = newTable.rowStyle.fontSize + (newTable.cellpadding * 2);
			int tableWidth = newTable.borderSize;
			int i;
			int j;
			int coordx;
			int coordy;
			int textx;
			string textWord;
			pdfTableRowStyle myStyle;
			bool alternate = false;

			
			for(i = 0; i < newTable.tableHeader.columnsCount; i++)			
			{				
				tableWidth += (newTable.borderSize + newTable.tableHeader[i].columnSize);
			}
			
			//Table's Header
			coordx = x;
			this.drawRectangle(x, y, x + tableWidth, y - headerHeight, newTable.borderColor, newTable.tableHeaderStyle.bgColor, newTable.borderSize);
			for (i = 0; i < newTable.tableHeader.columnsCount; i++)
			{
				textWord = textAdapter.cropWord(newTable.tableHeader[i].columnValue, newTable.tableHeaderStyle.fontSize, newTable.tableHeaderStyle.fontType, newTable.tableHeader[i].columnSize - (newTable.cellpadding * 2));
				switch (newTable.tableHeader[i].columnAlign) {
					case predefinedAlignment.csLeft:
					default:
						textx = coordx + newTable.cellpadding;
						break;
					case predefinedAlignment.csCenter:
						textx = coordx + ((newTable.tableHeader[i].columnSize - textAdapter.wordWeight(textWord,newTable.tableHeaderStyle.fontSize,newTable.tableHeaderStyle.fontType))/2);
						break;
					case predefinedAlignment.csRight:
						textx = coordx + (newTable.tableHeader[i].columnSize - newTable.cellpadding - textAdapter.wordWeight(textWord,newTable.tableHeaderStyle.fontSize,newTable.tableHeaderStyle.fontType));
						break;
				}
				this.addText(textWord, textx, y - (headerHeight - newTable.cellpadding),newTable.tableHeaderStyle.fontType,newTable.tableHeaderStyle.fontSize,newTable.tableHeaderStyle.fontColor) ;
				coordx += newTable.tableHeader[i].columnSize + newTable.borderSize;
				if (i < (newTable.tableHeader.columnsCount - 1)) {
					this.drawLine(coordx,y,coordx,y - headerHeight,newTable.borderColor, newTable.borderSize);								
				}
			}

			//Table's Rows			
			coordy = y - headerHeight;
			for(i = 0; i < newTable.rowsCount;i++)
			{	
				if (alternate && newTable.alternateRowStyle != null) {
					myStyle = newTable.alternateRowStyle;
				} else {
					myStyle = newTable.rowStyle;
				}

				alternate = !(alternate);

				this.drawRectangle(x, coordy, x + tableWidth, coordy - rowHeight, newTable.borderColor, myStyle.bgColor, newTable.borderSize);
				coordx = x;
				for (j = 0; j < newTable.tableHeader.columnsCount; j++)
				{	
					textWord = textAdapter.cropWord(newTable[i][j].columnValue, myStyle.fontSize, myStyle.fontType, newTable.tableHeader[j].columnSize - (newTable.cellpadding * 2));
					switch (newTable[i][j].columnAlign) {
						case predefinedAlignment.csLeft:
						default:
							textx = coordx + newTable.cellpadding;
							break;
						case predefinedAlignment.csCenter:
							textx = coordx + ((newTable.tableHeader[j].columnSize - textAdapter.wordWeight(textWord,myStyle.fontSize,myStyle.fontType))/2);
							break;
						case predefinedAlignment.csRight:
							textx = coordx + (newTable.tableHeader[j].columnSize - newTable.cellpadding - textAdapter.wordWeight(textWord,myStyle.fontSize,myStyle.fontType));
							break;
					}
					this.addText(textWord, textx, coordy - (rowHeight - newTable.cellpadding),myStyle.fontType,myStyle.fontSize,myStyle.fontColor) ;
					coordx += newTable.tableHeader[j].columnSize + newTable.borderSize;
					if (j < (newTable.tableHeader.columnsCount - 1)) {
						this.drawLine(coordx,coordy,coordx,coordy - rowHeight,newTable.borderColor, newTable.borderSize);								
					}
				}
				coordy -= rowHeight;
			}

		}

		/// <summary>
		/// Method that adds a line to the page object
		/// </summary>
		/// <param name="X">X position of the line in the page</param>
		/// <param name="Y">Y position of the line in the page</param>
		/// <param name="X1">X1 position of the line in the page</param>
		/// <param name="Y1">Y1 position of the line in the page</param>
		public void drawLine(int X, int Y, int X1, int Y1)
		{
			lineElement objLine = new lineElement(X, Y, X1, Y1);
			_elements.Add(objLine);
			objLine = null;
		}

		/// <summary>
		/// Method that adds a line to the page object [DEPRECATED]
		/// </summary>
		/// <param name="X">X position of the line in the page</param>
		/// <param name="Y">Y position of the line in the page</param>
		/// <param name="X1">X1 position of the line in the page</param>
		/// <param name="Y1">Y1 position of the line in the page</param>
		/// <param name="lineColor">Line's color</param>
		public void drawLine(int X, int Y, int X1, int Y1, predefinedColor lineColor)
		{
			lineElement objLine = new lineElement(X, Y, X1, Y1, lineColor);
			_elements.Add(objLine);
			objLine = null;
		}

		/// <summary>
		/// Method that adds a line to the page object
		/// </summary>
		/// <param name="X">X position of the line in the page</param>
		/// <param name="Y">Y position of the line in the page</param>
		/// <param name="X1">X1 position of the line in the page</param>
		/// <param name="Y1">Y1 position of the line in the page</param>
		/// <param name="lineColor">Line's color</param>
		public void drawLine(int X, int Y, int X1, int Y1, pdfColor lineColor)
		{
			lineElement objLine = new lineElement(X, Y, X1, Y1, lineColor);
			_elements.Add(objLine);
			objLine = null;
		}

		/// <summary>
		/// Method that adds a line to the page object [DEPRECATED]
		/// </summary>
		/// <param name="X">X position of the line in the page</param>
		/// <param name="Y">Y position of the line in the page</param>
		/// <param name="X1">X1 position of the line in the page</param>
		/// <param name="Y1">Y1 position of the line in the page</param>
		/// <param name="lineColor">Line's color</param>
		/// <param name="lineWidth">Line's size</param>
		public void drawLine(int X, int Y, int X1, int Y1, predefinedColor lineColor, int lineWidth)
		{
			lineElement objLine = new lineElement(X, Y, X1, Y1, lineWidth, lineColor);
			_elements.Add(objLine);
			objLine = null;
		}

		/// <summary>
		/// Method that adds a line to the page object
		/// </summary>
		/// <param name="X">X position of the line in the page</param>
		/// <param name="Y">Y position of the line in the page</param>
		/// <param name="X1">X1 position of the line in the page</param>
		/// <param name="Y1">Y1 position of the line in the page</param>
		/// <param name="lineColor">Line's color</param>
		/// <param name="lineWidth">Line's size</param>
		public void drawLine(int X, int Y, int X1, int Y1, pdfColor lineColor, int lineWidth)
		{
			lineElement objLine = new lineElement(X, Y, X1, Y1, lineWidth, lineColor);
			_elements.Add(objLine);
			objLine = null;
		}
		
		/// <summary>
		/// Method that adds a line to the page object
		/// </summary>
		/// <param name="X">X position of the line in the page</param>
		/// <param name="Y">Y position of the line in the page</param>
		/// <param name="X1">X1 position of the line in the page</param>
		/// <param name="Y1">Y1 position of the line in the page</param>
		/// <param name="lineStyle">Line's style</param>
		public void drawLine(int X, int Y, int X1, int Y1, predefinedLineStyle lineStyle)
		{
			lineElement objLine = new lineElement(X, Y, X1, Y1, lineStyle);
			_elements.Add(objLine);
			objLine = null;
		}

		/// <summary>
		/// Method that adds a line to the page object
		/// </summary>
		/// <param name="X">X position of the line in the page</param>
		/// <param name="Y">Y position of the line in the page</param>
		/// <param name="X1">X1 position of the line in the page</param>
		/// <param name="Y1">Y1 position of the line in the page</param>
		/// <param name="lineStyle">Line's style</param>
		/// <param name="lineWidth">Line's size</param>
		public void drawLine(int X, int Y, int X1, int Y1, predefinedLineStyle lineStyle, int lineWidth)
		{
			lineElement objLine = new lineElement(X, Y, X1, Y1, lineWidth, lineStyle);
			_elements.Add(objLine);
			objLine = null;
		}

		/// <summary>
		/// Method that adds a line to the page object [DEPRECATED]
		/// </summary>
		/// <param name="X">X position of the line in the page</param>
		/// <param name="Y">Y position of the line in the page</param>
		/// <param name="X1">X1 position of the line in the page</param>
		/// <param name="Y1">Y1 position of the line in the page</param>
		/// <param name="lineStyle">Line's style</param>
		/// <param name="lineColor">Line's color</param>
		public void drawLine(int X, int Y, int X1, int Y1, predefinedLineStyle lineStyle, predefinedColor lineColor)
		{
			lineElement objLine = new lineElement(X, Y, X1, Y1, lineStyle, lineColor);
			_elements.Add(objLine);
			objLine = null;
		}

		/// <summary>
		/// Method that adds a line to the page object
		/// </summary>
		/// <param name="X">X position of the line in the page</param>
		/// <param name="Y">Y position of the line in the page</param>
		/// <param name="X1">X1 position of the line in the page</param>
		/// <param name="Y1">Y1 position of the line in the page</param>
		/// <param name="lineStyle">Line's style</param>
		/// <param name="lineColor">Line's color</param>
		public void drawLine(int X, int Y, int X1, int Y1, predefinedLineStyle lineStyle, pdfColor lineColor)
		{
			lineElement objLine = new lineElement(X, Y, X1, Y1, lineStyle, lineColor);
			_elements.Add(objLine);
			objLine = null;
		}

		/// <summary>
		/// Method that adds a line to the page object [DEPRECATED]
		/// </summary>
		/// <param name="X">X position of the line in the page</param>
		/// <param name="Y">Y position of the line in the page</param>
		/// <param name="X1">X1 position of the line in the page</param>
		/// <param name="Y1">Y1 position of the line in the page</param>
		/// <param name="lineStyle">Line's style</param>
		/// <param name="lineColor">Line's color</param>
		/// <param name="lineWidth">Line's size</param>
		public void drawLine(int X, int Y, int X1, int Y1, predefinedLineStyle lineStyle, predefinedColor lineColor, int lineWidth)
		{
			lineElement objLine = new lineElement(X, Y, X1, Y1, lineWidth, lineStyle, lineColor);
			_elements.Add(objLine);
			objLine = null;
		}	

		/// <summary>
		/// Method that adds a line to the page object
		/// </summary>
		/// <param name="X">X position of the line in the page</param>
		/// <param name="Y">Y position of the line in the page</param>
		/// <param name="X1">X1 position of the line in the page</param>
		/// <param name="Y1">Y1 position of the line in the page</param>
		/// <param name="lineStyle">Line's style</param>
		/// <param name="lineColor">Line's color</param>
		/// <param name="lineWidth">Line's size</param>
		public void drawLine(int X, int Y, int X1, int Y1, predefinedLineStyle lineStyle, pdfColor lineColor, int lineWidth)
		{
			lineElement objLine = new lineElement(X, Y, X1, Y1, lineWidth, lineStyle, lineColor);
			_elements.Add(objLine);
			objLine = null;
		}

		/// <summary>
		/// Method that adds a rectangle to the page object [DEPRECATED]
		/// </summary>
		/// <param name="X">X position of the rectangle in the page</param>
		/// <param name="Y">Y position of the rectangle in the page</param>
		/// <param name="X1">X1 position of the rectangle in the page</param>
		/// <param name="Y1">Y1 position of the rectangle in the page</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Rectancle's color</param>
		public void drawRectangle(int X, int Y, int X1, int Y1, predefinedColor strokeColor, predefinedColor fillColor)
		{
			rectangleElement objRectangle = new rectangleElement(X, Y, X1, Y1, strokeColor, fillColor);
			_elements.Add(objRectangle);
			objRectangle = null;
		}

		/// <summary>
		/// Method that adds a rectangle to the page object
		/// </summary>
		/// <param name="X">X position of the rectangle in the page</param>
		/// <param name="Y">Y position of the rectangle in the page</param>
		/// <param name="X1">X1 position of the rectangle in the page</param>
		/// <param name="Y1">Y1 position of the rectangle in the page</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Rectancle's color</param>
		public void drawRectangle(int X, int Y, int X1, int Y1, pdfColor strokeColor, pdfColor fillColor)
		{
			rectangleElement objRectangle = new rectangleElement(X, Y, X1, Y1, strokeColor, fillColor);
			_elements.Add(objRectangle);
			objRectangle = null;
		}

		/// <summary>
		/// Method that adds a rectangle to the page object [DEPRECATED]
		/// </summary>
		/// <param name="X">X position of the rectangle in the page</param>
		/// <param name="Y">Y position of the rectangle in the page</param>
		/// <param name="X1">X1 position of the rectangle in the page</param>
		/// <param name="Y1">Y1 position of the rectangle in the page</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Rectancle's color</param>
		/// <param name="borderWidth">Border's size</param>
		public void drawRectangle(int X, int Y, int X1, int Y1, predefinedColor strokeColor, predefinedColor fillColor, int borderWidth)
		{
			rectangleElement objRectangle = new rectangleElement(X, Y, X1, Y1, strokeColor, fillColor, borderWidth);
			_elements.Add(objRectangle);
			objRectangle = null;
		}

		/// <summary>
		/// Method that adds a rectangle to the page object
		/// </summary>
		/// <param name="X">X position of the rectangle in the page</param>
		/// <param name="Y">Y position of the rectangle in the page</param>
		/// <param name="X1">X1 position of the rectangle in the page</param>
		/// <param name="Y1">Y1 position of the rectangle in the page</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Rectancle's color</param>
		/// <param name="borderWidth">Border's size</param>
		public void drawRectangle(int X, int Y, int X1, int Y1, pdfColor strokeColor, pdfColor fillColor, int borderWidth)
		{
			rectangleElement objRectangle = new rectangleElement(X, Y, X1, Y1, strokeColor, fillColor, borderWidth);
			_elements.Add(objRectangle);
			objRectangle = null;
		}

		/// <summary>
		/// Method that adds a rectangle to the page object [DEPRECATED]
		/// </summary>
		/// <param name="X">X position of the rectangle in the page</param>
		/// <param name="Y">Y position of the rectangle in the page</param>
		/// <param name="X1">X1 position of the rectangle in the page</param>
		/// <param name="Y1">Y1 position of the rectangle in the page</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Rectancle's color</param>
		/// <param name="borderStyle">Border's style</param>
		public void drawRectangle(int X, int Y, int X1, int Y1, predefinedColor strokeColor, predefinedColor fillColor, predefinedLineStyle borderStyle)
		{
			rectangleElement objRectangle = new rectangleElement(X, Y, X1, Y1, strokeColor, fillColor, borderStyle);
			_elements.Add(objRectangle);
			objRectangle = null;
		}

		/// <summary>
		/// Method that adds a rectangle to the page object
		/// </summary>
		/// <param name="X">X position of the rectangle in the page</param>
		/// <param name="Y">Y position of the rectangle in the page</param>
		/// <param name="X1">X1 position of the rectangle in the page</param>
		/// <param name="Y1">Y1 position of the rectangle in the page</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Rectancle's color</param>
		/// <param name="borderStyle">Border's style</param>
		public void drawRectangle(int X, int Y, int X1, int Y1, pdfColor strokeColor, pdfColor fillColor, predefinedLineStyle borderStyle)
		{
			rectangleElement objRectangle = new rectangleElement(X, Y, X1, Y1, strokeColor, fillColor, borderStyle);
			_elements.Add(objRectangle);
			objRectangle = null;
		}
		
		/// <summary>
		/// Method that adds a rectangle to the page object [DEPRECATED]
		/// </summary>
		/// <param name="X">X position of the rectangle in the page</param>
		/// <param name="Y">Y position of the rectangle in the page</param>
		/// <param name="X1">X1 position of the rectangle in the page</param>
		/// <param name="Y1">Y1 position of the rectangle in the page</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Rectancle's color</param>
		/// <param name="borderWidth">Border's size</param>
		/// <param name="borderStyle">Border's style</param>
		public void drawRectangle(int X, int Y, int X1, int Y1, predefinedColor strokeColor, predefinedColor fillColor, int borderWidth, predefinedLineStyle borderStyle)
		{
			rectangleElement objRectangle = new rectangleElement(X, Y, X1, Y1, strokeColor, fillColor, borderWidth, borderStyle);
			_elements.Add(objRectangle);
			objRectangle = null;
		}

		/// <summary>
		/// Method that adds a rectangle to the page object
		/// </summary>
		/// <param name="X">X position of the rectangle in the page</param>
		/// <param name="Y">Y position of the rectangle in the page</param>
		/// <param name="X1">X1 position of the rectangle in the page</param>
		/// <param name="Y1">Y1 position of the rectangle in the page</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Rectancle's color</param>
		/// <param name="borderWidth">Border's size</param>
		/// <param name="borderStyle">Border's style</param>
		public void drawRectangle(int X, int Y, int X1, int Y1, pdfColor strokeColor, pdfColor fillColor, int borderWidth, predefinedLineStyle borderStyle)
		{
			rectangleElement objRectangle = new rectangleElement(X, Y, X1, Y1, strokeColor, fillColor, borderWidth, borderStyle);
			_elements.Add(objRectangle);
			objRectangle = null;
		}

		/// <summary>
		/// Method that adds a circle to the page object [DEPRECATED]
		/// </summary>
		/// <param name="X">X position of the circle in the page</param>
		/// <param name="Y">Y position of the circle in the page</param>
		/// <param name="ray">Circle's ray</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Circle's color</param>
		public void drawCircle(int X, int Y, int ray, predefinedColor strokeColor, predefinedColor fillColor)
		{
			circleElement objCircle = new circleElement(X, Y, ray, strokeColor, fillColor);
			_elements.Add(objCircle);
			objCircle = null;
		}

		/// <summary>
		/// Method that adds a circle to the page object
		/// </summary>
		/// <param name="X">X position of the circle in the page</param>
		/// <param name="Y">Y position of the circle in the page</param>
		/// <param name="ray">Circle's ray</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Circle's color</param>
		public void drawCircle(int X, int Y, int ray, pdfColor strokeColor, pdfColor fillColor)
		{
			circleElement objCircle = new circleElement(X, Y, ray, strokeColor, fillColor);
			_elements.Add(objCircle);
			objCircle = null;
		}

		/// <summary>
		/// Method that adds a circle to the page object [DEPRECATED]
		/// </summary>
		/// <param name="X">X position of the circle in the page</param>
		/// <param name="Y">Y position of the circle in the page</param>
		/// <param name="ray">Circle's ray</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Circle's color</param>
		/// <param name="borderWidth">Border's size</param>
		public void drawCircle(int X, int Y, int ray, predefinedColor strokeColor, predefinedColor fillColor, int borderWidth)
		{
			circleElement objCircle = new circleElement(X, Y, ray, strokeColor, fillColor, borderWidth);
			_elements.Add(objCircle);
			objCircle = null;
		}

		/// <summary>
		/// Method that adds a circle to the page object
		/// </summary>
		/// <param name="X">X position of the circle in the page</param>
		/// <param name="Y">Y position of the circle in the page</param>
		/// <param name="ray">Circle's ray</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Circle's color</param>
		/// <param name="borderWidth">Border's size</param>
		public void drawCircle(int X, int Y, int ray, pdfColor strokeColor, pdfColor fillColor, int borderWidth)
		{
			circleElement objCircle = new circleElement(X, Y, ray, strokeColor, fillColor, borderWidth);
			_elements.Add(objCircle);
			objCircle = null;
		}

		/// <summary>
		/// Method that adds a circle to the page object [DEPRECATED]
		/// </summary>
		/// <param name="X">X position of the circle in the page</param>
		/// <param name="Y">Y position of the circle in the page</param>
		/// <param name="ray">Circle's ray</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Circle's color</param>
		/// <param name="borderStyle">Border's style</param>
		public void drawCircle(int X, int Y, int ray, predefinedColor strokeColor, predefinedColor fillColor, predefinedLineStyle borderStyle)
		{
			circleElement objCircle = new circleElement(X, Y, ray, strokeColor, fillColor, borderStyle);
			_elements.Add(objCircle);
			objCircle = null;
		}

		/// <summary>
		/// Method that adds a circle to the page object
		/// </summary>
		/// <param name="X">X position of the circle in the page</param>
		/// <param name="Y">Y position of the circle in the page</param>
		/// <param name="ray">Circle's ray</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Circle's color</param>
		/// <param name="borderStyle">Border's style</param>
		public void drawCircle(int X, int Y, int ray, pdfColor strokeColor, pdfColor fillColor, predefinedLineStyle borderStyle)
		{
			circleElement objCircle = new circleElement(X, Y, ray, strokeColor, fillColor, borderStyle);
			_elements.Add(objCircle);
			objCircle = null;
		}

		/// <summary>
		/// Method that adds a circle to the page object [DEPRECATED]
		/// </summary>
		/// <param name="X">X position of the circle in the page</param>
		/// <param name="Y">Y position of the circle in the page</param>
		/// <param name="ray">Circle's ray</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Circle's color</param>
		/// <param name="borderStyle">Border's style</param>
		/// <param name="borderWidth">Border's size</param>
		public void drawCircle(int X, int Y, int ray, predefinedColor strokeColor, predefinedColor fillColor, predefinedLineStyle borderStyle, int borderWidth)
		{
			circleElement objCircle = new circleElement(X, Y, ray, strokeColor, fillColor, borderWidth, borderStyle);
			_elements.Add(objCircle);
			objCircle = null;
		}

		/// <summary>
		/// Method that adds a circle to the page object
		/// </summary>
		/// <param name="X">X position of the circle in the page</param>
		/// <param name="Y">Y position of the circle in the page</param>
		/// <param name="ray">Circle's ray</param>
		/// <param name="strokeColor">Border's color</param>
		/// <param name="fillColor">Circle's color</param>
		/// <param name="borderStyle">Border's style</param>
		/// <param name="borderWidth">Border's size</param>
		public void drawCircle(int X, int Y, int ray, pdfColor strokeColor, pdfColor fillColor, predefinedLineStyle borderStyle, int borderWidth)
		{
			circleElement objCircle = new circleElement(X, Y, ray, strokeColor, fillColor, borderWidth, borderStyle);
			_elements.Add(objCircle);
			objCircle = null;
		}

		/// <summary>
		/// Internal method that adds fonts object to the page object
		/// </summary>
		/// <param name="fonts">ArrayList of fonts object</param>
		internal void addFonts(ArrayList fonts)
		{
			StringBuilder resultString = new StringBuilder();
			for (int i = 0; i < fonts.Count; i++) {
				resultString.Append("/F" + (i + 1).ToString() + " " + ((pdfFont)fonts[i]).objectID.ToString() +" 0 R " ); 
			}
			_fontObjectsReference = resultString.ToString();
			resultString = null;
		}
	
		/// <summary>
		/// Method that returns the PDF codes to write the page in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public string getText() 
		{
			StringBuilder pageContent = new StringBuilder();
            StringBuilder objContent = new StringBuilder();
            StringBuilder imgContent = new StringBuilder();          
            pageContent.Append(_objectID.ToString() + " 0 obj" + Convert.ToChar(13) + Convert.ToChar(10));
            pageContent.Append("<<" + Convert.ToChar(13) + Convert.ToChar(10));
            pageContent.Append("/Type /Page" + Convert.ToChar(13) + Convert.ToChar(10));
            pageContent.Append("/Parent " + _pageTreeID.ToString() + " 0 R" + Convert.ToChar(13) + Convert.ToChar(10));
            pageContent.Append("/Resources <</Font <<" + _fontObjectsReference + ">>" + Convert.ToChar(13) + Convert.ToChar(10));
			foreach( pdfElement item in _elements)
			{
                objContent.Append(item.objectID.ToString() + " 0 R ");
				if (item.GetType().Name == "imageElement")
				{
					imgContent.Append("/I" + ((imageElement)item).xObjectID.ToString() + " " + ((imageElement)item).xObjectID.ToString() + " 0 R ");
				}
			}
            if (imgContent.Length > 0)
			{
				pageContent.Append("/XObject <<" + imgContent.ToString() + ">>" + Convert.ToChar(13) + Convert.ToChar(10));
			}
            pageContent.Append(">>" + Convert.ToChar(13) + Convert.ToChar(10));
            pageContent.Append("/MediaBox [0 0 " + _width + " " + _height + "]" + Convert.ToChar(13) + Convert.ToChar(10));
			pageContent.Append("/CropBox [0 0 " + _width + " " + _height + "]" + Convert.ToChar(13) + Convert.ToChar(10));
            pageContent.Append("/Rotate 0" + Convert.ToChar(13) + Convert.ToChar(10));
			pageContent.Append("/ProcSet [/PDF /Text /ImageC]" + Convert.ToChar(13) + Convert.ToChar(10));
			if (objContent.Length > 0)
			{
				pageContent.Append("/Contents [" + objContent.ToString() + "]" + Convert.ToChar(13) + Convert.ToChar(10));
			}
            pageContent.Append(">>" + Convert.ToChar(13) + Convert.ToChar(10));
            pageContent.Append("endobj" + Convert.ToChar(13) + Convert.ToChar(10));
			objContent = null;
			imgContent = null;
            return pageContent.ToString();
		}
	}
}