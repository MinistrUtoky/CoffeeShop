using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UserManagement;
using System;
using static Assets.Scripts.Database.DataStructures;

public class AddToSubscriptionPopUpScript : MonoBehaviour
{
    private float price;
    private int amount=1;
    private ProductData product;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI priceVisual;
    [SerializeField] private TMP_InputField amountInputField;
    [SerializeField] private TMP_Dropdown renewalPeriodDropdown;
    [SerializeField] private TMP_Dropdown currencyDropdown;

    private void Awake()
    {
        
    }
    public void SetValues(ProductData product ,Sprite image)
    {
        this.product= product;
        amount = 1;
        productName.text = product.name;
        this.description.text = product.description;
        this.price = product.price;
        this.image.sprite = image;
        amountInputField.text = amount.ToString();
        UpdatePriceVisual();
    }
    public void UpdatePriceVisual()
    {
        string selectedCurrency = currencyDropdown.options[currencyDropdown.value].text;
        float totalPrice = price * amount * SubscriptionManagerScript.Instance.GetConversionRate
            (UserManagerScript.Instance.GetCurrentUser().currency, selectedCurrency);
        priceVisual.text = $"{totalPrice.ToString()} {selectedCurrency}s";
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
        UserData user = UserManagerScript.Instance.GetCurrentUser();
        string renewalPeriod = renewalPeriodDropdown.options[renewalPeriodDropdown.value].text;
        DateTime nextRenewalDate;
        switch (renewalPeriod)
        {
            case "Day":
                nextRenewalDate=DateTime.Now.AddDays(1);
                break;
            case "Week":
                nextRenewalDate=DateTime.Now.AddDays(7);
                break;
            case "Month":
                nextRenewalDate=DateTime.Now.AddDays(31);
                break;
            case "Year":
                nextRenewalDate = DateTime.Now.AddDays(365);
                break;
            default:
                nextRenewalDate= DateTime.Now;
                break;
        }
        Subscription newSubscription = new Subscription 
        {   
            userLogin = user.login, 
            subscriptionStart = DateTime.Now.ToString(),
            subscriptionEnd = nextRenewalDate.ToString(),
            productID = product.id,
            productAmount=amount
        };
        SubscriptionManagerScript.Instance.AddSubscription(newSubscription);
    }

}
