using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WalletConnectSharp.Unity;
using WalletConnectSharp.Unity.Utils;
using QRCoder;
using QRCoder.Unity;

[RequireComponent(typeof(Image))]
public class WalletConnectQRImage : BindableMonoBehavior
{
    public WalletConnect walletConnect;
    
    [BindComponent]
    private Image _image;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        if (walletConnect == null)
        {
            Debug.LogError("WalletConnectQRImage: No WalletConnect object given, QRImage will be disabled");
            enabled = false;
            return;
        }
        
        walletConnect.ConnectionStarted += WalletConnectOnConnectionStarted;
    }

    private void WalletConnectOnConnectionStarted(object sender, EventArgs e)
    {
        var url = walletConnect.Session.URI;
        //wc:ba77d891-d588-4930-86a7-ff704ec05606@1?bridge=wss%3A%2F%2Funity.etna.network%2Fws%2F&key=d199376a80d0847dc12c7c8d8cc8a85f3d71b7f82cfb9723426ef6b5b05143fe
        Debug.LogError("SessionUrl:: "+ url);
        
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        UnityQRCode qrCode = new UnityQRCode(qrCodeData);
        Texture2D qrCodeAsTexture2D = qrCode.GetGraphic(20);

        var qrCodeSprite = Sprite.Create(qrCodeAsTexture2D, new Rect(0, 0, qrCodeAsTexture2D.width, qrCodeAsTexture2D.height),
            new Vector2(0.5f, 0.5f), 100f);

        _image.sprite = qrCodeSprite;
    }
}
