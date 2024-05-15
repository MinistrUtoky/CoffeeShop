using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Directory = System.IO.Directory;
using File = System.IO.File;

public class TempLocalDataManagerScript : MonoBehaviour
{
    private static string dbName = "mainDB.mdf";
    private static string dbLogName = "mainDB_log.ldf";
    void Awake()
    {
        CopyResourcesToLocalFiles();
        DatabaseManager.Initiate();
    }
    private void CopyResourcesToLocalFiles()
    {
        TextAsset subscriptionsJsonFile = Resources.Load<TextAsset>("SubscriptionsDataDraft");
        string whereToSave = Application.persistentDataPath + "/Data/";
        if (!Directory.Exists(whereToSave))
            Directory.CreateDirectory(whereToSave);
        Debug.Log(Path.Combine(whereToSave, "SubscriptionsDataDraft.json"));
        if (!File.Exists(Path.Combine(whereToSave, "SubscriptionsDataDraft.json")))
            File.WriteAllText(Path.Combine(whereToSave, "SubscriptionsDataDraft.json"), subscriptionsJsonFile.text);
        SubscriptionManagerScript.Instance.JsonSavingDirectory = whereToSave;
        SubscriptionManagerScript.Instance.subscriptionsJsonFile = Resources.Load<TextAsset>("SubscriptionsDataDraft");
        //StartCoroutine(CopyDBToLocalFile(whereToSave)); Works with sqlite only
    }

    private IEnumerator CopyDBToLocalFile(string whereToSave)
    {
        byte[] db, dbLog;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, dbName)))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Failed to load " + dbName + "; error:" + webRequest.error);
            }
            else
            {
                db = webRequest.downloadHandler.data;
                File.WriteAllBytes(Path.Combine(whereToSave, dbName), db);
                Debug.Log("DB written to " + Path.Combine(whereToSave, dbName));
                using (UnityWebRequest webRequest2 = UnityWebRequest.Get(Path.Combine(Application.streamingAssetsPath, dbLogName)))
                {
                    yield return webRequest2.SendWebRequest();
                    if (webRequest2.result == UnityWebRequest.Result.ProtocolError)
                    {
                        Debug.Log("Failed to load " + dbLogName + "; error:" + webRequest2.error);
                    }
                    else
                    {
                        dbLog = webRequest2.downloadHandler.data;
                        Debug.Log("DB log written to " + Path.Combine(whereToSave, dbLogName));
                        File.WriteAllBytes(Path.Combine(whereToSave, dbLogName), dbLog);
                    }

                    Debug.Log(string.Join("\\", Application.dataPath.Split("/")) + "\\StreamingAssets\\" + dbName);
                    Debug.Log(Path.Combine(Application.streamingAssetsPath, dbName));
                    //
                    //DatabaseManager.Initiate(string.Join("\\", Path.Combine(whereToSave, dbName).Split("/")));
                }
            }
        }
    }
}
