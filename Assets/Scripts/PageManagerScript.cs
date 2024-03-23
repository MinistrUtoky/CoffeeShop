using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PageManagement
{

    public class PageManagerScript : MonoBehaviour
    {
        public enum PageEnumerator { 
            MainPage,
            ProductPage,
            LoginPage,
            ProfilePage
        };
        public static PageManagerScript Instance { get; private set; }
        private GameObject currentPage;
        [SerializeField] private GameObject starterScene;
        [SerializeField]
        private List<GameObject> pages;

        public GameObject CurrentScene { get => currentPage; }
        private void Awake()
        {
            Instance = this;
            currentPage = starterScene;
            currentPage.SetActive(true);
            
        }
        public void ChangeCurrentPage(GameObject newPage)
        {
            currentPage.SetActive(false);
            newPage.SetActive(true);
            currentPage = newPage;
        }

        public void ChangeCurrentPage(PageEnumerator newPage)
        {
            currentPage.SetActive(false);
            pages[((int)newPage)].SetActive(true);
            currentPage = pages[((int)newPage)];
        }
    }
}
