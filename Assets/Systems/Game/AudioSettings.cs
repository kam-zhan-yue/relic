using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "ScriptableObjects/AudioSettings")]
public class AudioSettings : ScriptableObject
{
    public AudioMixer bgmMixer;
    public AudioMixer sfxMixer;

    private float _bgm = 1f;
    private float _sfx = 1f;

    public float Bgm
    {
        get => _bgm;
        private set
        {
            _bgm = value;
            bgmMixer.SetFloat("Master", ToDecibels(_bgm));
        }
    }

    public float Sfx
    {
        get => _sfx;
        private set
        {
            _sfx = value;
            sfxMixer.SetFloat("Master", ToDecibels(_sfx));
        }
    }

    public void Init()
    {
        Bgm = PlayerPrefs.HasKey("BGM") ? PlayerPrefs.GetFloat("BGM") : 1f;
        Sfx = PlayerPrefs.HasKey("SFX") ? PlayerPrefs.GetFloat("SFX") : 1f;
    }
    
    public void OnBgmChanged(float value)
    {
        PlayerPrefs.SetFloat("BGM", value);
        PlayerPrefs.Save();
        Bgm = value;
    }

    public void OnSfxChanged(float value)
    {
        PlayerPrefs.SetFloat("SFX", value);
        PlayerPrefs.Save();
        Sfx = value;
    }
    
    private float ToDecibels(float value)
    {
        return Mathf.Log10(value) * 20;
    }
}