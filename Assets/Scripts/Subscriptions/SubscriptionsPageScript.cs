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
    public void UpdateSubscriptionButtons()
    {
        foreach (Transform child in SubscriptionScrollViewContent)
        {
            Destroy(child.gameObject);
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
