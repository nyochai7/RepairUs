using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInFadeOut : MonoBehaviour
{
    [SerializeField]
    private float fadeSpeed = 0.05f;

    public float CurrentAlpha
    {
        get
        {
            return spriteRenderer.color.a;
        }
        set
        {
            Color c = spriteRenderer.color;
            c.a = value;
            spriteRenderer.color = c;
        }
    }

    private float[] Values =
    {
        1,
        0
    };

    private int currentValueIndex = 0;

    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CurrentAlpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float targetValue = Values[currentValueIndex];

        if (Mathf.Abs(targetValue-CurrentAlpha) < fadeSpeed)
        {
            CurrentAlpha = targetValue;
            currentValueIndex++;
            if (currentValueIndex == Values.Length)
            {
                Destroy(this.gameObject);
            }
        }

        if (CurrentAlpha < targetValue)
        {
            CurrentAlpha += fadeSpeed * Time.deltaTime;
        } else
        {
            CurrentAlpha -= fadeSpeed * Time.deltaTime;
        }

        Debug.Log("currentValueIndex="+ currentValueIndex.ToString());
        Debug.Log("CurrentAlpha="+ CurrentAlpha.ToString());
        Debug.Log("targetValue="+ targetValue.ToString());
    }

}
