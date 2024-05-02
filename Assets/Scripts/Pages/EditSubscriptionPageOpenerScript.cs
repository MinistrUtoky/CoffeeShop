using UnityEngine;
using UnityEngine.UI;
using PageManagement;
using Unity.VisualScripting;
using Product = Assets.Scripts.Database.DataStructures.Product;
using static Assets.Scripts.Database.DataStructures;

public class EditSubscriptionPageOpenerScript : MonoBehaviour
{
    private PageManagerScript pageManager;
    private Subscription subscription;
    [SerializeField] private Image productImage;

    private void Start()
    {
        pageManager = PageManagerScript.Instance;
        subscription = GetComponent<SubscriptionButtonScript>().GetSubscription();
    }
    public void ChangePageToSubscriptionEditPage()
        => pageManager.ChangePageToSubscriptionEditPage(subscription, 
                    productImage.IsUnityNull()? null : productImage.sprite);
}
