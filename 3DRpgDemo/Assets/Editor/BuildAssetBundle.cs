using Snappy;
using System;
using System.IO;
using System.Text;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;
/// <summary>
/// AssetBundle �������
/// </summary>
public class BuildAssetBundle
{
    /// <summary>
    /// ����������е�AssetBundles������
    /// </summary>
    [MenuItem("Card1ac�Ĵ����AB��/���ɣ�")]
    public static void BuildAllAB()
    { 
        string strABOutPathDir = "AssetBundles"; 
        if (!Directory.Exists(strABOutPathDir))
        {
            Directory.CreateDirectory(strABOutPathDir);
        }

#if UNITY_XBOX360
        
#elif UNITY_IPHONE
        BuildPipeline.BuildAssetBundles(strABOutPAthDir, BuildAssetBundleOptions.None, BuildTarget.iOS);
#elif UNITY_ANDROID
        BuildPipeline.BuildAssetBundles(strABOutPAthDir, BuildAssetBundleOptions.None, BuildTarget.Android); 
#elif UNITY_STANDALONE_OSX
       
#elif UNITY_STANDALONE_WIN
        BuildPipeline.BuildAssetBundles(strABOutPathDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
#endif
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
        // ��ȡ�ļ��������к�׺Ϊ".pkg"���ļ�
        string[] pkgFiles = Directory.GetFiles(strABOutPathDir, "*.pkg"); 
        foreach (string pkgFilePath in pkgFiles)
        {

            Debug.Log($"�ҵ�AB��{pkgFilePath}�����ڼ������Ϊ������");
            byte[] binaryData = File.ReadAllBytes(pkgFilePath);
            byte[] encryptedBytes = Encrypt(binaryData, LoadManager.assetBundleKey);
            byte[] compressedData = SnappyCodec.Compress(encryptedBytes);
            string[] filStrArr = pkgFilePath.Split("\\");
            string fileName = filStrArr[1].Split(".")[0];
            File.WriteAllBytes($"{Application.streamingAssetsPath}\\{fileName}.bin", compressedData);
            Debug.Log($"�ɹ�����AB��{pkgFilePath}����{Application.streamingAssetsPath}��鿴");
        }
    }

    static byte[] Encrypt(byte[] bytes, string key)
    { 
        byte[] keyBytes = Encoding.UTF8.GetBytes(key);

        byte[] encryptedBytes = new byte[bytes.Length];
        for (int i = 0; i < bytes.Length; i++)
        {
            encryptedBytes[i] = (byte)(bytes[i] ^ keyBytes[i % keyBytes.Length]);
        } 
        return encryptedBytes;
    }
     
}