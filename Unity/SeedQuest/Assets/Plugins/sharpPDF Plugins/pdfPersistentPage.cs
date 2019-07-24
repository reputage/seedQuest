using System;
using System.Collections;
//using System.Drawing;

using sharpPDF.Enumerators;
using sharpPDF.Exceptions;

namespace sharpPDF
{
	/// <summary>
	/// Class that represents a persistent page.
	/// All its objects are inherited by all document's pages.
	/// </summary>
	public class pdfPersistentPage
	{

		private ArrayList _persistentElements;

		/// <summary>
		/// Page's persistent elements
		/// </summary>
		public ArrayList persistentElements
		{
			get
			{
				return _persistentElements;
			}
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		public pdfPersistentPage()
		{
			_persistentElements = new ArrayList();
		}

		/// <summary>
		/// Class's distructor
		/// </summary>
		~pdfPersistentPage()
		{
			_persistentElements = null;
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
				_persistentElements.Add(objImage);
				objImage = null;
			} catch (pdfImageNotFoundException ex) {
				throw new pdfImageNotFoundException(ex.Message,ex);
			} catch (pdfImageIOException ex) {
				throw new pdfImageIOException(ex.Message,ex);
			}
		}

		/// <summary>
		/// Method that adds an image to the page object
		/// </summary>
		/// <param name="newImgObject">Image Object</param>
		/// <param name="X">X position of the image in the page</param>
		/// <param name="Y">Y position of the image in the page</param>		
//		public void addImage(Image newImgObject, int X, int Y)
//		{
//			try {
//				imageElement objImage = new imageElement(newImgObject, X, Y);
//				_persistentElements.Add(objImage);
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
//				_persistentElements.Add(objImage);
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
//				_persistentElements.Add(objImage);
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
			_persistentElements.Add(objText);
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
			_persistentElements.Add(objText);
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
			_persistentElements.Add(objText);
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
			_persistentElements.Add(objParagraph);			
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
			_persistentElements.Add(objParagraph);			
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
			_persistentElements.Add(objParagraph);			
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
			_persistentElements.Add(objParagraph);			
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
			_persistentElements.Add(objParagraph);			
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
			_persistentElements.Add(objParagraph);			
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
			_persistentElements.Add(objParagraph);			
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
			_persistentElements.Add(objParagraph);			
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
			_persistentElements.Add(objParagraph);			
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
			_persistentElements.Add(objParagraph);
			objParagraph = null;
			} catch (pdfIncorrectParagraghException ex) {
				UnityEngine.Debug.Log ( ex );
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
			_persistentElements.Add(objParagraph);
			objParagraph = null;
			} catch (pdfIncorrectParagraghException ex) {
				UnityEngine.Debug.Log ( ex );
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
			_persistentElements.Add(objParagraph);
			objParagraph = null;
			} catch (pdfIncorrectParagraghException ex) {
				UnityEngine.Debug.Log ( ex );
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
			_persistentElements.Add(objLine);
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
			_persistentElements.Add(objLine);
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
			_persistentElements.Add(objLine);
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
			_persistentElements.Add(objLine);
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
			_persistentElements.Add(objLine);
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
			_persistentElements.Add(objLine);
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
			_persistentElements.Add(objLine);
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
			_persistentElements.Add(objLine);
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
			_persistentElements.Add(objLine);
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
			_persistentElements.Add(objLine);
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
			_persistentElements.Add(objLine);
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
			_persistentElements.Add(objRectangle);
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
			_persistentElements.Add(objRectangle);
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
			_persistentElements.Add(objRectangle);
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
			_persistentElements.Add(objRectangle);
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
			_persistentElements.Add(objRectangle);
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
			_persistentElements.Add(objRectangle);
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
			_persistentElements.Add(objRectangle);
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
			_persistentElements.Add(objRectangle);
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
			_persistentElements.Add(objCircle);
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
			_persistentElements.Add(objCircle);
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
			_persistentElements.Add(objCircle);
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
			_persistentElements.Add(objCircle);
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
			_persistentElements.Add(objCircle);
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
			_persistentElements.Add(objCircle);
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
			_persistentElements.Add(objCircle);
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
			_persistentElements.Add(objCircle);
			objCircle = null;
		}
	}
}
