using productRelated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using productRelated;
using UserManagement;
using System;
using UnityEngine.UI;

public class SubscriptionsPageScript : MonoBehaviour
{
    [SerializeField] private Transform SubscriptionButtonPrefab;
    [SerializeField] private Transform SubscriptionScrollViewContent;
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void UpdateSubscriptionButtons()
    {
        while(SubscriptionScrollViewContent.childCount>0)
        {
            Destroy(SubscriptionScrollViewContent.GetChild(0));
        }
        List<Subscription> currentUserSubscriptionList = SubscriptionManagerScript.Instance.GetCurrentUserSubscriptionList();
        foreach (var subscription in currentUserSubscriptionList)
        {
            var subscriptionButton = Instantiate(SubscriptionButtonPrefab, SubscriptionScrollViewContent);
            subscriptionButton.GetComponent<SubscriptionButtonScript>().SetValues(subscription);
            // Даже если пишем GetComponentInChildren оно добавляет лисенер в обычную кнопку(((
            subscriptionButton.GetComponent<Button>().onClick.AddListener(()=>subscriptionButton.GetComponent<SubscriptionButtonScript>().Click());
            subscriptionButton.gameObject.SetActive(true);
        }
    }
}
