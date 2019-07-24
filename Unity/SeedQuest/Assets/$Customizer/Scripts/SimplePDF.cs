using 	UnityEngine;
using 	System.Collections;
using 	System.IO;
using	sharpPDF;
using	sharpPDF.Enumerators;



public class SimplePDF : MonoBehaviour {
	
	internal	string		attacName	= "SimplePDF.pdf";

	// Use this for initialization
	IEnumerator Start () {
		yield return StartCoroutine ( CreatePDF() );
	}
	
	// Update is called once per frame
	public IEnumerator CreatePDF () {
		pdfDocument myDoc = new pdfDocument("Sample Application","Me", false);
		pdfPage myFirstPage = myDoc.addPage();
		
		
		
//		Debug.Log ( "Continue to create PDF");
		myFirstPage.addText("Test Driving",10,730,predefinedFont.csHelveticaOblique,30,new pdfColor(predefinedColor.csOrange));
		
		
		
		/*Table's creation*/
		pdfTable myTable = new pdfTable();
		//Set table's border
		myTable.borderSize = 1;
		myTable.borderColor = new pdfColor(predefinedColor.csDarkBlue);
		
		/*Add Columns to a grid*/
		myTable.tableHeader.addColumn(new pdfTableColumn("Model",predefinedAlignment.csRight,120));
		myTable.tableHeader.addColumn(new pdfTableColumn("Speed",predefinedAlignment.csCenter,120));
		myTable.tableHeader.addColumn(new pdfTableColumn("Weight",predefinedAlignment.csLeft,150));
		myTable.tableHeader.addColumn(new pdfTableColumn("Color",predefinedAlignment.csLeft,150));
		
		
		pdfTableRow myRow = myTable.createRow();
		myRow[0].columnValue = "A";
		myRow[1].columnValue = "100 km/h";
		myRow[2].columnValue = "180Kg";
		myRow[3].columnValue = "Orange";
		
		myTable.addRow(myRow);
		
		pdfTableRow myRow1 = myTable.createRow();
		myRow1[0].columnValue = "B";
		myRow1[1].columnValue = "130 km/h";
		myRow1[2].columnValue = "150Kg";
		myRow1[3].columnValue = "Yellow";
		
		myTable.addRow(myRow1);
		
		

		/*Set Header's Style*/
		myTable.tableHeaderStyle = new pdfTableRowStyle(predefinedFont.csCourierBoldOblique,12,new pdfColor(predefinedColor.csBlack),new pdfColor(predefinedColor.csLightOrange));
		/*Set Row's Style*/
		myTable.rowStyle = new pdfTableRowStyle(predefinedFont.csCourier,8,new pdfColor(predefinedColor.csBlack),new pdfColor(predefinedColor.csWhite));
		/*Set Alternate Row's Style*/
		myTable.alternateRowStyle = new pdfTableRowStyle(predefinedFont.csCourier,8,new pdfColor(predefinedColor.csBlack),new pdfColor(predefinedColor.csLightYellow));
		/*Set Cellpadding*/
		myTable.cellpadding = 10;
		/*Put the table on the page object*/
		myFirstPage.addTable(myTable, 5, 700);
		
		
		yield return StartCoroutine ( myFirstPage.newAddImage (  "FILE://picture1.jpg",2,100 ) );
		
		myDoc.createPDF(attacName);
		myTable = null;
	}
}
