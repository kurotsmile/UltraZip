using System;
using System.Collections;
using System.Collections.Generic;
using Carrot;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ZipForm : MonoBehaviour
{
    public AppHandle app;
    private Carrot_Box boxZip;
    private Carrot_Box_Item AddItemBoxFile(string sPathFile)
    {
        Carrot_Box_Item itemIn = boxZip.create_item();
        itemIn.gameObject.name = "zipF";
        itemIn.set_icon(app.iconZipFolder);
        itemIn.set_title("File add");
        itemIn.set_tip("Select file to zip");
        itemIn.set_type(Box_Item_Type.box_value_txt);
        itemIn.set_val(sPathFile);
        AddBtnOpenFolder(itemIn);
        itemIn.set_act(() =>
        {
            SelFiles(folderPath =>
            {
                itemIn.set_val(folderPath[0]);
            });

        });
        return itemIn;
    }

    public void BoxZip(string[] sPathIn, ZipType type = ZipType.Normal)
    {
        boxZip = app.carrot.Create_Box();
        boxZip.set_title("Compress files");
        if (type == ZipType.Normal)
            boxZip.set_icon(app.iconZipFolder);
        else
            boxZip.set_icon(app.iconAdvanced);

        boxZip.create_btn_menu_header(app.carrot.icon_carrot_add).set_act(() =>
        {
            SelFiles(folderPath =>
            {
                for (int i = 0; i < folderPath.Length; i++) AddItemBoxFile(folderPath[i]).gameObject.transform.SetAsFirstSibling();
            });
        });
        

        for (int i = 0; i < sPathIn.Length; i++)
        {
            AddItemBoxFile(sPathIn[i]);
        }

        Carrot_Box_Item itemNameFile = boxZip.create_item();
        itemNameFile.set_icon(app.iconFileZip);
        itemNameFile.set_title("Name file zip");
        itemNameFile.set_tip("Rename the zip file you are about to create.");
        itemNameFile.set_type(Box_Item_Type.box_value_input);
        itemNameFile.set_val("Data"+DateTime.Now.ToString("MM_dd_yyyy_HH_ss") + ".zip");

        boxZip.CreatePanelCancelDone(() =>
        {
            List<string> urls = new();
            foreach (Transform tr in boxZip.area_all_item)
            {
                if (tr.gameObject.name == "zipF")
                {
                    string s_url=tr.gameObject.GetComponent<Carrot_Box_Item>().get_val();
                    urls.Add(s_url);
                }
            }

            app.zip.ZipFiles(urls, itemNameFile.get_val(), path =>
            {
                new NativeShare()
                .AddFile(path)
                .SetSubject("Share compressed files")
                .SetText("Share the created zip file")
                .Share();
                app.carrot.Show_msg("Compress files", "Zip file success!\nAt:" + path);
                IDictionary dataZip = Json.Deserialize("{}") as IDictionary;
                dataZip["name"] = itemNameFile.get_val();
                dataZip["in"] =urls;
                dataZip["out"] = path;
                dataZip["date"] = DateTime.Now.ToString();
                app.history.Add(dataZip);
            });
        });
    }

    public void AddBtnOpenFolder(Carrot_Box_Item item)
    {
        Carrot_Box_Btn_Item btnDel = item.create_item();
        btnDel.set_icon(app.carrot.sp_icon_del_data);
        btnDel.set_icon_color(Color.white);
        btnDel.set_color(Color.red);
        Destroy(btnDel.gameObject.GetComponent<Button>());
        btnDel.set_act(() =>
        {
            app.carrot.play_sound_click();
            Destroy(item.gameObject);
        });

        Carrot_Box_Btn_Item btnSelPath = item.create_item();
        btnSelPath.set_icon(app.iconOpenPath);
        btnSelPath.set_icon_color(Color.white);
        btnSelPath.set_color(app.carrot.color_highlight);
        Destroy(btnSelPath.gameObject.GetComponent<Button>());
    }

    public void SelFiles(UnityAction<string[]> actDone)
    {
        if (app.carrot.os_app == OS.Android)
        {
            if (Application.isEditor)
            {
                app.file.Open_file(folderPath =>
                {
                    actDone?.Invoke(folderPath);
                });
            }
            else
            {
                NativeFilePicker.PickMultipleFiles(folderPath =>
                {
                    if (folderPath == null) return;
                    actDone?.Invoke(folderPath);
                });
            }
        }
        else
        {
            app.file.Open_file(folderPath =>
            {
                actDone?.Invoke(folderPath);
            });
        }
    }
}
