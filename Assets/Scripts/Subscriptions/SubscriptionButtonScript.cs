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

    //��������, �������� ����� ������� ������ �������� �������� �����, �� �� ����� ������� � ������ ������ ������ �� ���� � ����������
    [SerializeField] public TextAsset productJsonFile;

    public Subscription Subscription { get { return _subscription; } }
    public void SetValues(Subscription subscription)
    {
        //�� �� �����, �������� ��� ������ �� �����������!!!
        MyList<Product>products = JsonUtility.FromJson<MyList<Product>>(productJsonFile.text);
        if (products == null) products = new MyList<Product>();
        if (products.list == null)
        {
            products.list = new List<Product>();
        }
        try
        {
            this._subscription = subscription;
            this.productName.text = products.list[subscription.productID].name;
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
