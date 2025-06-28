using System;
using System.Collections;
using Carrot;
using SimpleFileBrowser;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;
public enum ZipType { file, folder }
public class AppHandle : MonoBehaviour
{
    [Header("Main Object")]
    public Carrot.Carrot carrot;
    public Carrot_File file;
    public HistoryZip history;
    public ZipHelper zip;
    public GameObject itemMenuPrefab;

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
        file.Open_file(path =>
        {
            BoxZip(ZipType.file, path[0]);
        });
    }

    public void BtnZipFolder()
    {
        carrot.play_sound_click();
        file.Open_folders(path =>
        {
            BoxZip(ZipType.folder, path[0]);
        });
    }

    private void BoxZip(ZipType type, string sPathIn = "")
    {
        Carrot_Box boxZip = carrot.Create_Box();
        if (type == ZipType.file)
        {
            boxZip.set_title("Zip File");
            boxZip.set_icon(iconZipFile);
        }
        else
        {
            boxZip.set_title("Zip Folder");
            boxZip.set_icon(iconZipFile);
        }

        Carrot_Box_Item itemIn = boxZip.create_item();
        Carrot_Box_Item itemOut = boxZip.create_item();
        Carrot_Box_Item itemNameFile = boxZip.create_item();
        itemIn.set_icon(iconZipFolder);
        itemIn.set_title("File in");
        itemIn.set_tip("Select file to zip");
        itemIn.set_type(Box_Item_Type.box_value_txt);
        AddBtnOpenFolder(itemIn);
        itemIn.set_act(() =>
        {
            if (type == ZipType.folder)
            {
                file.Open_folders(path =>
                {
                    itemNameFile.set_val(FileBrowserHelpers.GetFilename(path[0]) + ".zip");
                    itemIn.set_val(path[0]);
                    itemOut.set_val(path[0]);
                });
            }
            else
            {
                file.Open_file(path =>
                {
                    itemNameFile.set_val(FileBrowserHelpers.GetFilename(path[0]) + ".zip");
                    itemIn.set_val(path[0]);
                    itemOut.set_val(path[0]);
                });
            }

        });


        itemOut.set_type(Box_Item_Type.box_value_txt);
        itemOut.set_icon(iconPathOut);
        itemOut.set_title("File out");
        itemOut.set_tip("Select folder create file zip");
        AddBtnOpenFolder(itemOut);
        itemOut.set_act(() =>
        {
            file.Open_folders(path =>
            {
                itemOut.set_val(path[0]);
            });
        });

        itemNameFile.set_icon(iconFileZip);
        itemNameFile.set_title("Name file zip");
        itemNameFile.set_tip("Rename the zip file you are about to create.");
        itemNameFile.set_type(Box_Item_Type.box_value_input);

        if (sPathIn != "")
        {
            itemIn.set_val(sPathIn);
            itemOut.set_val(sPathIn);
            itemNameFile.set_val(FileBrowserHelpers.GetFilename(sPathIn) + ".zip");
        }

        boxZip.CreatePanelCancelDone(() =>
        {
            string sPathNew = itemOut.get_val() + "/" + itemNameFile.get_val();
            zip.ZipFolder(itemIn.get_val(), sPathNew,path=>
            {
                carrot.Show_msg("Zip file success!\nAt:" + sPathNew);
                IDictionary dataZip = Json.Deserialize("{}") as IDictionary;
                dataZip["name"] = itemNameFile.get_val();
                dataZip["in"] = itemIn.get_val();
                dataZip["out"] = itemOut.get_val();
                dataZip["date"] = new DateTime().ToString();
                history.Add(dataZip);
            });
        });
    }

    public void AddBtnOpenFolder(Carrot_Box_Item item)
    {
        Carrot_Box_Btn_Item btnSelPath = item.create_item();
        btnSelPath.set_icon(iconOpenPath);
        btnSelPath.set_icon_color(Color.white);
        btnSelPath.set_color(carrot.color_highlight);
        Destroy(btnSelPath.gameObject.GetComponent<Button>());
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
        history.ShowList();
    }

    public void BtnShowSetting()
    {
        carrot.Create_Setting();
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
}
