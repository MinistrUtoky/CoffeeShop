using CorvusEnLignumDBSolutionsIncorporated;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UserManagement;
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
        [Serializable]
        private struct MainPageVariables
        {
            public Transform productButtonPrefab;
            public Transform productScrollViewContent;
            public Transform adPanelPrefab;
        }
        [Serializable]
        private struct SubscriptionPageVariables
        {
            public Transform SubscriptionButtonPrefab;
            public Transform SubscriptionScrollViewContent;
        }
        [Serializable]
        private struct ProfilePageVariables
        {
            public Image profilePicture;
            public TextMeshProUGUI customUserName;
        }
        [Serializable]
        private struct EditProfilePageVariables
        {
            public TMP_InputField customUserNameInputField;
            public Transform profilePictureSelectorContentTransform;
            public Transform profilePictureButtonPrefab;
            public TMP_Dropdown currencyDropdown;
        }
        [Serializable]
        private struct Pages
        {
            public GameObject starterPage;
            public GameObject productPage;
            public GameObject editSubsciptionPage;
            public GameObject mainPage;
            public GameObject subscriptionsPage;
            public GameObject editProfilePage;
            public GameObject profilePage;

        }
        [Serializable]
        private struct ApprovablesVariables
        {
            public Transform approvablesScrollView;
            public GameObject approvablePrefab;
        }
        public static PageManagerScript Instance { get; private set; }
        private GameObject currentPage;
        [SerializeField]
        private GameObject techPagesSeparator;
        [SerializeField]
        private SubscriptionPageVariables subscriptionPageVariables;
        [SerializeField] 
        private MainPageVariables mainPageVariables;
        [SerializeField]
        private RectTransform bottomPanel;
        [SerializeField]
        private BottomPanelByUserType bottomPanelPresets;

        [SerializeField]
        private ProfilePageVariables profilePageVariables;
        [SerializeField]
        private Pages pages;
        [SerializeField]
        private EditProfilePageVariables editProfilePageVariables;
        [SerializeField]
        private ApprovablesVariables approvablesVariables;
        [SerializeField]
        private GameObject helpButton;
        [SerializeField]
        private GameObject helpPopUp;
        public GameObject CurrentScene { get { return currentPage; } }
        private void Awake()
        {
            Instance = this;
            currentPage = pages.starterPage;
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
                        if (newPage == pages.mainPage)
                        {
                            UpdateMainPage();
                            helpButton.SetActive(true);
                        }
                        if (newPage == pages.profilePage)
                        {

                            helpButton.SetActive(true);
                        }
                        if (newPage == pages.subscriptionsPage)
                        {
                            UpdateSubscriptionButtons();
                            helpButton.SetActive(true);
                        }
                        if(newPage == pages.editProfilePage)
                        {
                            UpdateEditProfilePage();
                        }
                        if(newPage ==  pages.profilePage)
                        {
                            UpdateProfilePage();
                        }
                        currentPage = newPage;
                    }
                }
            }
        }
        public void ChangePageToAddToSubscription(ProductData product, Sprite image)
        {
            pages.productPage.GetComponent<AddToSubscriptionPopUpScript>().SetValues(product,image);
            helpButton.SetActive(false);
            ChangeCurrentPage(pages.productPage);
        }
        public void ChangePageToSubscriptionEditPage(Subscription subscription, Sprite image)
        {
            pages.editSubsciptionPage.GetComponent<EditSubscriptionPopUpScript>().SetValues(subscription,image);
            helpButton.SetActive(false);
            SubscriptionManagerScript.Instance.RemoveSubscription(subscription); 
            ChangeCurrentPage(pages.editSubsciptionPage);
        }
        public void HideAddToSubscriptionPopUp()
        {
            pages.productPage.SetActive(false);
        }
        public void SwitchFromTechPagesToUsables()
        {
            ChangeCurrentPage(pages.mainPage);
            techPagesSeparator.SetActive(false);
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
        public void SaveProfileEdits()
        {
            UserManagerScript.Instance.SetCustomUserName(editProfilePageVariables.customUserNameInputField.text);
            var user = UserManagerScript.Instance.GetCurrentUser();
            user.currency =editProfilePageVariables.currencyDropdown.options[editProfilePageVariables.currencyDropdown.value].text;
        }
        public void UpdateProfilePage()
        {
            profilePageVariables.customUserName.text =UserManagerScript.Instance.GetCustomUserName();
            profilePageVariables.profilePicture.sprite=UserManagerScript.Instance.GetPicture(UserManagerScript.Instance.GetPictureId());
        }
        public void UpdateEditProfilePage()
        {
            editProfilePageVariables.customUserNameInputField.text = UserManagerScript.Instance.GetCustomUserName();
            foreach (Transform child in editProfilePageVariables.profilePictureSelectorContentTransform)
            {
                Destroy(child.gameObject);
            }
            foreach (Sprite profilePicture in UserManagerScript.Instance.GetProfilePictureList())
            {
                Transform profilePictureButtonTransform = Instantiate(editProfilePageVariables.profilePictureButtonPrefab,editProfilePageVariables.profilePictureSelectorContentTransform);
                profilePictureButtonTransform.GetComponent<Image>().sprite = profilePicture;
            }
        }
        public void UpdateMainPage()
        {
            pages.mainPage.SetActive(true); 
            SubscriptionManagerScript.Instance.RetrieveProductsFromDB();

            foreach (Transform child in mainPageVariables.productScrollViewContent)
            {
                Destroy(child.gameObject);
            }
            List<ProductData> productList = SubscriptionManagerScript.Instance.GetProductList();
            var adPanel = Instantiate(mainPageVariables.adPanelPrefab, mainPageVariables.productScrollViewContent);
            adPanel.gameObject.SetActive(true);
            foreach (ProductData product in productList)
            {
                var productButton = Instantiate(mainPageVariables.productButtonPrefab, mainPageVariables.productScrollViewContent);
                productButton.gameObject.SetActive(true);
                productButton.GetComponent<ProductButtonScript>().SetValues(product);
            }
        }
        public void UpdateSubscriptionButtons()
        {
            foreach (Transform child in subscriptionPageVariables.SubscriptionScrollViewContent)
            {
                Destroy(child.gameObject);
            }

            List<Subscription> currentUserSubscriptionList = SubscriptionManagerScript.Instance.GetCurrentUserSubscriptionList();
            foreach (var subscription in currentUserSubscriptionList)
            {
                var subscriptionButton = Instantiate(subscriptionPageVariables.SubscriptionButtonPrefab, subscriptionPageVariables.SubscriptionScrollViewContent);
                subscriptionButton.GetComponent<SubscriptionButtonScript>().SetValues(subscription);
                subscriptionButton.gameObject.SetActive(true);
            }
        }
        public void Exit()
        {
            MSSQLServerConnector.CloseDBConnection();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        public void ShowHelp()
        {
            helpPopUp.SetActive(true);
            helpButton.SetActive(false);
        }
        public void HideHelp()
        {
            helpPopUp.SetActive(false);
            helpButton.SetActive(true);
        }
        public void FillApprovalPage()
        {
            List<ProductData> approvables;
            if (DatabaseManager.TryRetrieveApprovables(out approvables))
            {
                foreach (Transform child in approvablesVariables.approvablesScrollView.GetComponent<ScrollRect>().content)
                    Destroy(child.gameObject);

                foreach (ProductData approvable in approvables)
                {
                    GameObject approvableWidget = Instantiate(approvablesVariables.approvablePrefab,
                                                              approvablesVariables.approvablesScrollView.GetComponent<ScrollRect>().content);
                    approvableWidget.GetComponent<ApprovableScript>().SetValues(approvable);
                }
                Debug.Log("Approvables successfully retrieved");
            }
            else
            {
                Debug.LogError("Couldn't open approvables");
            }
        }
    }
}

