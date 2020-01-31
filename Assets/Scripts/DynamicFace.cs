using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicFace : MonoBehaviour
{
    [SerializeField]
    Sprite[] sprites;

    public int CurrentSpriteIndex
    {
        get
        {
            return System.Array.IndexOf(sprites, this.GetComponent<SpriteRenderer>().sprite);
        }
        set
        {
            this.GetComponent<SpriteRenderer>().sprite = sprites[value];
        }
    }

    public int SpritesCount
    {
        get
        {
            return this.sprites.Length;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
