using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Text : MonoBehaviour
{
    // Start is called before the first frame update

    Color color = new Color(0.1f, 0.7f, 0.2f);
    public bool isStart;
    public bool isQuit;
    void Start()
    {
        gameObject.GetComponent<TextMeshPro>().color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        gameObject.GetComponent<TextMeshPro>().color = color;
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<TextMeshPro>().color = Color.white;
    }

    void OnMouseUp()
    {
        if (isStart)
        {
            Application.LoadLevel(1);
        }
        if (isQuit)
        {
            Application.Quit();
        }
    }

}
