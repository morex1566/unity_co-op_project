using UnityEngine;

public class SaberCotroller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Fragile Obstacle"))
        {
            Destroy(other.gameObject);
        }
    }
}