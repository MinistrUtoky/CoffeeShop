using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using TextAsset = UnityEngine.TextAsset;

namespace productRelated
{
    [Serializable]
    public class SubList<Subscription>
    {
        public List<Subscription> subscriptionList;
    }
    [Serializable]
    public struct Product
    {
        public int id;
        public string name;
        [TextArea(10, 10)]
        public string description;
        public float price;
    }
    [Serializable]
    public class MyList<Product>
    {
        public List<Product> productList;
    }
    public class AddProductScript : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField productId;
        [SerializeField]
        private TMP_InputField productName;
        [SerializeField]
        private TMP_InputField productDescription;
        [SerializeField]
        private TMP_InputField productPrice;

        [SerializeField]
        public TextAsset jsonFile;
        [SerializeField]
        private MyList<Product> products;
        private void Awake()
        {
            products = JsonUtility.FromJson<MyList<Product>> (jsonFile.text);
            if (products == null) products = new MyList<Product>();
        }
        public void AddProduct()
        {
            Product product = new Product
            {
                id=products.productList.Count,
                name = productName.text,
                description = productDescription.text,
            };
            float.TryParse(productPrice.text, out product.price);
            products.productList.Add(product);
            SaveProductIntoJson();
        }
        private void SaveProductIntoJson()
        {
            string jsonNew = JsonUtility.ToJson(products);
            Debug.Log("Added to products JSON " + jsonNew);
            File.WriteAllText(AssetDatabase.GetAssetPath(jsonFile), jsonNew);
            EditorUtility.SetDirty(jsonFile);
        }
    }
}