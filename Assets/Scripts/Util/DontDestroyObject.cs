using UnityEngine;

/**
 * @author Pantelis Andrianakis
 */
public class DontDestroyObject : MonoBehaviour
{
	private void Update ()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
