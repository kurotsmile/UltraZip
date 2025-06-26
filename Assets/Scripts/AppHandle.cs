using UnityEngine;

public class AppHandle : MonoBehaviour
{

    [Header("UI")]
    public GameObject panelHome;
    public GameObject panelList;

    void Start()
    {
        this.panelHome.SetActive(true);
        this.panelList.SetActive(false);
    }

    public void OnStartZipFile()
    {
        ZipHelper.ZipFolder("sdsd", "thanh.zip");
    }

    public void BtnShowSetting()
    {
        
    }
}
