using UnityEngine;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine.Events;
using SimpleFileBrowser;
using System.Text;
public class ZipHelper : MonoBehaviour
{
    [Header("Object Main")]
    public AppHandle app;
    public void ZipFolder(string sourceFolderPath, string zipFilePath, UnityAction<string> ActDone = null)
    {
        CreateZip(sourceFolderPath.Replace(FileBrowserHelpers.GetFilename(sourceFolderPath),""), zipFilePath, ActDone);
    }

    public void CreateZip(string sourceFolderPath, string zipFilePath, UnityAction<string> ActDone = null)
    {
        if (!Directory.Exists(sourceFolderPath))
        {
            app.carrot.Show_msg("UntraZip", "Directory does not exist:" + sourceFolderPath, Carrot.Msg_Icon.Error);
            return;
        }

        string[] files = Directory.GetFiles(sourceFolderPath, "*", SearchOption.AllDirectories);
        ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
        using (FileStream fsOut = File.Create(zipFilePath))
        using (ZipOutputStream zipStream = new ZipOutputStream(fsOut))
        {
            zipStream.SetLevel(9);

            foreach (string file in files)
            {
                string entryName = file.Substring(sourceFolderPath.Length + 1).Replace("\\", "/");
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
        ActDone?.Invoke(zipFilePath);
    }
}