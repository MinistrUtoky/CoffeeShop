using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.Database.DataStructures;

public class MainPageScript : MonoBehaviour
{
    [SerializeField] private Transform ProductButtonPrefab;
    [SerializeField] private Transform ProductScrollViewContent;
    public void UpdateProductButtons()
    {
        foreach (Transform child in ProductScrollViewContent)
        {
            Destroy(child.gameObject);
        }
        List<Product> productList = SubscriptionManagerScript.Instance.GetProductList();
        foreach (var product in productList)
        {
            var productButton = Instantiate(ProductButtonPrefab, ProductScrollViewContent);
            productButton.GetComponent<ProductButtonScript>().SetValues(product);
            productButton.gameObject.SetActive(true);
        }
    }
}
