using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using QRCoder;
using QRCoder.Unity;
using sharpPDF;

public class QRCodeTextureTest : MonoBehaviour
{

    public RawImage rawImage;

    void Start()
    {
        //saveToFile("ugly call give address amount venture misery dose quick spoil weekend inspire");
        pdfTest();
    }

    public void setRawImage(string sentence)
    {
        rawImage = GetComponent<RawImage>();
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(sentence, QRCodeGenerator.ECCLevel.Q);
        UnityQRCode qrCode = new UnityQRCode(qrCodeData);
        Texture2D qrCodeAsTexture2D = qrCode.GetGraphic(20);
        rawImage.texture = qrCodeAsTexture2D;
    }

    public void saveToFile(string sentence)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(sentence, QRCodeGenerator.ECCLevel.Q);
        UnityQRCode qrCode = new UnityQRCode(qrCodeData);
        Texture2D qrCodeAsTexture2D = qrCode.GetGraphic(20);

        byte[] bytes = qrCodeAsTexture2D.EncodeToPNG();
           
        File.WriteAllBytes(Application.dataPath + "/../SavedQRCode.png", bytes);
    }

    public void testWithRawImage()
    {
        rawImage = GetComponent<RawImage>();
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode("ugly call give address amount venture misery dose quick spoil weekend inspire", QRCodeGenerator.ECCLevel.Q);
        UnityQRCode qrCode = new UnityQRCode(qrCodeData);
        Texture2D qrCodeAsTexture2D = qrCode.GetGraphic(20);
        rawImage.texture = qrCodeAsTexture2D;
    }

    public void pdfTest()
    {
        string sentence = "ugly call give address amount venture misery dose quick spoil weekend inspire";
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(sentence, QRCodeGenerator.ECCLevel.Q);
        UnityQRCode qrCode = new UnityQRCode(qrCodeData);
        Texture2D qrCodeAsTexture2D = qrCode.GetGraphic(20);

        rawImage.texture = qrCodeAsTexture2D;
        byte[] bytes = qrCodeAsTexture2D.EncodeToJPG(); // .EncodeToPNG();

        pdfDocument myDoc = new sharpPDF.pdfDocument("qr_pdf_test", "qr tester");
        pdfPage myPage = myDoc.addPage(500, 500);
        myPage.addImage(bytes, 1, 150, 200, 200);

        myPage.addText("Your seed entropy is: ", 10, 470, sharpPDF.Enumerators.predefinedFont.csCourier, 15);
        myPage.addText("0x3720B025A102812744F830F55DDA275C5", 10, 450, sharpPDF.Enumerators.predefinedFont.csCourier, 15);
        myPage.addText("ugly call give address amount venture misery dose quick spoil weekend inspire", 10, 425, sharpPDF.Enumerators.predefinedFont.csCourier, 10);

        myDoc.createPDF("qr_pdf_test.pdf");
        myPage = null;
        myDoc = null;

    }


}
