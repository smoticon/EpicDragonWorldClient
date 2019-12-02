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

            // World target related.
            if (!MainManager.Instance.lastLoadedScene.Equals(MainManager.WORLD_SCENE))
            {
                return;
            }
            // Raycast new target.
            if (Physics.Raycast(CameraController.Instance.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
            {
                Transform transform = raycastHit.transform;
                if (transform != null)
                {
                    WorldObject worldObject = transform.gameObject.GetComponent<WorldObject>();
                    if (worldObject != null && worldObject.characterData != null && worldObject.characterData.IsTargetable())
                    {
                        // Interact.
                        if (WorldManager.Instance.targetWorldObject == worldObject)
                        {
                            // TODO: Interact.
                        }
                        else // Select new target.
                        {
                            WorldManager.Instance.SetTarget(worldObject);
                        }
                    }
                }
            }
        }
        else if (InputManager.LEFT_MOUSE_UP || InputManager.RIGHT_MOUSE_UP)
        {
            Cursor.visible = true;
            Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
