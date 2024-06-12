using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadExternalImages : MonoBehaviour
{
    private string _externalImagesFolder = "ExternalImages";
    private int _spritesQuantity = 0;
    private MainMenu _mainMenuScript;

    private List<GameObject> _imageContainers = new List<GameObject>();
    private List<Sprite> _sprites = new List<Sprite>();

    [SerializeField] private GameObject _canvasParent;
    [SerializeField] private GameObject _button;

    void Start()
    {
        _mainMenuScript = GameObject.FindObjectOfType<MainMenu>();
        LoadImages();
    }

    void LoadImages()
    {
        string path = Path.Combine(Application.dataPath, _externalImagesFolder);

        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path, "*.jpg");

            foreach (string file in files)
            {
                
                StartCoroutine(LoadImageAsSprite(file));
            }
        }
        else
        {
            Debug.LogWarning("External images folder does not exist: " + path);
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

                if (_spritesQuantity > 10)
                    yield break;

                // Stocke le sprite dans une liste
                _sprites.Add(sprite);

                // Instantie un nouveau boutton
                _imageContainers.Add(Instantiate(_button, _canvasParent.transform));
                _imageContainers[_spritesQuantity].GetComponent<Button>().onClick.AddListener(() => _mainMenuScript.LoadMain(filePath));

                // Applique le sprite chargé au nouveau boutton
                ApplySprite(_sprites[_spritesQuantity], _imageContainers[_spritesQuantity]);

                // Incrémente l'index
                _spritesQuantity++;
            }
            else
            {
                Debug.LogError("Error loading image: " + www.error);
            }
        }
    }

    void ApplySprite(Sprite sprite, GameObject imageContainer)
    {
        if (imageContainer != null)
        {
            var spriteRenderer = imageContainer.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprite;
            }
            else
            {
                var imageComponent = imageContainer.GetComponent<UnityEngine.UI.Image>();
                if (imageComponent != null)
                {
                    imageComponent.sprite = sprite;
                }
                else
                {
                    Debug.LogWarning("No SpriteRenderer or Image component found on the imageContainer.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Image container is not assigned.");
        }
    }
}
