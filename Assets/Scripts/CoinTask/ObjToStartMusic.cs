using UnityEngine;

public class ObjToStartMusic : MonoBehaviour
{
    [SerializeField] AudioSource mainAudio;

    private void OnTriggerEnter(Collider other)
    {
        mainAudio.Play();
    }
}
