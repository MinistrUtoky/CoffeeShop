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

    //��������, �������� ����� ������� ������ �������� �������� �����, �� �� ����� ������� � ������ ������ ������ �� ���� � ����������
    [SerializeField] public TextAsset productJsonFile;

    public Subscription Subscription { get { return _subscription; } }
    public void SetValues(Subscription subscription)
    {
        //�� �� �����, �������� ��� ������ �� �����������!!!
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
            Debug.LogError("��� �� ����� �� ��� � ������ ��������");
            Destroy(gameObject);
        }

    }
    public void Click()
    {
        SubscriptionManagerScript.Instance.RemoveSubscription(_subscription);
        Destroy(gameObject);
    }

}
