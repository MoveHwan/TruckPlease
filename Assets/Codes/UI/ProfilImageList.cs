using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilImageList : MonoBehaviour
{
    public static ProfilImageList Instance;

    public Sprite PlayerImage;

    public List<Texture> Textures;
    public List<Sprite> Sprites;


    void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            PlayerImage = GetSprite(PlayerPrefs.GetString("ProfileImage", "Human_1"));
        }
    }

    public Texture GetTexture(string name) => Textures.Find(x => x.name == name);
    public Sprite GetSprite(string name) => Sprites.Find(x => x.name == name);

}
