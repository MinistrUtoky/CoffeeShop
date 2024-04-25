using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Assets.Scripts.Database.DataStructures;
using Product = Assets.Scripts.Database.DataStructures.Product;

public class MainPageScript : MonoBehaviour
{
    [SerializeField] private Transform productButtonPrefab;
    [SerializeField] private Transform productScrollViewContent;
    [SerializeField] private Transform adPanelPrefab;

    public void UpdateProductButtons()
    {
        gameObject.SetActive(true); // :D

        //—ƒ≈À¿“‹  ¿  ƒŒ¡¿¬»Ã –»¿À “¿…Ã œŒƒ√–”« ” — ‘¿…À¿
        foreach (Transform child in productScrollViewContent)
        {
            Destroy(child.gameObject);
        }
        List<Product> productList = SubscriptionManagerScript.Instance.GetProductList();
        var adPanel=Instantiate(adPanelPrefab,productScrollViewContent);
        adPanel.gameObject.SetActive(true);
        foreach (Product product in productList)
        {
            var productButton = Instantiate(productButtonPrefab, productScrollViewContent);
            productButton.gameObject.SetActive(true);
            productButton.GetComponent<ProductButtonScript>().SetValues(product);
        }
    }

}
