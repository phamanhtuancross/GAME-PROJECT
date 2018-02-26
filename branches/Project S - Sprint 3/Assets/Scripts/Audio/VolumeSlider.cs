using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public enum VolumeSliderType
    {
        MUSIC,
        SFX
    }

    public VolumeSliderType type;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        if (type == VolumeSliderType.MUSIC)
        {
            slider.value = UserData.GetMusicVolume();
        }
        else if (type == VolumeSliderType.SFX)
        {
            slider.value = UserData.GetSFXVolume();
        }
    }

    public void OnValueChanged(float volumeLevel)
    {
        if (type == VolumeSliderType.MUSIC)
            AudioMixerControl.Instance.setMusicLvl(volumeLevel);
        else if (type == VolumeSliderType.SFX)
            AudioMixerControl.Instance.setSFXLvl(volumeLevel);
    }

    public void UpdateValue()
    {
        if(slider == null)
            slider = GetComponent<Slider>();
        if (type == VolumeSliderType.MUSIC)
            slider.value = UserData.GetMusicVolume();
        else if (type == VolumeSliderType.SFX)
            slider.value = UserData.GetSFXVolume();
    }
}
