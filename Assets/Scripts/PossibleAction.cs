using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PossibleAction : MonoBehaviour
{
    [SerializeField]
    Task task;
    SpriteRenderer mySprite;

    Texture2D plusCursor;

    // Start is called before the first frame update
    void Start()
    {
        plusCursor = Resources.Load<Texture2D>("Sprites/plus");
        mySprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(plusCursor, new Vector2(17, 17), CursorMode.ForceSoftware);
        mySprite.color = Color.green;
    }

    void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
        mySprite.color = Color.white;
    }

    void OnMouseDown()
    {
        Vector3 w = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        w = new Vector3(w.x, w.y, 0);

        Debug.Log("click");
        BlockObj bo = Instantiate(MainObject.Get().blockObjPrefab,
                           Vector3.zero,
                           Quaternion.identity).GetComponent<BlockObj>();
        bo.transform.position = w;
        bo.IsDragged = true;
    }

}
