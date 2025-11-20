using UnityEngine;

public class ObjToStopMusic : MonoBehaviour
{
    [SerializeField] AudioSource mainAudio;

    private void OnTriggerEnter(Collider other)
    {
        mainAudio.Stop();
    }
}
