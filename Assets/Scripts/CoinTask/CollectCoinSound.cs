using Unity.VisualScripting;
using UnityEngine;

public class CollectCoinSound : MonoBehaviour
{
    [SerializeField] AudioClip coinSound;

    private void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(coinSound, transform.position);
        Destroy(gameObject);
    }
}
