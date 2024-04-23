using productRelated;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Assets.Scripts.Database.DataStructures;

public class SubscriptionButtonScript : MonoBehaviour
{
    private Subscription _subscription;
    
    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private TextMeshProUGUI subscriptionEndDate;
    [SerializeField] private TextMeshProUGUI productAmount;


    public Subscription Subscription { get { return _subscription; } }
    public void SetValues(Subscription subscription)
    {
        try
        {
            this._subscription = subscription;
            this.productName.text = SubscriptionManagerScript.Instance.GetProduct(subscription.productID).name;
            this.subscriptionEndDate.text = subscription.subscriptionEnd;
            this.productAmount.text = subscription.productAmount.ToString();
        }
        catch
        {
            Debug.LogError("Что то пошло не так в задаче значений");
            Destroy(gameObject);
        }

    }
    public void Click()
    {
        SubscriptionManagerScript.Instance.RemoveSubscription(_subscription);
        Destroy(gameObject);
    }

}
