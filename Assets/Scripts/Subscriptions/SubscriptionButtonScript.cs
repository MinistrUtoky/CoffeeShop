using productRelated;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.Database.DataStructures;

public class SubscriptionButtonScript : MonoBehaviour
{
    private Subscription _subscription;
    
    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private TextMeshProUGUI subscriptionEndDate;
    [SerializeField] private TextMeshProUGUI productAmount;
    [SerializeField] private Button deleteButton;

    public Subscription Subscription { get { return _subscription; } }

    public void SetValues(Subscription subscription)
    {
        try
        {
            this._subscription = subscription;
            this.productName.text = SubscriptionManagerScript.Instance.GetProduct(subscription.productID-1).name;
            this.subscriptionEndDate.text = subscription.subscriptionEnd;
            this.productAmount.text = subscription.productAmount.ToString();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Что то пошло не так в задаче значений\n{ex.Message}");
            //Destroy(gameObject);
        }
    }
    public Subscription GetSubscription()
    {
        return _subscription;
    }
    public void DeleteSubscription()
    {
        SubscriptionManagerScript.Instance.RemoveSubscription(_subscription);
        Destroy(gameObject);
    }
    public Button GetDeleteButton()
    {
        return deleteButton;
    }
}
