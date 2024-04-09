using productRelated;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UserManagement;
using Product = productRelated.Product;

public class SubscriptionManagerScript : MonoBehaviour
{
    public static SubscriptionManagerScript Instance { get; private set; }

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
    }

    private void UserManagerScript_OnLogin()
    {
        Debug.Log(UserManagerScript.Instance.GetCurrentUser().login);
        _currentCurrency = UserManagerScript.Instance.GetCurrentUser().currency;
        GetSubscriptionsFromJson();
    }
    public void AddSubscription(Subscription subscription)
    {
        allSubscriptionList.productList.Add(subscription);
        UpdateSubscriptionJsonFile();
        UpdateCurrentUserSubscriptionList();
        OnSubscriptionRelatedChanges?.Invoke();
    }
    public void RemoveSubscription(Subscription subscription)
    {
        allSubscriptionList.productList.Remove(subscription);
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
        // ���� �� ������ ������ � ����������� ���������� �����, �������������. ����������� ������ ������ �������� � ������������.
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
    public float GetConversionRate(string currency1, string currency2)
    {
        return currencyRates[currency2]/currencyRates[currency1];
    }
    public Product GetProduct(int id)
    {
        return allProductList.productList[id];
    }
    public void AddNewProduct(Product newProduct)
    {
        allProductList.productList.Add(newProduct);
        UpdateProductJsonFile();
    } 
    public void RemoveProduct(Product product)
    {
        allProductList.productList.Remove(product);
        UpdateProductJsonFile();
    }
    private void GetProductsFromJson()
    {
        allProductList = JsonUtility.FromJson<MyList<Product>>(productJsonFile.text);
        if (allProductList == null) 
            allProductList = new MyList<Product>();
        if (allProductList.productList == null)
        {
            allProductList.productList = new List<Product>();
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

        // �������� ����� ������� ��������������� ���������� ������
        currencyRates.Add("Euro",1f);
        currencyRates.Add("Dollar", 1.09f);
        currencyRates.Add("Ruble",100.85f);
        currencyRates.Add("Yuan",7.85f);
        Debug.Log(currencyRates);
    }
}
