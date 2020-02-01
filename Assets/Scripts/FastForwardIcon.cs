using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastForwardIcon : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    public bool Visible
    {
        get
        {
            return spriteRenderer.color.a == 1f;
        }
        set
        {
            Color c = spriteRenderer.color;
            c.a = value ? 1f: 0f;
            spriteRenderer.color = c;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        this.Visible = Utils.IsFastForward();
    }
}
