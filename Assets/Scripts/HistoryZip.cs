using System.Collections;
using System.Collections.Generic;
using Carrot;
using UnityEngine;
using UnityEngine.UI;

public class HistoryZip : MonoBehaviour
{
    [Header("Obj Main")]
    public AppHandle app;
    public Transform trAreaContains;
    public Color32 colorRowA;
    public Color32 colorRowB;
    int length = 0;

    public void OnLoad()
    {
        length = PlayerPrefs.GetInt("lengthHistory", 0);
    }

    public void Add(IDictionary data)
    {
        string sData = Json.Serialize(data);
        PlayerPrefs.SetString("h_" + length, sData);
        length++;
        PlayerPrefs.SetInt("lengthHistory", length);
    }

    public void DeleteItem(int index)
    {
        if (index < 0 || index >= length) return;
        
        IDictionary dataZip = Json.Deserialize(PlayerPrefs.GetString("h_"+index)) as IDictionary;
        var pathFileDel = dataZip["out"].ToString();
        if (app.carrot.get_tool().check_file_exist(pathFileDel)) app.zip.Delete(pathFileDel);
        PlayerPrefs.DeleteKey("h_" + index);
        for (int i = index + 1; i < length; i++)
        {
            string nextKey = "h_" + i;
            string newKey = "h_" + (i - 1);
            PlayerPrefs.SetString(newKey, PlayerPrefs.GetString(nextKey, ""));
            PlayerPrefs.DeleteKey(nextKey);
        }
        
        length--;
        PlayerPrefs.SetInt("lengthHistory", length);
    }

    public void ShowList()
    {
        app.carrot.clear_contain(trAreaContains);
        int countFile = 0;
        for (int i = length-1; i >=0; i--)
        {
            string sData = PlayerPrefs.GetString("h_" + i, "");
            if (sData != "")
            {
                var IndexItem = i;
                IDictionary dataZip = Json.Deserialize(sData) as IDictionary;
                var urlShare = dataZip["out"].ToString();
                var dZip = dataZip;
                IList listUrl = dataZip["in"] as IList;
                Carrot_Box_Item itemZ = app.CreateMenuItem(trAreaContains);
                itemZ.set_icon(app.iconFileZip);
                itemZ.set_title(dataZip["name"].ToString());
                itemZ.set_tip(listUrl.Count+" File - "+dataZip["date"].ToString());
                itemZ.set_act(() =>
                {
                    app.carrot.play_sound_click();
                    BoxInfo(dZip);
                });

                if (i % 2 == 0)
                    itemZ.GetComponent<Image>().color = colorRowA;
                else
                    itemZ.GetComponent<Image>().color = colorRowB;

                Carrot_Box_Btn_Item btnShare = itemZ.create_item();
                btnShare.set_icon(app.carrot.sp_icon_share);
                btnShare.set_icon_color(Color.white);
                btnShare.set_color(app.carrot.color_highlight);
                btnShare.set_act(() =>
                {
                    app.carrot.play_sound_click();
                    app.carrot.play_vibrate();
                    new NativeShare().AddFile(urlShare).SetSubject("Here is my zip file").SetText(dZip["name"].ToString()).SetUrl("https://play.google.com/store/apps/details?id=com.carrotstore.loverlyai").Share();
                });

                Carrot_Box_Btn_Item btnExport = itemZ.create_item();
                btnExport.set_icon(app.iconExportFile);
                btnExport.set_icon_color(Color.white);
                btnExport.set_color(app.carrot.color_highlight);
                btnExport.set_act(() =>
                {
                    app.carrot.play_sound_click();
                    app.carrot.play_vibrate();
                    app.file.Save_file(spath =>
                    {
                        app.zip.Export(urlShare, spath[0]);
                    });
                });

                Carrot_Box_Btn_Item btnDel = itemZ.create_item();
                btnDel.set_icon(app.carrot.sp_icon_del_data);
                btnDel.set_icon_color(Color.white);
                btnDel.set_color(app.carrot.color_highlight);
                btnDel.set_act(() =>
                {
                    app.carrot.play_vibrate();
                    app.carrot.play_sound_click();
                    DeleteItem(IndexItem);
                    ShowList();
                });
                countFile++;
            }
        }

        if (countFile == 0)
        {
            Carrot_Box_Item itemSad = app.CreateMenuItem(trAreaContains);
            itemSad.set_icon(app.iconCatSad);
            itemSad.set_title("Empty list");
            itemSad.set_tip("No items have been compressed yet.");
            itemSad.set_type(Box_Item_Type.box_value_txt);
            itemSad.set_val("Start compressing files and you can manage them here");
            itemSad.GetComponent<Image>().color = colorRowA;

            Carrot_Box_Item itemZipFile = app.CreateMenuItem(trAreaContains);
            itemZipFile.set_icon(app.iconZipFile);
            itemZipFile.set_title("Add file");
            itemZipFile.set_tip("Select file to compress");
            app.zipForm.AddBtnOpenFolder(itemZipFile);
            itemZipFile.set_act(app.BtnZipNormal);
            itemZipFile.GetComponent<Image>().color = colorRowB;
        }
    }

    private void BoxInfo(IDictionary dataZ)
    {
        Carrot_Box boxInfo = app.carrot.Create_Box();
        boxInfo.set_icon(app.carrot.user.icon_user_info);
        boxInfo.set_title("Info");

        Carrot_Box_Item itemName = boxInfo.create_item();
        itemName.set_title("Name File");
        itemName.set_tip(dataZ["name"].ToString());
        itemName.set_icon(app.iconFileZip);
        itemName.set_act(() => { app.ShowCopy(dataZ["name"].ToString()); });

        IList listUrl = dataZ["in"] as IList;
        for (int i = 0; i < listUrl.Count; i++)
        {
            Carrot_Box_Item itemInpath = boxInfo.create_item();
            itemInpath.set_title("File " + (i + 1));
            itemInpath.set_tip(listUrl[i].ToString());
            itemInpath.set_icon(app.iconZipFile);
            itemInpath.set_act(() => app.ShowCopy(listUrl[i].ToString()));
        }

        Carrot_Box_Item itemOutpath = boxInfo.create_item();
        itemOutpath.set_title("Path to compress files");
        itemOutpath.set_tip(dataZ["out"].ToString());
        itemOutpath.set_icon(app.iconPathOut);
        itemOutpath.set_act(() => { app.ShowCopy(dataZ["out"].ToString()); });

        Carrot_Box_Item itemLevel = boxInfo.create_item();
        itemLevel.set_title("Compression level");
        itemLevel.set_tip("Level " + dataZ["level"].ToString());
        itemLevel.set_icon(app.iconCompressionlevel);
        
        Carrot_Box_Item itemDate = boxInfo.create_item();
        itemDate.set_title("Date created");
        itemDate.set_tip(dataZ["date"].ToString());
        itemDate.set_icon(app.carrot.sp_icon_table_color);
    }
}
