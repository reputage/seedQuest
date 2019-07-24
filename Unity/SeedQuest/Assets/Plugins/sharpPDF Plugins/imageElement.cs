
using 	UnityEngine;
using 	System.Collections;

using System;
using System.IO;
//using System.Drawing;
using System.Text;


using sharpPDF.Exceptions;
using sharpPDF.Enumerators;

namespace sharpPDF
{
	/// <summary>
	/// A Class that implements a PDF image.
	/// </summary>
	internal class imageElement : pdfElement
	{

		private int _height;
		private int _width;
		private int _newHeight;
		private int _newWidth;
		private byte[] _content;
		private int _xObjectID;

		/// <summary>
		/// Original height of the image
		/// </summary>
		internal int height
		{
			get
			{
				return _height;
			}
		}

		/// <summary>
		/// Original width of the image
		/// </summary>
		internal int width
		{
			get
			{
				return _width;
			}				
		}

		/// <summary>
		/// New height of the image
		/// </summary>
		internal int newHeight
		{
			get
			{
				return _newHeight;
			}
			set
			{
				_newHeight = value;
			}
		}

		/// <summary>
		/// New width of the image
		/// </summary>
		internal int newWidth
		{
			get
			{
				return _newWidth;
			}
			set
			{
				_newWidth = value;
			}
		}

		/// <summary>
		/// Byte array that contains the stream of the image
		/// </summary>
		internal byte[] content
		{
			get
			{
				return _content;
			}
		}

		/// <summary>
		/// XObject ID
		/// </summary>
		internal int xObjectID
		{
			get
			{
				return _xObjectID;
			}
			set
			{
				_xObjectID = value;
			}
		}

	

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="imageName">Image's Name</param>
		/// <param name="newCoordX">X position in the PDF document</param>
		/// <param name="newCoordY">Y position in the PDF document</param>
		internal imageElement(Byte[] myImage, int newCoordX, int newCoordY, int imageHeight, int imageWidth)
		{	
			
			try 
			{
				//_content = new byte[myImage.Length];
				_content = myImage;				
				_height = imageHeight;
				_width = imageWidth;			
				_coordX = newCoordX;
				_coordY = newCoordY;				
			} 
			catch (System.IO.FileNotFoundException ex) 
			{
				throw new pdfImageNotFoundException("Immagine non trovata!", ex);
			} 
			catch (System.IO.IOException ex) 
			{
				throw new pdfImageIOException("Errore generale di IO sull'immagine ", ex);
			} finally {
//				if (tmpContent != null) {
//					tmpContent.Dispose();
//					tmpContent = null;
//				}
//				if (outStream != null) {					
//					outStream.Close();				
//					outStream = null;
//				}
			}
		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="myImage">Image Object</param>
		/// <param name="newCoordX">X position in the PDF document</param>
		/// <param name="newCoordY">Y position in the PDF document</param>
//		internal imageElement(Texture2D myImage, int newCoordX, int newCoordY)
//		{	
//			MemoryStream outStream = null;
//			try {				
//				outStream = new MemoryStream();			
//				//myImage.Save(outStream,System.Drawing.Imaging.ImageFormat.Jpeg);
//				_content = new byte[outStream.Length];
//				_content = outStream.ToArray();				
//				_height = myImage.Height;
//				_width = myImage.Width;			
//				_coordX = newCoordX;
//				_coordY = newCoordY;
//			} catch (System.IO.FileNotFoundException ex) {
//				throw new pdfImageNotFoundException("Oggetto Immagine non corretto!", ex);
//			} catch (System.IO.IOException ex) {
//				throw new pdfImageIOException("Errore generale di IO sull' oggetto immagine!", ex);
//			} finally {
//				if (outStream != null) {					
//					outStream.Close();				
//					outStream = null;
//				}
//			}
//		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="imageName">Image's Name</param>
		/// <param name="newCoordX">X position in the PDF document</param>
		/// <param name="newCoordY">Y position in the PDF document</param>
		/// <param name="newHeight">New height of the image</param>
		/// <param name="newWidth">New width of the image</param>
//		internal imageElement(string imageName, int newCoordX, int newCoordY, int newHeight, int newWidth)
//		{
//			Image myImage = null;
//			MemoryStream outStream = null;
//			try {				
//				myImage = Image.FromFile(imageName);
//				outStream = new MemoryStream();			
//				myImage.Save(outStream,System.Drawing.Imaging.ImageFormat.Jpeg);
//				_content = new byte[outStream.Length];
//				_content = outStream.ToArray();				
//				_height = myImage.Height;
//				_width = myImage.Width;
//				_newHeight = newHeight;
//				_newWidth = newWidth;				
//				_coordX = newCoordX;
//				_coordY = newCoordY;			
//			} catch (System.IO.FileNotFoundException ex) {
//				throw new pdfImageNotFoundException("Immagine "+ imageName +" non trovata o formato non corretto!",ex);
//			} catch (System.IO.IOException ex) {
//				throw new pdfImageIOException("Errore generale di IO sull'immagine "+ imageName,ex);
//			} finally {
//				if (myImage != null) {
//					myImage.Dispose();
//					myImage = null;
//				}
//				if (outStream != null) {					
//					outStream.Close();				
//					outStream = null;
//				}
//			}
//		}

		/// <summary>
		/// Class's constructor
		/// </summary>
		/// <param name="myImage">Image Object</param>
		/// <param name="newCoordX">X position in the PDF document</param>
		/// <param name="newCoordY">Y position in the PDF document</param>
		/// <param name="newHeight">New height of the image</param>
		/// <param name="newWidth">New width of the image</param>
//		internal imageElement(Image myImage, int newCoordX, int newCoordY, int newHeight, int newWidth)
//		{
//			MemoryStream outStream = null;
//			try {
//				outStream = new MemoryStream();			
//				myImage.Save(outStream,System.Drawing.Imaging.ImageFormat.Jpeg);
//				_content = new byte[outStream.Length];
//				_content = outStream.ToArray();				
//				_height = myImage.Height;
//				_width = myImage.Width;
//				_newHeight = newHeight;
//				_newWidth = newWidth;				
//				_coordX = newCoordX;
//				_coordY = newCoordY;			
//			} catch (System.IO.FileNotFoundException ex) {
//				throw new pdfImageNotFoundException("Oggetto Immagine non corretto!",ex);
//			} catch (System.IO.IOException ex) {
//				throw new pdfImageIOException("Errore generale di IO sull' oggetto immagine!",ex);
//			} finally {
//				if (outStream != null) {					
//					outStream.Close();				
//					outStream = null;
//				}
//			}
//		}

		/// <summary>
		/// Method that returns the PDF codes to write the image in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		public override string getText()
		{
			StringBuilder resultImage = new StringBuilder();
            StringBuilder imageContent = new StringBuilder();            
            imageContent.Append("q" + Convert.ToChar(13) + Convert.ToChar(10));
            if (_newHeight == 0 || _newWidth == 0) {                
                imageContent.Append(_width.ToString() + " 0 0 " + _height.ToString() + " " + _coordX.ToString() + " " + _coordY.ToString() + " cm" + Convert.ToChar(13) + Convert.ToChar(10));
            } else {
                imageContent.Append(_newWidth.ToString() + " 0 0 " + _newHeight.ToString() + " " + _coordX.ToString() + " " + _coordY.ToString() + " cm" + Convert.ToChar(13) + Convert.ToChar(10));
			}
            imageContent.Append("/I" + _xObjectID.ToString() + " Do" + Convert.ToChar(13) + Convert.ToChar(10));
            imageContent.Append("Q" + Convert.ToChar(13) + Convert.ToChar(10));			
            resultImage.Append(_objectID.ToString() + " 0 obj" + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("<<" + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("/Length " + imageContent.Length.ToString() + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append(">>" + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("stream" + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append(imageContent.ToString());
            resultImage.Append("endstream" + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("endobj" + Convert.ToChar(13) + Convert.ToChar(10));
			imageContent = null;
            return resultImage.ToString();            
		}

		/// <summary>
		/// Method that returns the PDF codes to write the XObject in the document
		/// </summary>
		/// <returns>String that contains PDF codes</returns>
		internal string getXObjectText()
		{
			StringBuilder resultImage = new StringBuilder();
            //------<XObject Header>------
            resultImage.Append(_xObjectID.ToString() + " 0 obj" + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("<<" + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("/Type /XObject" + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("/Subtype /Image" + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("/Name /I" + _xObjectID.ToString() + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("/Filter /DCTDecode" + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("/Width " + _width.ToString() + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("/Height " + _height.ToString() + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("/BitsPerComponent 8" + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("/ColorSpace /DeviceRGB" + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append("/Length " + _content.Length.ToString() + Convert.ToChar(13) + Convert.ToChar(10));
            resultImage.Append(">>" + Convert.ToChar(13) + Convert.ToChar(10));
			//------</XObject Header>-----
            return resultImage.ToString();            
		}
	}
}
