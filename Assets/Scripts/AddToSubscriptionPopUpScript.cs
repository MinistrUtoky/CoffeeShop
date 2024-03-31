using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddToSubscriptionPopUpScript : MonoBehaviour
{
    private float price;
    private int quantity=1;
    [SerializeField] private GameObject description;
    [SerializeField] private GameObject productName;
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject priceVisual;
    [SerializeField] private GameObject quantityInputField;
    public void SetValues(string name, string description, Sprite image, float price)
    {
        quantity = 1;
        productName.GetComponent<TextMeshProUGUI>().text = name;
        this.description.GetComponent<TextMeshProUGUI>().text= description;
        this.price = price;
        this.image.GetComponent<Image>().sprite=image;
        float totalPrice=price;
        quantityInputField.GetComponent<TMP_InputField>().text = quantity.ToString();
        priceVisual.GetComponent<TextMeshProUGUI>().text =totalPrice.ToString();
        FillSubscriptionDropdown();
    }
    public void UpdateQuantity()
    {
        if(int.TryParse(quantityInputField.GetComponent<TMP_InputField>().text, out quantity))
        {
            UpdatePriceVisual();
        }
    }
    public void UpdatePriceVisual()
    {
        float totalPrice = price * quantity;
        priceVisual.GetComponent<TextMeshProUGUI>().text = totalPrice.ToString();
    }
    public void AddQuantity()
    {
        quantity++;
        quantityInputField.GetComponent<TMP_InputField> ().text= quantity.ToString();
        UpdatePriceVisual();
    }
    public void RemoveQuantity()
    {
        if(quantity-1>0)
        {
            quantity--;
            quantityInputField.GetComponent<TMP_InputField>().text = quantity.ToString();
            UpdatePriceVisual();
        }
    }
    public void FillSubscriptionDropdown()
    {
         //Написать метод заполнения дропдауна списком подписок, пренадлежащих текущему пользователю
    }
}
