using System.Collections;
using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class WorldManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject playerCharacter;
    public GameObject playerMale;
    public GameObject playerFemale;
    public GameObject cameraMale;
    public GameObject cameraFemale;

    [HideInInspector]
    public static WorldManager instance;
    [HideInInspector]
    ArrayList gameObjects = new ArrayList();
    [HideInInspector]
    private static readonly int visibilityRange = 3000;

    // Additional code to check out errors.
    bool newCharacState = false;
    bool movement = false;
    int animState = 0;
    int waterState = 0;
    Vector3 characPos = new Vector3(0, 0, 0);
    Vector3 movementPos = new Vector3(0,0,0);
    long gmObjClassId = 0;
    long moveObjectId = 0;
    long updatingObjectId = 0;
    float angleY = 0f;

    private void Start()
    {
        if (NetworkManager.instance == null) // Offline mode.
        {
            // Set player model to male.
            playerMale.SetActive(true);
            cameraMale.SetActive(true);
            playerCharacter = playerMale;

            // Set position.
            playerCharacter.transform.position = new Vector3(9945.9f, 9.2f, 10534.9f); // Spawn location.
        }
        else // Online mode.
        {
            // Return if account name is empty.
            if (PlayerManager.instance == null || PlayerManager.instance.accountName == null)
            {
                return; // Return to login?
            }

            // Set instance.
            instance = this;

            // Change music.
            MusicManager.instance.PlayMusic(MusicManager.instance.EnterWorld);

            // Set player model.
            if (PlayerManager.instance.selectedCharacterData.GetClassId() == 0) // Male.
            {
                playerMale.SetActive(true);
                cameraMale.SetActive(true);
                playerCharacter = playerMale;
            }
            if (PlayerManager.instance.selectedCharacterData.GetClassId() == 1) // Female.
            {
                playerFemale.SetActive(true);
                cameraFemale.SetActive(true);
                playerCharacter = playerFemale;
            }

            // Set position.
            playerCharacter.transform.position = new Vector3(PlayerManager.instance.selectedCharacterData.GetX(), PlayerManager.instance.selectedCharacterData.GetY(), PlayerManager.instance.selectedCharacterData.GetZ());

            // Request world info from server.
            NetworkManager.instance.ChannelSend(new EnterWorldRequest(PlayerManager.instance.selectedCharacterData.GetName()));

            // Object distance forget task.
            StartCoroutine(DistanceCheck());
        }
    }

    public void FixedUpdate()
    {
        if (newCharacState)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GetComponent<WorldObject>().objectId == updatingObjectId)
                {
                    newCharacState = false;
                    // TODO: Update object info.
                    return;
                }
            }
            // Object does not exist. Instantiate.
            GameObject obj = Instantiate(GameObjectManager.instance.gameObjectList[gmObjClassId], characPos, Quaternion.identity) as GameObject;

            // TODO: Proper appearance.

            // Assign object id.
            obj.AddComponent<WorldObject>();
            obj.GetComponent<WorldObject>().objectId = updatingObjectId;
            obj.GetComponent<WorldObject>().targetPos = obj.transform.position;
            // Add RigidBody.
            Rigidbody rigidBody = obj.AddComponent<Rigidbody>();
            rigidBody.mass = 1;
            rigidBody.angularDrag = 0.05f;
            rigidBody.freezeRotation = true;

            // Add to game object list.
            gameObjects.Add(obj);
            newCharacState = false;
        }
        if (movement)
        {
            movement = false;
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.GetComponent<WorldObject>().objectId == moveObjectId)
                {
                    Vector3 tmpPos = new Vector3();
                    tmpPos.x = movementPos.x;
                    tmpPos.z = movementPos.z;
                    tmpPos.y = gameObject.transform.position.y;
                    gameObject.GetComponent<WorldObject>().PlayAnimation(movementPos, angleY, animState, waterState);
                }
            }
        }
    }

    public void UpdateObject(long objectId, int classId, float posX, float posY, float posZ, int posHeading)
    {
        // Check for existing objects.
        characPos.x = posX;
        characPos.y = posY;
        characPos.z = posZ;
        updatingObjectId = objectId;
        gmObjClassId = classId;
        newCharacState = true;
    }

    public void MoveObject(long objectId, float posX, float posY, float posZ, float angY, int animId, int wState)
    {
        waterState = wState;
        movementPos.x = posX;
        movementPos.y = posY;
        movementPos.z = posZ;
        angleY = angY;
        moveObjectId = objectId;
        animState = animId;
        movement = true;
        // Request unknown object info from server.
        NetworkManager.instance.ChannelSend(new ObjectInfoRequest(objectId));
    }

    public void DeleteObject(long objectId)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.GetComponent<WorldObject>().objectId == objectId)
            {
                gameObjects.Remove(gameObject);
                DeleteObject(gameObject);
                return;
            }
        }
    }

    private void DeleteObject(GameObject gameObject)
    {
        // Remove from objects list.
        gameObjects.Remove(gameObject);

        // Delete game object from world.
        Destroy(gameObject);
    }

    IEnumerator DistanceCheck()
    {
        while (true)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (Vector3.Distance(playerCharacter.transform.position, gameObject.transform.position) > visibilityRange)
                {
                    DeleteObject(gameObject);
                }
            }
            yield return new WaitForSeconds(3);
        }
    }
}
