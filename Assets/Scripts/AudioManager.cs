using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _clickSound;
    [SerializeField] AudioClip _puttingDownSound;
    [SerializeField] AudioClip _shipGoingSound;
    [SerializeField] AudioClip _winSound;
    [SerializeField] AudioClip _loseSound;

    [SerializeField] float _volume = 1f;

    public void PlayClickSound()
    {
        _audioSource.PlayOneShot(_clickSound, _volume);
    }

    public void PlayPuttingSound()
    {
        _audioSource.PlayOneShot(_puttingDownSound, _volume);
    }

    public void PlayShipSound()
    {
        _audioSource.PlayOneShot(_shipGoingSound, _volume);
    }

    public void PlayWinSound()
    {
        _audioSource.PlayOneShot(_winSound, _volume);
    }

    public void PlayLoseSound()
    {
        _audioSource.PlayOneShot(_loseSound, _volume);
    }
}
