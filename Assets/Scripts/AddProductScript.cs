using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UserManagement;
using static Assets.Scripts.Database.DataStructures;

namespace productRelated
{
    public class AddProductScript : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField productName;
        [SerializeField]
        private TMP_InputField productDescription;
        [SerializeField]
        private TMP_InputField productPrice;

        private List<ProductData> products;
        private void Start()
        {
            UpdateProductsList();
        }
        public void AddProduct()
        {
            float numPrice=-1;
            float.TryParse(productPrice.text, out numPrice);
            ProductData product = new ProductData
            {
                id = products.Count,
                name = productName.text,
                description = productDescription.text,
                price = numPrice,
                //Временное решение пока не поменял меню добавления товара
                pictureURL = "https://cliparting.com/wp-content/uploads/2018/03/cool-pictures-2018-3.jpg",
                vendorUserLogin = UserManagerScript.Instance.CurrentUser.login
            };
            SubscriptionManagerScript.Instance.AddNewProduct(product);
        }
        public void UpdateProductsList()
        {
            products = SubscriptionManagerScript.Instance.GetProductList();
        }
    }
}