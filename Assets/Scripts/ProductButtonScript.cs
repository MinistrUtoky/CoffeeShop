using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UserManagement;
using static Assets.Scripts.Database.DataStructures;

public class ProductButtonScript : MonoBehaviour
{
    private Product _product;
    [SerializeField] private TextMeshProUGUI productNameText;
    [SerializeField] private TextMeshProUGUI productPriceText;
    [SerializeField] private Image productImage;
    private Texture2D productIcon;

    public void SetValues(Product product)
    {
        try
        {
            _product = product;
            productNameText.text = product.name;
            string currency = UserManagerScript.Instance.GetCurrentUser().currency;
            productPriceText.text = $"Price:\n{product.price* SubscriptionManagerScript.Instance.GetConversionRate("Euro", currency)} {currency}s";
            // бпелеммне пеьемхе, оепедекюрэ йнцдю асдер цнрнб лемедфеп спк
            if (gameObject.activeInHierarchy)
                StartCoroutine(GetTexture(product.pictureURL));
        }
        catch
        {
            Debug.LogError("вРН РН ОНЬКН МЕ РЮЙ Б ГЮДЮВЕ ГМЮВЕМХИ ДКЪ БХДФЕРНБ ЦКЮБМНИ ЯРПЮМХЖШ");
            Destroy(gameObject);
        }
    }
    public Product GetProduct()
    {
        return _product;
    }
    // бпелеммне пеьемхе, оепедекюрэ йнцдю асдер цнрнб лемедфеп спк
    private IEnumerator GetTexture(string URL)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            productIcon = ((DownloadHandlerTexture)www.downloadHandler).texture;
            productImage.sprite = Sprite.Create(productIcon, new Rect(0, 0, productIcon.width, productIcon.height), Vector2.zero); ;
        }
    }
}
