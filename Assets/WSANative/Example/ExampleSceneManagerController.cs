﻿using System;
using System.Collections.Generic;
using System.Linq;
using CI.WSANative.Advertising;
using CI.WSANative.Common;
using CI.WSANative.Device;
using CI.WSANative.Dialogs;
using CI.WSANative.Facebook;
using CI.WSANative.FileStorage;
using CI.WSANative.Mapping;
using CI.WSANative.Media;
using CI.WSANative.Notification;
using CI.WSANative.Pickers;
using CI.WSANative.Security;
using CI.WSANative.Serialisers;
using CI.WSANative.Store;
using CI.WSANative.Twitter;
using CI.WSANative.Web;
using UnityEngine;

public class ExampleSceneManagerController : MonoBehaviour
{
    public void Awake()
    {
        // Call this once when your app starts up to configure the library
        WSANativeCore.Initialise();
    }

    public void CreateDialog()
    {
        WSANativeDialog.ShowDialogWithOptions("This is a title", "This is a message", new List<WSADialogCommand>() { new WSADialogCommand("Yes"), new WSADialogCommand("No"), new WSADialogCommand("Cancel") }, 0, 2, (WSADialogResult result) =>
        {
            if (result.ButtonPressed == "Yes")
            {
                WSANativeDialog.ShowDialog("Yes Pressed", "Yes was pressed!");
            }
            else if (result.ButtonPressed == "No")
            {
                WSANativeDialog.ShowDialog("No Pressed", "No was pressed!");
            }
            else if (result.ButtonPressed == "Cancel")
            {
                WSANativeDialog.ShowDialog("Cancel Pressed", "Cancel was pressed!");
            }
        });
    }

    public void CreatePopupMenu()
    {
        WSANativeNotification.RemoveToastNotification("Tag1");
    }

    public void CreateToastNotification()
    {
        WSANativeNotification.ShowToastNotification(new WSAToastNotification()
        {
            Title = "This is a title",
            Text = "This is a description",
            Tag = "Tag1"
        });
    }

    public void SaveFile()
    {
        WSANativeNotification.ShowScheduledToastNotification(new WSAScheduledToastNotification()
        {
            Title = "This is a title",
            Text = "This is a description",
            Id = "1234",
            DeliveryTime = DateTime.Now.AddSeconds(15),
            Tag = "Tag1"
        });
    }

    public void LoadFile()
    {
        WSANativeNotification.RemoveScheduledToastNotification("1234");
    }

    public void DeleteFile()
    {
        var items = WSANativeNotification.GetScheduledToastNotifications();

        WSANativeDialog.ShowDialog($"{items.Count} items", string.Join(", ", items.Select(x => x.Id)));
    }

    public void PurchaseProduct()
    {
        WSANativeStore.GetAddOns(products =>
        {
            if (products.Products != null && products.Products.Count > 0)
            {
                WSANativeStore.RequestPurchase(products.Products.Keys.First(), result =>
                {
                    if (result.Status == WSAStorePurchaseStatus.Succeeded)
                    {
                        WSANativeDialog.ShowDialog("Purchased", "YAY");
                    }
                    else
                    {
                        WSANativeDialog.ShowDialog("Not Purchased", "NAY");
                    }
                });
            }
        });
    }

    public void ShowFileOpenPicker()
    {
        WSANativeFilePicker.PickSingleFile("Select", WSAPickerViewMode.Thumbnail, WSAPickerLocationId.PicturesLibrary, new[] { ".png", ".jpg" }, result =>
        {
        });
    }

    public void ShowFileSavePicker()
    {
        WSANativeFilePicker.PickSaveFile("Save", ".txt", "Test Text File", WSAPickerLocationId.DocumentsLibrary, new List<KeyValuePair<string, IList<string>>>() { new KeyValuePair<string, IList<string>>("Text Files", new List<string>() { ".txt" }) }, result =>
        {
        });
    }

    public void ShowFolderPicker()
    {
        WSANativeFolderPicker.PickSingleFolder("Ok", WSAPickerViewMode.List, WSAPickerLocationId.DocumentsLibrary, null, result =>
        {
        });
    }

    public void ShowContactPicker()
    {
        WSANativeContactPicker.PickContact(result =>
        {
        });
    }

    public void CreateInterstitialAd()
    {
        WSANativeInterstitialAd.Initialise(WSAInterstitialAdType.Vungle, "Your app id");
        WSANativeInterstitialAd.AdReady += (adType, adUnitOrPlacementId) =>
        {
            if (adType == WSAInterstitialAdType.Vungle)
            {
                WSANativeInterstitialAd.ShowAd(WSAInterstitialAdType.Vungle, adUnitOrPlacementId);
            }
        };
        WSANativeInterstitialAd.RequestAd(WSAInterstitialAdType.Vungle, "Your ad unit or placement id");
    }

    public void CreateBannerAd()
    {
        WSANativeBannerAd.Initialise(WSABannerAdType.AdDuplex, "Your app id", "Your ad unit or placement id");
        WSANativeBannerAd.CreatAd(WSABannerAdType.AdDuplex, 728, 90, WSAVerticalPlacement.Top, WSAHorizontalPlacement.Centre);
    }

    public void LaunchMapsApp()
    {
        WSANativeMap.LaunchMapsApp("collection=point.40.726966_-74.006076_Some Business");
    }

    public void CreateMap()
    {
        WSANativeMap.CreateMap(new WSAMapSettings()
        {
            MapServiceToken = "",
            HorizontalPlacement = WSAHorizontalPlacement.Centre,
            VerticalPlacement = WSAVerticalPlacement.Centre,
            Centre = new WSAGeoPoint() { Latitude = 50, Longitude = 0 },
            ZoomLevel = 6,
            Height = 700,
            Width = 700,
            InteractionMode = WSAMapInteractionMode.GestureAndControl
        });
    }

    public void DestroyMap()
    {
        WSANativeMap.DestroyMap();
    }

    public void AddPOI()
    {
        WSANativeMap.AddMapElement("You are here", new WSAGeoPoint() { Latitude = 52, Longitude = 5 });
    }

    public void ClearMap()
    {
        WSANativeMap.ClearMap();
    }

    public void CenterMap()
    {
        WSANativeMap.CenterMap(new WSAGeoPoint() { Latitude = 52, Longitude = 5 });
    }

    public void CreateBrowser()
    {
        WSANativeWebView.Create(new WSAWebViewSettings()
        {
            HorizontalPlacement = WSAHorizontalPlacement.Stretch,
            VerticalPlacement = WSAVerticalPlacement.Stretch,
            Uri = new Uri("https://www.claytoninds.com/")
        });
    }

    public void DestroyBrowser()
    {
        WSANativeWebView.Destroy();
    }

    public void CreateSpinner()
    {
        WSANativeDevice.CreateProgressRing(new WSAProgressControlSettings()
        {
            HorizontalPlacement = WSAHorizontalPlacement.Centre,
            VerticalPlacement = WSAVerticalPlacement.Centre,
            Height = 45,
            Width = 45,
            Colour = new Color32(255, 20, 147, 255)
        });
    }

    public void DestroySpinner()
    {
        WSANativeDevice.DestroyProgressRing();
    }

    public void CameraCapture()
    {
        WSANativeDevice.CapturePicture(512, 512, result =>
        {
        });
    }

    public void EncryptDecrypt()
    {
        string encrypted = WSANativeSecurity.SymmetricEncrypt("ffffffffffffffffffffffffffffffff", "aaaaaaaaaaaaaaaa", "Tesing123");

        WSANativeSecurity.SymmetricDecrypt("ffffffffffffffffffffffffffffffff", "aaaaaaaaaaaaaaaa", encrypted);
    }

    public void FacebookLogin()
    {
        WSANativeFacebook.Initialise("facebookId", "redirectUri");
        WSANativeFacebook.Login(null, result =>
        {
            if (result.Success)
            {
            }
        });
    }

    public void TwitterLogin()
    {
        WSANativeTwitter.Initialise("consumerKey", "consumerSecret", "https://www.twitter.com/");
        WSANativeTwitter.Login(true, result =>
        {
            if (result.Success)
            {
            }
        });
    }

    public void CreateMediaPlayer()
    {
        WSANativeMediaPlayer.Create(new WSAMediaPlayerSettings()
        {
            HorizontalPlacement = WSAHorizontalPlacement.Centre,
            VerticalPlacement = WSAVerticalPlacement.Centre,
            Height = 700,
            Width = 700,
            AreTransportControlsEnabled = true,
            AutoPlay = true,
            IsFullWindow = false,
            Uri = new Uri("https://cdn.flowplayer.com/a30bd6bc-f98b-47bc-abf5-97633d4faea0/hls/de3f6ca7-2db3-4689-8160-0f574a5996ad/playlist.m3u8")
        });
    }
}

public class Test
{
    public int x = 10;
    public float y = 20.56f;
    public string s = "Hello World";
}