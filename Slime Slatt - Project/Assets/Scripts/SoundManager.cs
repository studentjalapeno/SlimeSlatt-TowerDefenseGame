using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private Slider sfxSlider;

    [SerializeField]
    private Slider musicSlider;

    Dictionary<string , AudioClip> audioClips = new Dictionary<string, AudioClip>();


    // Start is called before the first frame update
    void Start()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[]; //loads mp3 files into clips

        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);

        }

        LoadVolume();

        musicSlider.onValueChanged.AddListener(delegate { UpdateVolume(); }); //listener (music slider)
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });


    }


    public void PlaySFX(string name)
    {
        sfxSource.PlayOneShot(audioClips[name]);
    }

    public void UpdateVolume()
    {

        //music volume is = to music slider value
        musicSource.volume = musicSlider.value;

        //sfx volume is = to sfx slider value
        sfxSource.volume = sfxSlider.value;

        //says sliders values even when player quits game
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        PlayerPrefs.SetFloat("Music", musicSlider.value);

    }

    public void LoadVolume()
    {
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.5f); //default value

        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.5f); //default value

        musicSlider.value = musicSource.volume;

        sfxSlider.value = sfxSource.volume;
    }
}
