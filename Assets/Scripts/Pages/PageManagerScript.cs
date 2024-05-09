using CorvusEnLignumDBSolutionsIncorporated;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Assets.Scripts.Database.DataStructures;
using ProductData = Assets.Scripts.Database.DataStructures.ProductData;

namespace PageManagement
{
    public class PageManagerScript : MonoBehaviour
    {
        [Serializable]
        private struct BottomPanelByUserType
        {
            public List<Transform> buyerButtons;
            public List<Transform> sellerButtons;
            public List<Transform> moderatorButtons;
        }
        public static PageManagerScript Instance { get; private set; }
        private GameObject currentPage;
        [SerializeField] 
        private GameObject starterPage;
        [SerializeField]
        private GameObject productPage;
        [SerializeField] 
        private GameObject editSubsciptionPage;
        [SerializeField]
        private GameObject techPagesSeparator;
        [SerializeField]
        private GameObject mainPage;
        [SerializeField]
        private RectTransform bottomPanel;
        [SerializeField]
        private BottomPanelByUserType bottomPanelPresets;
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
        public void ChangePageToAddToSubscription(ProductData product, Sprite image)
        {
            productPage.GetComponent<AddToSubscriptionPopUpScript>().SetValues(product,image);
            ChangeCurrentPage(productPage);
            Debug.Log("Showing Add to Subscription Pop up");
        }
        public void ChangePageToSubscriptionEditPage(Subscription subscription, Sprite image)
        {
            editSubsciptionPage.GetComponent<EditSubscriptionPopUpScript>().SetValues(subscription,image);
            SubscriptionManagerScript.Instance.RemoveSubscription(subscription); 
            ChangeCurrentPage(editSubsciptionPage);
            Debug.Log("Showing Sub Edit Popup");
        }
        public void HideAddToSubscriptionPopUp()
        {
            productPage.SetActive(false);
        }
        public void SwitchFromTechPagesToUsables()
        {
            ChangeCurrentPage(mainPage);
            techPagesSeparator.SetActive(false);
        }
        public void UpdateMainPageProductButtons()
        {
            mainPage.GetComponent<MainPageScript>().UpdateProductButtons();
        }
        public void ShowNavPanelAccordingToUserType(UserType userType)
        {
            Transform bg = bottomPanel.Find("BottomPanelBackground");
            foreach (Transform child in bottomPanel)
            {
                if (child != bg) {
                    child.gameObject.SetActive(false);
                }
            }

            List<Transform> bottomButtons = new List<Transform>();
            if (userType == UserType.Buyer)
                bottomButtons = bottomPanelPresets.buyerButtons;
            else if (userType == UserType.Seller)
                bottomButtons = bottomPanelPresets.sellerButtons;
            else if (userType == UserType.Moderator)
                bottomButtons = bottomPanelPresets.moderatorButtons;

            Vector3 canvasScale = bottomPanel.transform.GetComponentsInParent<Canvas>()[0].transform.localScale;
            for (int i = 0; i < bottomButtons.Count; i++)
            {              
                Debug.Log(bg.GetComponent<RectTransform>().rect.width);
                bottomButtons[i].position = new Vector3((i+1) * canvasScale.x * bg.GetComponent<RectTransform>().rect.width / (bottomButtons.Count+1),
                                                         canvasScale.y * bg.GetComponent<RectTransform>().rect.height / 2, 0);
                bottomButtons[i].gameObject.SetActive(true);
            }
        }   
        public void Exit()
        {
            MSSQLServerConnector.CloseDBConnection();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

