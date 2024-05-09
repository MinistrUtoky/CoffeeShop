using System.Collections.Generic;
using UnityEngine;
using ProductData = Assets.Scripts.Database.DataStructures.ProductData;

public class MainPageScript : MonoBehaviour
{
    [SerializeField] private Transform productButtonPrefab;
    [SerializeField] private Transform productScrollViewContent;
    [SerializeField] private Transform adPanelPrefab;

    public void UpdateProductButtons()
    {
        gameObject.SetActive(true); // :D
        SubscriptionManagerScript.Instance.RetrieveProductsFromDB();

        //—ƒ≈À¿“‹  ¿  ƒŒ¡¿¬»Ã –»¿À “¿…Ã œŒƒ√–”« ” — ‘¿…À¿
        foreach (Transform child in productScrollViewContent)
        {
            Destroy(child.gameObject);
        }
        List<ProductData> productList = SubscriptionManagerScript.Instance.GetProductList();
        var adPanel=Instantiate(adPanelPrefab,productScrollViewContent);
        adPanel.gameObject.SetActive(true);
        foreach (ProductData product in productList)
        {
            var productButton = Instantiate(productButtonPrefab, productScrollViewContent);
            productButton.gameObject.SetActive(true);
            productButton.GetComponent<ProductButtonScript>().SetValues(product);
        }
    }

}
