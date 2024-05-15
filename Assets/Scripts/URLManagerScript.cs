using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class URLManagerScript : MonoBehaviour
{
    private Texture2D texture;
    public static URLManagerScript Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        texture = null;
    }
    public IEnumerator GetTexture(Sprite sprite,string URL)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URL);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            texture = null;
        }
    }
}
