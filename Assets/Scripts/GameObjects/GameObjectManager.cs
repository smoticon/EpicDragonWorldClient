using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class GameObjectManager : MonoBehaviour
{
    // GameObject manager instance.
    public static GameObjectManager instance;
    // Player models are saved here.
    public GameObject[] playerModels;
    // Game object models are saved here.
    public GameObject[] gameObjectList;

    void Start()
    {
        instance = this;
    }

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
