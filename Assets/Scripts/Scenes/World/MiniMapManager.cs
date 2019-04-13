using UnityEngine;

/**
 * Author: Ilias Vlachos, Pantelis Andrianakis
 * Date: January 8th 2019
 */
public class MiniMapManager : MonoBehaviour
{
    private static readonly float cameraHeight = 100f;
    private static readonly float fieldOfView = 150f;

    private void LateUpdate()
    {
        // Check if tag Player exists and return untill is defined.
        if (WorldManager.Instance.activeCharacter == null)
        {
            return;
        }

        // Minimap follow player.
        Vector3 newPosition = WorldManager.Instance.activeCharacter.transform.position;
        newPosition.y += cameraHeight;
        transform.position = newPosition;

        // Minimap rotate with player.
        transform.rotation = Quaternion.Euler(90f, WorldManager.Instance.activeCharacter.transform.eulerAngles.y, 0f);

        // Field of view.
        Camera camera = gameObject.GetComponent<Camera>();
        camera.fieldOfView = fieldOfView;
    }
}
