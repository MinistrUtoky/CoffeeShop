using System.Collections.Generic;
using System.ComponentModel.Design;
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

    private MyList<ProductData> allProductList;

    private Dictionary<string, float> currencyRates;
    private string _currentCurrency;

    [HideInInspector]
    public TextAsset subscriptionsJsonFile;
    [HideInInspector]
    public TextAsset productJsonFile;

    public string CurrentCurrency { get { return _currentCurrency; } }
    public string JsonSavingDirectory { get; set; }

    public MyList<Subscription> GetAllSubscriptionList() => allSubscriptionList;

    public List<Subscription> GetCurrentUserSubscriptionList() => currentUserSubscriptionList;

    public float GetConversionRate(string currency1, string currency2) => currencyRates[currency2] / currencyRates[currency1];

    public ProductData GetProduct(int id) => allProductList.list[id];

    public ProductData GetProduct(ProductData product) => allProductList.list.Find(x => x == product);

    public List<ProductData> GetProductList() => allProductList.list;

    private void Awake()
    {
        Instance = this;
        currencyRates = new Dictionary<string, float>();
        _currentCurrency = "Euro";

        UpdateConversionRates();
    }
    private void Start()
    {
        UserManagerScript.Instance.OnLogin.AddListener(UserManagerScript_OnLogin);
    }

    private void UserManagerScript_OnLogin()
    {
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
    
    public void AddNewProduct(ProductData newProduct)
    {
        allProductList.list.Add(newProduct);
        if (DatabaseManager.TryAddProduct(newProduct))
        {
            Debug.Log("Successfully added product to DB");
        }
        else
        {
            Debug.Log("Something went wrong during products insertion");
        }
        RetrieveProductsFromDB();
        //UpdateProductJsonFile();
    } 
    public void RetrieveProductsFromDB()
    {
        if (allProductList == null)
            allProductList = new MyList<ProductData>();
        if (DatabaseManager.TryRetrieveAllProducts(out allProductList.list))
        {
            Debug.Log("Successfully retrieved products from DB");
        }
        else
        {
            allProductList.list = new List<ProductData>();
            Debug.Log("Something went wrong during products retrieval");
        }
    }
    /*
     * 
    public void RemoveProduct(ProductData product)
    {
        allProductList.list.Remove(product);
        UpdateProductJsonFile();
    }
    private void GetProductsFromJson()
    {
        allProductList = JsonUtility.FromJson<MyList<ProductData>>(productJsonFile.text);
        if (allProductList == null) 
            allProductList = new MyList<ProductData>();
        if (allProductList.list == null)
            allProductList.list = new List<ProductData>();
        Debug.Log("Got products from JSON file");
    }    
    private void UpdateProductJsonFile()
    {
        string jsonNew = JsonUtility.ToJson(allProductList);
        File.WriteAllText(Path.Combine(JsonSavingDirectory, productJsonFile.name + ".json"), jsonNew);
        Debug.Log("Updated products JSON " + jsonNew);
    }
    */
    private void UpdateConversionRates()
    {
        // Дописать потом функцию автоматического обновления курсов
        currencyRates.Add("Euro",1f);
        currencyRates.Add("Dollar", 1.09f);
        currencyRates.Add("Ruble",100.85f);
        currencyRates.Add("Yuan",7.85f);
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
        File.WriteAllText(Path.Combine(JsonSavingDirectory, subscriptionsJsonFile.name + ".json"), jsonNew);
        Debug.Log("Updated products JSON " + jsonNew);
    }
}
