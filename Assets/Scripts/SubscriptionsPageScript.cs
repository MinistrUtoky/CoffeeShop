using productRelated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using productRelated;
using UserManagement;
using System;

public class SubscriptionsPageScript : MonoBehaviour
{
    [SerializeField] public TextAsset subscriptionJsonFile;
    [SerializeField] public TextAsset productJsonFile;
    [SerializeField] private Transform SubscriptionButtonPrefab;
    [SerializeField] private Transform SubscriptionScrollViewContent;
    [SerializeField] private SubList<Subscription> subscriptions;
    [SerializeField] private MyList<Product> products;
    [SerializeField] private List<Subscription> userSubscriptions;
    [SerializeField] private UserManagement.UserManagerScript.User currentUser;
    private void Awake()
    {
        products = JsonUtility.FromJson<MyList<Product>>(productJsonFile.text);
        if (products == null) products = new MyList<Product>();
        if (products.productList == null)
        {
            products.productList = new List<Product>();
        }
        subscriptions = JsonUtility.FromJson<SubList<Subscription>>(subscriptionJsonFile.text);
        Debug.Log(subscriptions == null);
        if (subscriptions == null) subscriptions = new SubList<Subscription>();
        if (subscriptions.subscriptionList == null)
        {
            subscriptions.subscriptionList = new List<Subscription>();
        }
        currentUser = UserManagerScript.Instance.GetCurrentUser();
        userSubscriptions = GetUserSubscriptions();

    }
    void Start()
    {
        foreach (var subscription in userSubscriptions)
        {
            var subscriptionButton = Instantiate(SubscriptionButtonPrefab,SubscriptionScrollViewContent);
            subscriptionButton.GetComponent<SubscriptionButtonScript>().SetValues(products.productList[subscription.productID].name,subscription.subscriptionEnd,subscription.productAmount);
            subscriptionButton.gameObject.SetActive(true);
        }
    }
    void Update()
    {
        
    }
    private List<Subscription> GetUserSubscriptions()
    {
        List<Subscription> userSubscriptions = new List<Subscription>();
        foreach (Subscription i in subscriptions.subscriptionList)
        {
            if (i.userLogin == currentUser.login)
            {
                userSubscriptions.Add(i);
            }
        }
        return userSubscriptions;
    }
}
