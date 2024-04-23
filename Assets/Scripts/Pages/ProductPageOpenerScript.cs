using UnityEngine;
using UnityEngine.UI;
using PageManagement;
using Unity.VisualScripting;
using Product = Assets.Scripts.Database.DataStructures.Product;

public class ProductPageOpenerScript : MonoBehaviour
{
    private PageManagerScript pageManager;
    private Product productToAdd;
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
