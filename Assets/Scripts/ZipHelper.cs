using UnityEngine;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine.Events;

public class ZipHelper: MonoBehaviour
{
    [Header("Object Main")]
    public AppHandle app;
    public void ZipFolder(string sourceFolderPath, string zipFilePath, UnityAction<string> ActDone = null)
    {
        if (!Directory.Exists(sourceFolderPath))
        {
            app.carrot.Show_msg("UntraZip", "Directory does not exist:" + sourceFolderPath, Carrot.Msg_Icon.Error);
            return;
        }

        if (File.Exists(zipFilePath))
        {
            try
            {
                File.Delete(zipFilePath);
                System.Threading.Thread.Sleep(100);
            }
            catch (IOException ex)
            {
                Debug.LogError("Không thể xóa file zip cũ: " + ex.Message);
                return;
            }
        }

        string[] files = Directory.GetFiles(sourceFolderPath, "*", SearchOption.AllDirectories);

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