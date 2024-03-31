
using productRelated;
using UnityEngine;

namespace PageManagement
{
    public class PageManagerScript : MonoBehaviour
    {
        public static PageManagerScript Instance { get; private set; }
        private GameObject currentPage;
        [SerializeField] 
        private GameObject starterPage;
        [SerializeField]
        private GameObject productPage;
        public GameObject CurrentScene { get { return currentPage; } }

        private void Awake()
        {
            Instance = this;
            currentPage = starterPage;
            currentPage.SetActive(true);
        }
        public void ChangeCurrentPage(GameObject newPage)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase != TouchPhase.Moved)
                {
                    Debug.Log($"Changed page to {newPage.name}");
                    if (newPage != null)
                    {
                        currentPage.SetActive(false);
                        newPage.SetActive(true);
                        currentPage = newPage;
                    }
                }
            }
        }
        public void ShowAddToSubscriptionPopUp(Product product, Sprite image)
        {
            productPage.GetComponent<AddToSubscriptionPopUpScript>().SetValues(product.name, product.description, image, product.price);
            ChangeCurrentPage(productPage);
            Debug.Log("Showing Add to Subscription Pop up");
        }
        public void HideAddToSubscriptionPopUp()
        {
            productPage.SetActive(false);
        }
    }
}

