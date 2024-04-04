using UnityEngine;
using TMPro;
using UnityEngine.UI;
using productRelated;
using UserManagement;
using System;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using System.Collections.Generic;

public class AddToSubscriptionPopUpScript : MonoBehaviour
{
    private float price;
    private int amount=1;
    private productRelated.Product product;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI priceVisual;
    [SerializeField] private TMP_InputField amountInputField;
    [SerializeField] private TMP_Dropdown renewalPeriodDropdown;
    [SerializeField] public TextAsset jsonFile;
    [SerializeField] private SubList<Subscription> subscriptions;

    private void Awake()
    {
        subscriptions = JsonUtility.FromJson<SubList<Subscription>>(jsonFile.text);
        Debug.Log(subscriptions==null);
        if (subscriptions == null) subscriptions = new SubList<Subscription>();
        if (subscriptions.subscriptionList==null)
        {
            subscriptions.subscriptionList = new List<Subscription>();
        }
    }
    public void SetValues(productRelated.Product product ,Sprite image)
    {
        this.product= product;
        amount = 1;
        productName.text = product.name;
        this.description.text = product.description;
        this.price = product.price;
        this.image.sprite = image;
        float totalPrice = price;
        amountInputField.text = amount.ToString();
        priceVisual.text = totalPrice.ToString();
    }
    public void UpdatePriceVisual()
    {
        float totalPrice = price * amount;
        priceVisual.text = totalPrice.ToString();
    }
    public void AddAmount()
    {
        amount++;
        amountInputField.text= amount.ToString();
        UpdatePriceVisual();
    }
    public void RemoveAmount()
    {
        if(amount-1>0)
        {
            amount--;
            amountInputField.text = amount.ToString();
            UpdatePriceVisual();
        }
    }
    public void AddSubscription()
    {
        UserManagement.UserManagerScript.User user = UserManagerScript.Instance.GetCurrentUser();
        string renewalPeriod = renewalPeriodDropdown.options[renewalPeriodDropdown.value].text;
        DateTime nextRenewalDate = DateTime.Now;
        switch (renewalPeriod)
        {
            case "Day":
                {
                    nextRenewalDate.AddDays(1);
                }
                break;
            case "Week":
                {
                    nextRenewalDate.AddDays(7);
                }
                break;
            case "Month":
                {
                    nextRenewalDate.AddDays(31);
                }
                break;
            case "Year":
                {
                    nextRenewalDate.AddDays(365);
                }
                break;
        }
        Subscription newSubscription = new Subscription { userLogin = user.login, subscriptionStart = DateTime.Now.ToString(),
            subscriptionEnd = nextRenewalDate.ToString(),
            productID = product.id,
            productAmount=amount
        };
        Debug.Log(subscriptions.subscriptionList);
        subscriptions.subscriptionList.Add(newSubscription);
        SaveSubscriptionIntoJson();
    }
    private void SaveSubscriptionIntoJson()
    {
        string jsonNew = JsonUtility.ToJson(subscriptions);
        Debug.Log("Added to subscriptions JSON " + jsonNew);
        File.WriteAllText(AssetDatabase.GetAssetPath(jsonFile), jsonNew);
        EditorUtility.SetDirty(jsonFile);
    }
}
