using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubscriptionButtonScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private TextMeshProUGUI subscriptionEndDate;
    [SerializeField] private TextMeshProUGUI productAmount;
    public void SetValues(string productName, string subscriptionEndDate, int productAmount)
    {
        this.productName.text = productName;
        this.subscriptionEndDate.text = subscriptionEndDate;
        this.productAmount.text = productAmount.ToString();
    }
}
