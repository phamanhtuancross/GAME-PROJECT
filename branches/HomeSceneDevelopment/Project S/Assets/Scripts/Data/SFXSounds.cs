using UnityEngine;
using System.Collections;

public class SFXSounds : MonoBehaviour
{
    public AudioClip clipPlaceBlocks;
    public AudioClip clipClearBlocks;
    public AudioClip clipCompleteGame;
    public AudioClip clipFailedGame;
    public AudioClip clipButtonClick;
    public AudioSource backgroundMusic;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playPlaceBlockSFX()
    {
        audioSource.PlayOneShot(clipPlaceBlocks);
    }

    public void playClearBlockSFX()
    {
        audioSource.PlayOneShot(clipClearBlocks);
    }

    public void playCompleteGameSFX()
    {
        backgroundMusic.Stop();
        backgroundMusic.PlayDelayed(clipCompleteGame.length);
        audioSource.PlayOneShot(clipCompleteGame);
    }

    public void playFailedGameSFX()
    {
        backgroundMusic.Stop();
        backgroundMusic.PlayDelayed(clipFailedGame.length);
        audioSource.PlayOneShot(clipFailedGame);
    }

    public void playButtonClickSFX()
    {
        audioSource.PlayOneShot(clipButtonClick);
    }
}
