using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer; // drag your mixer here
    [SerializeField] private float master = 1f;
    [SerializeField] private float music = 1f;
    [SerializeField] private float sfx = 1f;
    [SerializeField] private bool mute = false;

    // Draft values (preview)
    private float draftMaster, draftMusic, draftSfx;
    private bool draftMute;

    private const string KEY_MASTER = "settings_master";
    private const string KEY_MUSIC = "settings_music";
    private const string KEY_SFX = "settings_sfx";
    private const string KEY_MUTE = "settings_mute";

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
        SyncDraftFromSaved();
        ApplyDraftAudio();   // applies current settings (same as saved at this moment)
    }

    // --- getters ---
    public float Master => master;
    public float Music => music;
    public float Sfx => sfx;
    public bool Mute => mute;

    // --- setters ---
    public void SetMaster(float v) { master = Mathf.Clamp01(v); ApplyAudio(); }
    public void SetMusic(float v) { music = Mathf.Clamp01(v); ApplyAudio(); }
    public void SetSfx(float v) { sfx = Mathf.Clamp01(v); ApplyAudio(); }
    public void SetMute(bool on) { mute = on; ApplyAudio(); }

    public float DraftMaster => draftMaster;
    public float DraftMusic => draftMusic;
    public float DraftSfx => draftSfx;
    public bool DraftMute => draftMute;

    public void SetDraftMaster(float v) { draftMaster = Mathf.Clamp01(v); ApplyDraftAudio(); }
    public void SetDraftMusic(float v) { draftMusic = Mathf.Clamp01(v); ApplyDraftAudio(); }
    public void SetDraftSfx(float v) { draftSfx = Mathf.Clamp01(v); ApplyDraftAudio(); }
    public void SetDraftMute(bool on) { draftMute = on; ApplyDraftAudio(); }

    public void Save()
    {
        PlayerPrefs.SetFloat(KEY_MASTER, master);
        PlayerPrefs.SetFloat(KEY_MUSIC, music);
        PlayerPrefs.SetFloat(KEY_SFX, sfx);
        PlayerPrefs.SetInt(KEY_MUTE, mute ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        master = PlayerPrefs.GetFloat(KEY_MASTER, 1f);
        music = PlayerPrefs.GetFloat(KEY_MUSIC, 1f);
        sfx = PlayerPrefs.GetFloat(KEY_SFX, 1f);
        mute = PlayerPrefs.GetInt(KEY_MUTE, 0) == 1;
    }

    private void ApplyAudio()
    {
        if (audioMixer == null) return;

        float m = mute ? 0.0001f : master;
        SetMixerVolume("MasterVol", m);
        SetMixerVolume("MusicVol", mute ? 0.0001f : music);
        SetMixerVolume("SfxVol", mute ? 0.0001f : sfx);
    }

    private void SetMixerVolume(string param, float linear)
    {
        // linear 0..1 to decibels
        float db = Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
        audioMixer.SetFloat(param, db);
    }

    public void SyncDraftFromSaved()
    {
        draftMaster = master;
        draftMusic = music;
        draftSfx = sfx;
        draftMute = mute;
    }

    public void ApplyDraftAudio()
    {
        if (audioMixer == null) return;

        float m = draftMute ? 0.0001f : draftMaster;
        SetMixerVolume("MasterVol", m);
        SetMixerVolume("MusicVol", draftMute ? 0.0001f : draftMusic);
        SetMixerVolume("SfxVol", draftMute ? 0.0001f : draftSfx);
    }

    public void CommitDraft()
    {
        master = draftMaster;
        music = draftMusic;
        sfx = draftSfx;
        mute = draftMute;

        Save();          // writes PlayerPrefs
        ApplyAudio();    // apply saved (same as draft now)
    }

    public void RevertDraft()
    {
        SyncDraftFromSaved();
        ApplyDraftAudio(); // returns preview back to saved values
    }

    public void ResetDraftAudioToDefaults()
    {
        draftMaster = 1f;
        draftMusic = 1f;
        draftSfx = 1f;
        draftMute = false;

        ApplyDraftAudio();
    }

}