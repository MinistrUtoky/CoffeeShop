using UnityEngine;
using UnityEngine.UI;
using PageManagement;
using Unity.VisualScripting;
using Product = Assets.Scripts.Database.DataStructures.Product;

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
