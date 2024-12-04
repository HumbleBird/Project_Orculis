using System.IO;
using UnityEditor;
using UnityEngine;

public class MyEditor
{
    // 경로 설정 (슬래시 사용)
    private static string originalPath = @"C:/Users/wltns/AppData/LocalLow/DefaultCompany/Project_Orculis/Samples";
    private static string copyPath = @"D:/LeeTaeSeop/Game Make/PP/Project_Orculis/Assets/Resources/Data/Gesture Recognition/";

    [MenuItem("My Editor/File/Gesture Record File Load C To D")]
    static void SyncFiles()
    {
        // 원본 폴더 존재 확인
        if (!Directory.Exists(originalPath))
        {
            Debug.LogError($"Original path does not exist: {originalPath}");
            return;
        }

        // 원본 폴더에서 파일 목록 확인
        string[] files = Directory.GetFiles(originalPath);
        if (files.Length == 0)
        {
            Debug.LogWarning($"No files found in the original path: {originalPath}");
            return;
        }

        // 복사 대상 폴더 확인 및 준비
        if (!Directory.Exists(copyPath))
        {
            // 복사 대상 폴더 생성
            Directory.CreateDirectory(copyPath);
            Debug.Log($"Created directory: {copyPath}");
        }
        else
        {
            // 복사 대상 폴더의 기존 파일 삭제
            string[] existingFiles = Directory.GetFiles(copyPath);
            foreach (string file in existingFiles)
            {
                try
                {
                    File.Delete(file);
                    Debug.Log($"Deleted file: {file}");
                }
                catch (IOException e)
                {
                    Debug.LogError($"Failed to delete file: {file}. Error: {e.Message}");
                }
            }
        }

        // 원본 파일 복사
        foreach (string file in files)
        {
            try
            {
                string fileName = Path.GetFileName(file);
                string destinationFile = Path.Combine(copyPath, fileName);
                File.Copy(file, destinationFile);
                Debug.Log($"Copied file: {file} to {destinationFile}");
            }
            catch (IOException e)
            {
                Debug.LogError($"Failed to copy file: {file}. Error: {e.Message}");
            }
        }

        Debug.Log("File synchronization completed successfully.");
    }
}
