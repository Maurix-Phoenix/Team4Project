//AudioManager.cs 
//by: MAURIZIO FISCHETTI

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static T4P;

/// <summary>
/// AudioManager manage and plays the audioclips of the game, the Audioclips MUST be inside their list type
/// </summary>
public class AudioManager : MonoBehaviour
{
    //TODO settings
    //music volume,
    //sfx volume,

    public AudioSource AudioSourceMusic { get; private set; }
    public AudioSource AudioSourceSFX { get; private set; }

    [Header("Audio Libraries")]
    public List<Music> Musics= new List<Music>();
    public List<SFX> SFXs = new List<SFX>();

    public Slider SliderMusic;
    public Slider SliderSFX;

    [SerializeField]private float _SliderAudioIntervail = 1;
    private float _SliderAudioT;

    private void Awake()
    {
        Initialize();
    }

    private bool Initialize()
    {
        AudioSourceMusic = transform.GetChild(0).GetComponent<AudioSource>();
        AudioSourceSFX = transform.GetChild(1).GetComponent<AudioSource>();

        AudioSourceMusic.volume = GameManager.Instance.DataManager.GameData.MusicVolume;
        AudioSourceSFX.volume = GameManager.Instance.DataManager.GameData.SFXVolume;

        if (AudioSourceMusic != null && AudioSourceSFX != null)
        {
            T4Debug.Log("[AudioManager] Initialized.");
            return true;
        }
        else 
        {
            T4Debug.Log(gameObject.name + "Failed Initialization, one or more AudioSource is null");
            return false;
        }
    }

    /// <summary>
    /// Play the music clip from the Musics List using the Audio Source of the Manager
    /// </summary>
    /// <param name="clipName"></param>
    public void PlayMusic(string clipName)
    {
        foreach (Music music in Musics)
        {
            if (music != null)
            {
                if (music.Name == clipName)
                {
                    AudioSourceMusic.clip = music.clip;
                    AudioSourceMusic.Play();
                    return;
                }
            }
            else T4Debug.Log($"[Audio Manager] AudioClip invalid.", T4P.T4Debug.LogType.Warning);
        }
        T4Debug.Log($"[Audio Manager] can't find clip named '{clipName}' in the Music list.", T4P.T4Debug.LogType.Warning);
    }
    public void PlayMusic(AudioClip ac)
    {//global music
        foreach (Music music in Musics)
        {
            if (music != null)
            {
                if (music.clip == ac)
                {
                    AudioSourceMusic.clip = ac;
                    AudioSourceMusic.Play();
                    return;
                }
            }
            else T4Debug.Log($"[Audio Manager] AudioClip invalid.", T4P.T4Debug.LogType.Warning);
        }
        T4Debug.Log($"[Audio Manager] can't find clip in the Musics list.", T4P.T4Debug.LogType.Warning);
    }

    /// <summary>
    /// Playe the sfx clip from the sfxs list using the Audio Source of the Manager
    /// </summary>
    /// <param name="clipName"></param>
    public void PlaySFX(string clipName)
    {
        foreach (SFX sfx in SFXs)
        {
            if (sfx != null)
            {
                if (sfx.Name == clipName)
                {
                    AudioSourceSFX.PlayOneShot(sfx.clip);
                    return;
                }
            }
            else T4Debug.Log($"[Audio Manager] AudioClip invalid.", T4P.T4Debug.LogType.Warning);
        }
        T4Debug.Log($"[Audio Manager] can't find clip named '{clipName}' in the Music list.", T4P.T4Debug.LogType.Warning);
    }
    public void PlaySFX(AudioClip ac)
    {//global sound effect
        foreach(SFX sfx in SFXs)
        {
            if (sfx != null)
            {
                if(sfx.clip == ac)
                {
                    AudioSourceSFX.PlayOneShot(ac);
                    return;
                }
            }

            else T4Debug.Log($"[Audio Manager] AudioClip invalid.", T4P.T4Debug.LogType.Warning);
        }
        T4Debug.Log($"[Audio Manager] can't find clip in the SFXs list.", T4P.T4Debug.LogType.Warning);
    }


    /// <summary>
    /// Play the music in the musics lists with the given audio source
    /// </summary>
    /// <param name="audioS"></param>
    /// <param name="clipName"></param>
    public void PlayMusicLocal(AudioSource audioS, string clipName)
    {//local music
        if (audioS != null && clipName != null)
        {
            foreach (Music music in Musics)
            {
                if (music != null)
                {
                    if (music.Name == clipName)
                    {
                        audioS.clip = music.clip;
                        audioS.Play();
                        return;
                    }
                }
            }
            T4Debug.Log($"[Audio Manager] can't find clip named '{clipName}' in the Music list.", T4P.T4Debug.LogType.Warning);
        }
        else T4Debug.Log($"[Audio Manager] Audiosource or clip invalid in {audioS.gameObject.name}.", T4P.T4Debug.LogType.Warning);
    }
    public void PlayMusicLocal(AudioSource audioS, AudioClip ac)
    {//local music
        if (audioS != null && ac != null)
        {
            foreach (Music music in Musics)
            {
                if (music != null)
                {
                    if (music.clip == ac)
                    {
                        audioS.clip = ac;
                        audioS.Play();
                        return;
                    }
                }
            }
            T4Debug.Log($"[Audio Manager] can't find clip {ac.name} in the Music list.", T4P.T4Debug.LogType.Warning);
        }
        else T4Debug.Log($"[Audio Manager] Audiosource or clip invalid in {audioS.gameObject.name}.", T4P.T4Debug.LogType.Warning);
    }

    /// <summary>
    /// Play the sfx in the sfxs lists with the given audio source
    /// </summary>
    /// <param name="audioS"></param>
    /// <param name="clipName"></param>
    public void PlaySFXLocal(AudioSource audioS, string clipName)
        {//local music
        if (audioS != null && clipName != null)
        {
            foreach (SFX sfx in SFXs)
            {
                if (sfx != null)
                {
                    if (sfx.Name == clipName)
                    {
                        audioS.PlayOneShot(sfx.clip);
                        return;
                    }
                }
            }
            T4Debug.Log($"[Audio Manager] can't find clip named '{clipName}' in the SFXs list.", T4P.T4Debug.LogType.Warning);
        }
        else T4Debug.Log($"[Audio Manager] Audiosource or clip invalid in {audioS.gameObject.name}.", T4P.T4Debug.LogType.Warning);
    }
    public void PlaySFXLocal(AudioSource audioS, AudioClip ac)
    {//local sound effect
        if (audioS != null && ac != null)
        {
            foreach (SFX sfx in SFXs)
            {
                if (sfx != null)
                {
                    if (sfx.clip == ac)
                    {
                        audioS.PlayOneShot(ac);
                        return;
                    }
                }
            }
            T4Debug.Log($"[Audio Manager] can't find clip {ac.name} in the SFXs list.", T4P.T4Debug.LogType.Warning);
        }
        else T4Debug.Log($"[Audio Manager] Audiosource or clip invalid in {audioS.gameObject.name}.", T4P.T4Debug.LogType.Warning);
    }


    /// <summary>
    /// Music class to associate music clips with a name, mainly used in the editor
    /// </summary>
    [Serializable]
    public class Music
    {
        public string Name;
        public AudioClip clip;
    }

    /// <summary>
    /// SFX class to associate sfx clips with a name, mainly used in the editor
    /// </summary>
    [Serializable]
    public class SFX
    {
        public string Name;
        public AudioClip clip;
    }



    public void SetSliderMusicVolume() 
    { 
       AudioSourceMusic.volume = SliderMusic.value;
       GameManager.Instance.DataManager.GameData.MusicVolume = AudioSourceMusic.volume;
        T4Debug.Log($"[Audio Manager] music volume: {AudioSourceMusic.volume}");
    }
    public void SetSliderSFXSVolume()
    {
        if(_SliderAudioT <= 0)
        {
            PlaySFX("GAME_CoinObtained");
            _SliderAudioT = _SliderAudioIntervail;
        }
        AudioSourceSFX.volume = SliderSFX.value;
        GameManager.Instance.DataManager.GameData.SFXVolume = AudioSourceSFX.volume;
        T4Debug.Log($"[Audio Manager] sfx volume: {AudioSourceSFX.volume}");


    }

    private void Update()
    {
        if(_SliderAudioT > 0)
        {
            _SliderAudioT -= Time.unscaledDeltaTime;
            if( _SliderAudioT <= 0 )
            {
                _SliderAudioT = 0;
            }
        }
    }
}
