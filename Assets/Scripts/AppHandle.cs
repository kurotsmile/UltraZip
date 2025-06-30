using System;
using System.Collections;
using Carrot;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.UI;
public enum ZipType {Normal,Advanced}
public class AppHandle : MonoBehaviour
{
    [Header("Main Object")]
    public Carrot.Carrot carrot;
    public Carrot_File file;
    public HistoryZip history;
    public ZipHelper zip;
    public ZipForm zipForm;
    public IronSourceAds ads;
    public GameObject itemMenuPrefab;
    public AudioSource audioBk;

    [Header("UI")]
    public GameObject panelHome;
    public GameObject panelList;
    public Image[] imgBtnMenuMain;

    [Header("Asset Icon")]
    public Sprite iconZipFile;
    public Sprite iconZipFolder;
    public Sprite iconPathOut;
    public Sprite iconOpenPath;
    public Sprite iconFileZip;
    public Sprite iconCatSad;
    public Sprite iconCopy;
    public Sprite iconAdvanced;
    public Sprite iconExportFile;
    public Sprite iconCompressionlevel;

    public Color32 colorNomalMenu;
    public Color32 colorSelMenu;
    private int IndexSelMenu = 0;

    void Start()
    {
        this.carrot.Load_Carrot();
        this.panelHome.SetActive(true);
        this.panelList.SetActive(false);
        history.OnLoad();
        UpdateStatusMenu();
        if (carrot.os_app == OS.Window) file.type = Carrot_File_Type.StandaloneFileBrowser;
        this.carrot.game.load_bk_music(audioBk);
    }

    private void UpdateStatusMenu()
    {
        this.imgBtnMenuMain[0].color = this.colorNomalMenu;
        this.imgBtnMenuMain[1].color = this.colorNomalMenu;
        this.imgBtnMenuMain[IndexSelMenu].color = this.colorSelMenu;
    }

    public void BtnZipNormal()
    {
        this.ads.show_ads_Interstitial();
        carrot.play_sound_click();
        zipForm.SelFiles(folderPath =>zipForm.BoxZip(folderPath,ZipType.Normal));
    }

    public void BtnZipAdvanced()
    {
        this.ads.show_ads_Interstitial();
        carrot.play_sound_click();
        zipForm.SelFiles(folderPath =>
        {
            zipForm.BoxZip(folderPath,ZipType.Advanced);
        });
    }


    public void BtnShowHome()
    {
        this.ads.show_ads_Interstitial();
        carrot.play_sound_click();
        IndexSelMenu = 0;
        this.panelHome.SetActive(true);
        this.panelList.SetActive(false);
        UpdateStatusMenu();
    }

    public void BtnShowList()
    {
        this.ads.show_ads_Interstitial();
        carrot.play_sound_click();
        IndexSelMenu = 1;
        this.panelHome.SetActive(false);
        this.panelList.SetActive(true);
        UpdateStatusMenu();
        history.ShowList();
    }

    public void BtnShowSetting()
    {
        Carrot_Box boxSetting = carrot.Create_Setting();
        boxSetting.set_act_before_closing(this.ads.show_ads_Interstitial);
    }

    public Carrot_Box_Item CreateMenuItem(Transform trFathe)
    {
        GameObject item = Instantiate(itemMenuPrefab);
        item.transform.SetParent(trFathe);
        item.transform.localScale = new Vector3(1f, 1f, 1f);
        item.transform.localRotation = Quaternion.Euler(Vector3.zero);
        Carrot_Box_Item BoxItem = item.GetComponent<Carrot_Box_Item>();
        BoxItem.check_type();
        return BoxItem;
    }

    public void ShowCopy(string sText)
    {
        carrot.Show_input("UltraZip", sText, sText, Window_Input_value_Type.input_field).set_icon(iconCopy);
    }
}
