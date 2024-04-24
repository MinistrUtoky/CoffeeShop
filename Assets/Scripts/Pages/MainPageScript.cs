using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UserManagement;
using static Assets.Scripts.Database.DataStructures;

public class MainPageScript : MonoBehaviour
{
    [SerializeField] private Transform productButtonPrefab;
    [SerializeField] private Transform productScrollViewContent;
    [SerializeField] private Transform adPanelPrefab;

    public void UpdateProductButtons()
    {
        //—ƒ≈À¿“‹  ¿  ƒŒ¡¿¬»Ã –»¿À “¿…Ã œŒƒ√–”« ” — ‘¿…À¿
        foreach (Transform child in productScrollViewContent)
        {
            Destroy(child.gameObject);
        }
        List<Product> productList = SubscriptionManagerScript.Instance.GetProductList();
        var adPanel=Instantiate(adPanelPrefab,productScrollViewContent);
        adPanel.gameObject.SetActive(true);
        foreach (var product in productList)
        {
            var productButton = Instantiate(productButtonPrefab, productScrollViewContent);
            productButton.GetComponent<ProductButtonScript>().SetValues(product);
            productButton.gameObject.SetActive(true);
        }
    }
}
