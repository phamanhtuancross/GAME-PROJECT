using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioMixerControl : Singleton<AudioMixerControl>
{
    public AudioMixer masterMixer;
    
    public void setMusicLvl(float musicLvl)
    {
        masterMixer.SetFloat("MusicVol", musicLvl);
        UserData.SetMusicVolume(musicLvl);
    }

    public void setSFXLvl(float SFXLvl)
    {
        masterMixer.SetFloat("SFXVol", SFXLvl);
        UserData.SetSFXVolume(SFXLvl);
    }
}
