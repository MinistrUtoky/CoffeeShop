using UnityEngine;
using UnityEngine.UI;
using PageManagement;
using Unity.VisualScripting;
using ProductData = Assets.Scripts.Database.DataStructures.ProductData;

public class ProductPageOpenerScript : MonoBehaviour
{
    private PageManagerScript pageManager;
    private ProductData productToAdd;
    [SerializeField] private Image productImage;

    private void Start()
    {
        pageManager = PageManagerScript.Instance;
        productToAdd = GetComponent<ProductButtonScript>().GetProduct();
    }
    public void ChangePageToProductPage()
        => pageManager.ChangePageToAddToSubscription(productToAdd, 
                    productImage.IsUnityNull()? null : productImage.sprite);
}
