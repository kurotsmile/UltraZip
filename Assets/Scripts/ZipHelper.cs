using UnityEngine;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine.Events;
using System.Text;
using System.Collections.Generic;
public class ZipHelper : MonoBehaviour
{
    [Header("Object Main")]
    public AppHandle app;

    public void ZipFiles(List<string> files, string nameFile,int level, UnityAction<string> ActDone = null)
    {
        ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
        string pathNewFile;
        if (Application.isEditor)
            pathNewFile = Application.dataPath + "/" + nameFile;
        else
            pathNewFile = Application.persistentDataPath + "/" + nameFile;
        using (FileStream fsOut = File.Create(pathNewFile))
        using (ZipOutputStream zipStream = new ZipOutputStream(fsOut))
        {
            zipStream.SetLevel(level);

            foreach (string file in files)
            {
                if (!File.Exists(file))
                {
                    Debug.LogWarning($"File không tồn tại: {file}");
                    continue;
                }

                string entryName = Path.GetFileName(file);
                ZipEntry newEntry = new(entryName)
                {
                    DateTime = File.GetLastWriteTime(file)
                };
                zipStream.PutNextEntry(newEntry);

                using (FileStream fileStream = File.OpenRead(file))
                {
                    fileStream.CopyTo(zipStream);
                }

                zipStream.CloseEntry();
            }

            zipStream.IsStreamOwner = true;
        }

        ActDone?.Invoke(pathNewFile);
    }

    public void Export(string PathFileCur, string PathNew)
    {
        File.Copy(PathFileCur, PathNew, true);
        app.carrot.Show_msg("Export file", "Export file success!\nAt:" + PathNew, Carrot.Msg_Icon.Success);
    }
    
    public void Delete(string PathFile)
    {
        File.Delete(PathFile);
    }

}