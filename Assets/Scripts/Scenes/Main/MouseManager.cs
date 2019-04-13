using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: December 23th 2018
 */
public class MouseManager : MonoBehaviour
{
    public Texture2D normalCursor;
    public Texture2D clickCursor;

    private void Start()
    {
        Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        if (InputManager.LEFT_MOUSE_DOWN)
        {
            Cursor.SetCursor(clickCursor, Vector2.zero, CursorMode.Auto);
        }
        else if (InputManager.LEFT_MOUSE_UP || InputManager.RIGHT_MOUSE_UP)
        {
            Cursor.visible = true;
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
