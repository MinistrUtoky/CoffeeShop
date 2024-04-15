using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using productRelated;
using UnityEngine.UI;
using PageManagement;
using Unity.VisualScripting;
using Product = productRelated.Product;

public class ProductPageOpenerScript : MonoBehaviour
{
    private PageManagerScript pageManager;
    [SerializeField]
    private Product productToAdd;
    [SerializeField]
    private Image productImage;

    private void Start()
    {
        pageManager = GameObject.FindWithTag("PageManager").GetComponent<PageManagerScript>();
    }
    public void ChangePageToProductPage()
        => pageManager.ChangePageToAddToSubscription(productToAdd, 
                    productImage.IsUnityNull()? null : productImage.sprite);
}
