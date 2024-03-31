using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace PageManagement
{

    public class PageManagerScript : MonoBehaviour
    {
        public static PageManagerScript Instance { get; private set; }
        private GameObject currentPage;
        [SerializeField] 
        private GameObject starterPage;

        [SerializeField] private Sprite testImg;
        public GameObject CurrentScene { get { return currentPage; } }

        [SerializeField]public GameObject addToSubscriptionPopUp;

        private void Awake()
        {
            Instance = this;
            currentPage = starterPage;
            currentPage.SetActive(true);
            ShowAddToSubscriptionPopUp("êîôý","ÐÔÎËÏÛÐÎÏÐÔÛÎËÐÏÎËÔÐÛÎËÏÐÎËÔÐÛÎËÏÐËÎÔÐÛÎË",testImg,10);
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
        public void ShowAddToSubscriptionPopUp(string name,string description, Sprite image, float price)
        {
            addToSubscriptionPopUp.GetComponent<AddToSubscriptionPopUpScript>().SetValues(name,description,image,price);
            addToSubscriptionPopUp.SetActive(true);
            Debug.Log("Showing Add to Subscription Pop up");
        }
        public void HideAddToSubscriptionPopUp()
        {
            addToSubscriptionPopUp.SetActive(false);
        }
    }
}

