using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserManagement;
using static Assets.Scripts.Database.DataStructures;

public class ProductButtonScript : MonoBehaviour
{
    private Product _product;
    [SerializeField] private TextMeshProUGUI productNameText;
    [SerializeField] private TextMeshProUGUI productPriceText;
    [SerializeField] private Image productImage;
    public void SetValues(Product product)
    {
        try
        {
            _product = product;
            productNameText.text = product.name;
            productPriceText.text = $"Price:\n{product.price* SubscriptionManagerScript.Instance.GetConversionRate("Euro", UserManagerScript.Instance.GetCurrentUser().currency)} {UserManagerScript.Instance.GetCurrentUser().currency}s";
            productImage.sprite = GetImageFromURL(product.pictureURL);
        }
        catch
        {
            Debug.LogError("Что то пошло не так в задаче значений");
            Destroy(gameObject);
        }

    }
    private Sprite GetImageFromURL(string URL)
    {
        //написать функцию подгрузки пикчи по ссылке
        return null;
    }
    public Product GetProduct()
    {
        return _product;
    }
}
