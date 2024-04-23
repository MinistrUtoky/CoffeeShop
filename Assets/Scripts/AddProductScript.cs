using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using static Assets.Scripts.Database.DataStructures;
using Debug = UnityEngine.Debug;
using TextAsset = UnityEngine.TextAsset;

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

        private List<Product> products;
        private void Start()
        {
            UpdateProductsList();
        }
        public void AddProduct()
        {
            float numPrice=-1;
            float.TryParse(productPrice.text, out numPrice);
            Product product = new Product
            {
                id = products.Count,
                name = productName.text,
                description = productDescription.text,
                price = numPrice,
                //Временное решение пока не поменял меню добавления товара
                pictureURL = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e4/Latte_and_dark_coffee.jpg/1920px-Latte_and_dark_coffee.jpg",
            };
            SubscriptionManagerScript.Instance.AddNewProduct(product);
        }
        public void UpdateProductsList()
        {
            products = SubscriptionManagerScript.Instance.GetProductList();
        }
    }
}