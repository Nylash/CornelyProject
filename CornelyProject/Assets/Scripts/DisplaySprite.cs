using UnityEngine;
using UnityEngine.Networking;

public class DisplaySprite : MonoBehaviour
{
    private void Awake()
    {
        if(PlayerPrefs.GetInt("UseSprite") == 1)
        {
            StartCoroutine(LoadImageAsSprite(PlayerPrefs.GetString("SpritePath")));
        }
    }

    System.Collections.IEnumerator LoadImageAsSprite(string filePath)
    {
        string uri = "file://" + filePath;

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(uri))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                // Créer un Sprite à partir de la Texture2D
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // Applique le sprite chargé à l'image de fond
                GameObject.FindObjectOfType<SpriteRenderer>().sprite = sprite;
            }
            else
            {
                Debug.LogError("Error loading image: " + www.error);
            }
        }
    }
}
