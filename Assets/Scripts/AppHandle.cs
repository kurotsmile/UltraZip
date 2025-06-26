using Carrot;
using UnityEngine;
using UnityEngine.UI;

public class AppHandle : MonoBehaviour
{
    [Header("Main Object")]
    public Carrot.Carrot carrot;
    public Carrot_File file;

    [Header("UI")]
    public GameObject panelHome;
    public GameObject panelList;
    public Image[] imgBtnMenuMain;

    public Color32 colorNomalMenu;
    public Color32 colorSelMenu;
    private int IndexSelMenu = 0;

    void Start()
    {
        this.carrot.Load_Carrot();
        this.panelHome.SetActive(true);
        this.panelList.SetActive(false);
        UpdateStatusMenu();
    }

    private void UpdateStatusMenu()
    {
        this.imgBtnMenuMain[0].color = this.colorNomalMenu;
        this.imgBtnMenuMain[1].color = this.colorNomalMenu;
        this.imgBtnMenuMain[IndexSelMenu].color = this.colorSelMenu;
    }

    public void BtnZipFile()
    {
        carrot.play_sound_click();
        ZipHelper.ZipFolder("sdsd", "thanh.zip");
    }

    public void BtnZipFolder()
    {
        carrot.play_sound_click();
        ZipHelper.ZipFolder("sdsd", "thanh.zip");
    }


    public void BtnShowHome()
    {
        carrot.play_sound_click();
        IndexSelMenu = 0;
        this.panelHome.SetActive(true);
        this.panelList.SetActive(false);
        UpdateStatusMenu();
    }

    public void BtnShowList()
    {
        carrot.play_sound_click();
        IndexSelMenu = 1;
        this.panelHome.SetActive(false);
        this.panelList.SetActive(true);
        UpdateStatusMenu();
    }

    public void BtnShowSetting()
    {
        carrot.Create_Setting();
    }
}
