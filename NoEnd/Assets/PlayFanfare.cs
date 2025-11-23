using UnityEngine;

public class PlayFanfare : MonoBehaviour
{
    [SerializeField] AudioSource _myAudio;
    public void PlayAudio()
    {
        _myAudio.Play();
    }
}
