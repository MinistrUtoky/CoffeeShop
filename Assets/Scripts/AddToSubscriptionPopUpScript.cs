using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddToSubscriptionPopUpScript : MonoBehaviour
{
    private float price;
    private int quantity=1;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI productName;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI priceVisual;
    [SerializeField] private TMP_InputField quantityInputField;
    public void SetValues(string name, string description, Sprite image, float price)
    {
        quantity = 1;
        productName.text = name;
        this.description.text= description;
        this.price = price;
        this.image.sprite=image;
        float totalPrice=price;
        quantityInputField.text = quantity.ToString();
        priceVisual.text =totalPrice.ToString();
    }
    private void UpdatePriceVisual()
    {
        float totalPrice = price * quantity;
        priceVisual.text = totalPrice.ToString();
    }
    public void AddQuantity()
    {
        quantity++;
        quantityInputField.text= quantity.ToString();
        UpdatePriceVisual();
    }
    public void RemoveQuantity()
    {
        if(quantity-1>0)
        {
            quantity--;
            quantityInputField.text = quantity.ToString();
            UpdatePriceVisual();
        }
    }
}
