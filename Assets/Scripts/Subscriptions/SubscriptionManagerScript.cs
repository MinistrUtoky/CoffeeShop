using productRelated;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UserManagement;
using static Assets.Scripts.Database.DataStructures;

public class SubscriptionManagerScript : MonoBehaviour
{
    public static SubscriptionManagerScript Instance { get; private set; }
    [HideInInspector]

    public UnityEvent OnSubscriptionRelatedChanges;

    private MyList<Subscription> allSubscriptionList;
    private List<Subscription> currentUserSubscriptionList;

    private MyList<Product> allProductList;

    private Dictionary<string, float> currencyRates;
    private string _currentCurrency;

    [SerializeField] public TextAsset subscriptionsJsonFile;
    [SerializeField] public TextAsset productJsonFile;

    public string CurrentCurrency { get { return _currentCurrency; } }

    private void Awake()
    {
        Instance = this;
        currencyRates = new Dictionary<string, float>();

        _currentCurrency = "Euro";
        UpdateConversionRates();
        GetProductsFromJson();
    }
    private void Start()
    {
        UserManagerScript.Instance.OnLogin.AddListener(UserManagerScript_OnLogin);
        //GetProductsFromJson();
    }

    private void UserManagerScript_OnLogin()
    {
        Debug.Log(UserManagerScript.Instance.GetCurrentUser().login);
        _currentCurrency = UserManagerScript.Instance.GetCurrentUser().currency;
        GetSubscriptionsFromJson();
    }
    public void AddSubscription(Subscription subscription)
    {
        allSubscriptionList.list.Add(subscription);
        UpdateSubscriptionJsonFile();
        UpdateCurrentUserSubscriptionList();
        OnSubscriptionRelatedChanges?.Invoke();
    }
    public void RemoveSubscription(Subscription subscription)
    {
        allSubscriptionList.list.Remove(subscription);
        UpdateSubscriptionJsonFile();
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

        if (allSubscriptionList.list == null)
            allSubscriptionList.list = new List<Subscription>();*/

        currentUserSubscriptionList = new List<Subscription>();
        var user = UserManagerScript.Instance.GetCurrentUser();
        foreach (Subscription i in allSubscriptionList.list)
        {
            if (i.userLogin == user.login)
            {
                currentUserSubscriptionList.Add(i);
            }
        }
    }
    public float GetConversionRate(string currency1, string currency2)
    {
        return currencyRates[currency2]/currencyRates[currency1];
    }
    public Product GetProduct(int id)
    {
        return allProductList.list[id];
    }
    public Product GetProduct(Product product)
    {
        return allProductList.list.Find(x => x == product); 
    }
    public List<Product> GetProductList()
    {
        return allProductList.list;
    }
    public void AddNewProduct(Product newProduct)
    {
        allProductList.list.Add(newProduct);
        UpdateProductJsonFile();
    } 
    public void RemoveProduct(Product product)
    {
        allProductList.list.Remove(product);
        UpdateProductJsonFile();
    }
    private void GetProductsFromJson()
    {
        allProductList = JsonUtility.FromJson<MyList<Product>>(productJsonFile.text);
        if (allProductList == null) 
            allProductList = new MyList<Product>();
        if (allProductList.list == null)
        {
            allProductList.list = new List<Product>();
        }
        Debug.Log("Got products from JSON file");
    }
    private void GetSubscriptionsFromJson()
    {
        allSubscriptionList = JsonUtility.FromJson<MyList<Subscription>>(subscriptionsJsonFile.text);
        if (allSubscriptionList == null)
            allSubscriptionList = new MyList<Subscription>();

        if (allSubscriptionList.list == null)
            allSubscriptionList.list = new List<Subscription>();
        UpdateCurrentUserSubscriptionList();
    }
    private void UpdateSubscriptionJsonFile()
    {
        string jsonNew = JsonUtility.ToJson(allSubscriptionList);
        Debug.Log("Updated subscriptions JSON " + jsonNew);
        File.WriteAllText(AssetDatabase.GetAssetPath(subscriptionsJsonFile), jsonNew);
        EditorUtility.SetDirty(subscriptionsJsonFile);
    }
    private void UpdateProductJsonFile()
    {
        string jsonNew = JsonUtility.ToJson(allProductList);
        Debug.Log("Updated products JSON " + jsonNew);
        File.WriteAllText(AssetDatabase.GetAssetPath(productJsonFile), jsonNew);
        EditorUtility.SetDirty(productJsonFile);
    }
    private void UpdateConversionRates()
    {

        // Дописать потом функцию автоматического обновления курсов
        currencyRates.Add("Euro",1f);
        currencyRates.Add("Dollar", 1.09f);
        currencyRates.Add("Ruble",100.85f);
        currencyRates.Add("Yuan",7.85f);
    }

}
