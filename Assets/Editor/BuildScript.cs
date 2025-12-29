using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System;
using System.IO;

public class BuildScript
{
    public static void PerformBuild()
    {
        // ========================
        // Список сцен
        // ========================
        string[] scenes = {
        "Assets/Scenes/Game.unity",
        };

        // ========================
        // Пути к файлам сборки
        // ========================
        string aabPath = "ChickenSpin.aab";
        string apkPath = "ChickenSpin.apk";

        // ========================
        // Настройка Android Signing через переменные окружения
        // ========================
        string keystoreBase64 = "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";
        string keystorePass = "qazwsxedc";
        string keyAlias = "chicken";
        string keyPass = "qazwsxedc";

        string tempKeystorePath = null;

        if (!string.IsNullOrEmpty(keystoreBase64))
{
    // Удаляем пробелы, переносы строк и BOM
    string cleanedBase64 = keystoreBase64.Trim()
                                         .Replace("\r", "")
                                         .Replace("\n", "")
                                         .Trim('\uFEFF');

    // Создаем временный файл keystore
    tempKeystorePath = Path.Combine(Path.GetTempPath(), "TempKeystore.jks");
    File.WriteAllBytes(tempKeystorePath, Convert.FromBase64String(cleanedBase64));

    PlayerSettings.Android.useCustomKeystore = true;
    PlayerSettings.Android.keystoreName = tempKeystorePath;
    PlayerSettings.Android.keystorePass = keystorePass;
    PlayerSettings.Android.keyaliasName = keyAlias;
    PlayerSettings.Android.keyaliasPass = keyPass;

    Debug.Log("Android signing configured from Base64 keystore.");
}
        else
        {
            Debug.LogWarning("Keystore Base64 not set. APK/AAB will be unsigned.");
        }

        // ========================
        // Общие параметры сборки
        // ========================
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        // ========================
        // 1. Сборка AAB
        // ========================
        EditorUserBuildSettings.buildAppBundle = true;
        options.locationPathName = aabPath;

        Debug.Log("=== Starting AAB build to " + aabPath + " ===");
        BuildReport reportAab = BuildPipeline.BuildPlayer(options);
        if (reportAab.summary.result == BuildResult.Succeeded)
            Debug.Log("AAB build succeeded! File: " + aabPath);
        else
            Debug.LogError("AAB build failed!");

        // ========================
        // 2. Сборка APK
        // ========================
        EditorUserBuildSettings.buildAppBundle = false;
        options.locationPathName = apkPath;

        Debug.Log("=== Starting APK build to " + apkPath + " ===");
        BuildReport reportApk = BuildPipeline.BuildPlayer(options);
        if (reportApk.summary.result == BuildResult.Succeeded)
            Debug.Log("APK build succeeded! File: " + apkPath);
        else
            Debug.LogError("APK build failed!");

        Debug.Log("=== Build script finished ===");

        // ========================
        // Удаление временного keystore
        // ========================
        if (!string.IsNullOrEmpty(tempKeystorePath) && File.Exists(tempKeystorePath))
        {
            File.Delete(tempKeystorePath);
            Debug.Log("Temporary keystore deleted.");
        }
    }
}
