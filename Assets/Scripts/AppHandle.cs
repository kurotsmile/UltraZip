using UnityEngine;
using UnityEngine.UI;

public class AppHandle : MonoBehaviour
{

    [Header("UI")]
    public GameObject panelHome;
    public GameObject panelList;
    public Image[] imgBtnMenuMain;

    public Color32 colorNomalMenu;
    public Color32 colorSelMenu;
    private int IndexSelMenu = 0;

    void Start()
    {
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

    public void OnStartZipFile()
    {
        ZipHelper.ZipFolder("sdsd", "thanh.zip");
    }

    public void BtnShowHome()
    {
        IndexSelMenu = 0;
        this.panelHome.SetActive(true);
        this.panelList.SetActive(false);
        UpdateStatusMenu();
    }

    public void BtnShowList()
    {
        IndexSelMenu = 1;
        this.panelHome.SetActive(false);
        this.panelList.SetActive(true);
        UpdateStatusMenu();
    }

    public void BtnShowSetting()
    {

    }
}
