using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: February 25th 2018
 */
public class DontDestroyObject : MonoBehaviour
{
	private void Update ()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
