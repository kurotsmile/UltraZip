using UnityEngine;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

public class ZipHelper
{
    public static void ZipFolder(string sourceFolderPath, string zipFilePath)
    {
        if (!Directory.Exists(sourceFolderPath))
        {
            Debug.LogError("Thư mục không tồn tại: " + sourceFolderPath);
            return;
        }

        FileStream fsOut = File.Create(zipFilePath);
        ZipOutputStream zipStream = new ZipOutputStream(fsOut);
        zipStream.SetLevel(9); // Độ nén từ 0 (không nén) đến 9 (nén cao nhất)

        string[] files = Directory.GetFiles(sourceFolderPath, "*", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            string entryName = file.Substring(sourceFolderPath.Length + 1).Replace("\\", "/"); // Tên file trong zip
            ZipEntry newEntry = new ZipEntry(entryName);
            newEntry.DateTime = File.GetLastWriteTime(file);
            zipStream.PutNextEntry(newEntry);

            byte[] buffer = File.ReadAllBytes(file);
            zipStream.Write(buffer, 0, buffer.Length);
            zipStream.CloseEntry();
        }

        zipStream.IsStreamOwner = true;
        zipStream.Close();

        Debug.Log("Đã nén thành công: " + zipFilePath);
    }
}