using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using Directory = System.IO.Directory;
using File = System.IO.File;

public class TempLocalDataManagerScript : MonoBehaviour
{
    private static string dbName = "mainDB.mdf";
    void Awake()
    {
        CopyResourcesToLocalFiles();
    }
    private void CopyResourcesToLocalFiles()
    {
        TextAsset productJsonFile = Resources.Load<TextAsset>("ProductsDataDraft");
        TextAsset subscriptionsJsonFile = Resources.Load<TextAsset>("SubscriptionsDataDraft");
        string whereToSave = Application.persistentDataPath + "/Data/";
        if (!Directory.Exists(whereToSave))
            Directory.CreateDirectory(whereToSave);
        Debug.Log(Path.Combine(whereToSave, "ProductsDataDraft.json"));
        Debug.Log(Path.Combine(whereToSave, "SubscriptionsDataDraft.json"));
        File.WriteAllText(Path.Combine(whereToSave, "ProductsDataDraft.json"), productJsonFile.text);
        File.WriteAllText(Path.Combine(whereToSave, "SubscriptionsDataDraft.json"), subscriptionsJsonFile.text);
        SubscriptionManagerScript.Instance.JsonSavingDirectory = whereToSave;
        StartCoroutine(CopyDBToLocalFile(whereToSave));
    }

    private IEnumerator CopyDBToLocalFile(string whereToSave)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, dbName)))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Failed to load " + dbName + "; error:" + webRequest.error);
            }
            else
            {
                byte[] db = webRequest.downloadHandler.data;
                Debug.Log("DB written to " + Path.Combine(whereToSave, dbName));
                File.WriteAllBytes(Path.Combine(whereToSave, dbName), db);
                if (Application.isEditor)
                    DatabaseManager.Initiate(string.Join("\\", Application.dataPath.Split("/")) + "\\StreamingAssets\\" + dbName);
                else
                    DatabaseManager.Initiate(Path.Combine(whereToSave, dbName));
            }
        }
    }
}
