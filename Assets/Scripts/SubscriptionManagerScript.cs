using productRelated;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UserManagement;

public class SubscriptionManagerScript : MonoBehaviour
{
    public static SubscriptionManagerScript Instance { get; private set; }

    public UnityEvent OnSubscriptionRelatedChanges;

    private MyList<Subscription> allSubscriptionList;
    private List<Subscription> currentUserSubscriptionList;

    [SerializeField] public TextAsset subscriptionsJsonFile;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UserManagerScript.Instance.OnLogin.AddListener(GetSubscriptionsFromJson);
    }

    private void UserManagerScript_OnLogin()
    {
        Debug.Log(UserManagerScript.Instance.GetCurrentUser().login);
        GetSubscriptionsFromJson();
    }
    public void AddSubscription(Subscription subscription)
    {
        allSubscriptionList.productList.Add(subscription);
        UpdateJsonFile();
        UpdateCurrentUserSubscriptionList();
        OnSubscriptionRelatedChanges?.Invoke();
    }
    public void RemoveSubscription(Subscription subscription)
    {
        allSubscriptionList.productList.Remove(subscription);
        UpdateJsonFile();
        UpdateCurrentUserSubscriptionList();
        OnSubscriptionRelatedChanges?.Invoke();
    }
    public MyList<Subscription> GetAllSubscriptionList()
    {
        return allSubscriptionList;
    }
    public List<Subscription> GetCurrentUserSubscriptionList()
    {
        return currentUserSubscriptionList;
    }
    private void UpdateCurrentUserSubscriptionList()
    {
        // Пока не решили вопрос с обновлением текстового файла, закомментирую. Обновляется только список подписок у пользователя.
        /*allSubscriptionList = JsonUtility.FromJson<MyList<Subscription>>(subscriptionsJsonFile.text);
        if (allSubscriptionList == null)
            allSubscriptionList = new MyList<Subscription>();

        if (allSubscriptionList.productList == null)
            allSubscriptionList.productList = new List<Subscription>();*/

        currentUserSubscriptionList = new List<Subscription>();
        var user = UserManagerScript.Instance.GetCurrentUser();
        foreach (Subscription i in allSubscriptionList.productList)
        {
            if (i.userLogin == user.login)
            {
                currentUserSubscriptionList.Add(i);
            }
        }
    }
    private void GetSubscriptionsFromJson()
    {
        allSubscriptionList = JsonUtility.FromJson<MyList<Subscription>>(subscriptionsJsonFile.text);
        if (allSubscriptionList == null)
            allSubscriptionList = new MyList<Subscription>();

        if (allSubscriptionList.productList == null)
            allSubscriptionList.productList = new List<Subscription>();
        UpdateCurrentUserSubscriptionList();
    }
    private void UpdateJsonFile()
    {
        string jsonNew = JsonUtility.ToJson(allSubscriptionList);
        Debug.Log("Updated subscriptions JSON " + jsonNew);
        File.WriteAllText(AssetDatabase.GetAssetPath(subscriptionsJsonFile), jsonNew);
        EditorUtility.SetDirty(subscriptionsJsonFile);
    }
}
