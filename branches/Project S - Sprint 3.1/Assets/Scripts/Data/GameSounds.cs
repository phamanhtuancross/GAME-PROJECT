using UnityEngine;
using System.Collections;

public class GameSounds : MonoBehaviour
{
    public AudioClip clipPlaceBlocks;
    public AudioClip clipClearBlocks;
    public AudioClip clipCompleteGame;
    public AudioClip clipFailedGame;
    public AudioClip clipButtonClick;
    public AudioClip clipHomeBGMusic;
    public AudioClip clipPlayBGMusic;
    public AudioSource backgroundMusic;
    public AudioSource sfxSounds;

    public void playPlaceBlockSFX()
    {
        sfxSounds.PlayOneShot(clipPlaceBlocks);
    }

    public void playClearBlockSFX()
    {
        sfxSounds.PlayOneShot(clipClearBlocks);
    }

    public void playCompleteGameSFX()
    {
        backgroundMusic.Stop();
        backgroundMusic.PlayDelayed(clipCompleteGame.length);
        sfxSounds.PlayOneShot(clipCompleteGame);
    }

    public void playFailedGameSFX()
    {
        backgroundMusic.Stop();
        backgroundMusic.PlayDelayed(clipFailedGame.length);
        sfxSounds.PlayOneShot(clipFailedGame);
    }

    public void playButtonClickSFX()
    {
        sfxSounds.PlayOneShot(clipButtonClick);
    }

    public void playHomeBGMusic()
    {
        backgroundMusic.clip = clipHomeBGMusic;
        backgroundMusic.Play();
    }

    public void playPlayBGMusic()
    {
        backgroundMusic.clip = clipPlayBGMusic;
        backgroundMusic.Play();
    }
}
