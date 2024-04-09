using productRelated;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SubscriptionButtonScript : MonoBehaviour
{
    private Subscription _subscription;
    
    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private TextMeshProUGUI subscriptionEndDate;
    [SerializeField] private TextMeshProUGUI productAmount;

    //бпелеммн, бшохкхрэ йнцдю ядекюел яоняна онксвюрэ опндсйрш ксвье, лш ме асдел упюмхрэ б йюфдни ймнойе яяшкйс мю тюик я опндсйрюлх
    [SerializeField] public TextAsset productJsonFile;

    public Subscription Subscription { get { return _subscription; } }
    public void SetValues(Subscription subscription)
    {
        //рн фе яюлне, бшохкхрэ опх оепбни фе бнглнфмнярх!!!
        MyList<productRelated.Product>products = JsonUtility.FromJson<MyList<productRelated.Product>>(productJsonFile.text);
        if (products == null) products = new MyList<productRelated.Product>();
        if (products.productList == null)
        {
            products.productList = new List<productRelated.Product>();
        }
        try
        {
            this._subscription = subscription;
            this.productName.text = products.productList[subscription.productID].name;
            this.subscriptionEndDate.text = subscription.subscriptionEnd;
            this.productAmount.text = subscription.productAmount.ToString();
        }
        catch
        {
            Debug.LogError("вРН РН ОНЬКН МЕ РЮЙ Б ГЮДЮВЕ ГМЮВЕМХИ");
            Destroy(gameObject);
        }

    }
    public void Click()
    {
        SubscriptionManagerScript.Instance.RemoveSubscription(_subscription);
        Destroy(gameObject);
    }

}
