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

    // Video settings:

    [Header("Video")]
    [SerializeField] private bool fullscreen = true;
    [SerializeField] private int resolutionIndex = 0;
    [SerializeField] private bool vSync = true;
    [SerializeField] private int fpsCap = 60;

    // Draft (preview)
    private bool draftFullscreen;
    private int draftResolutionIndex;
    private bool draftVSync;
    private int draftFpsCap;

    // Cache available resolutions
    private Resolution[] availableResolutions;

    private const string KEY_FULLSCREEN = "settings_fullscreen";
    private const string KEY_RESINDEX = "settings_resindex";
    private const string KEY_VSYNC = "settings_vsync";
    private const string KEY_FPSCAP = "settings_fpscap";

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CacheResolutions();

        Load();
        SyncDraftFromSaved();
        ApplyDraftAudio();
        ApplyDraftVideo();
    }

    // --- getters ---
    // audio getters:
    public float Master => master;
    public float Music => music;
    public float Sfx => sfx;
    public bool Mute => mute;



    // --- setters ---
    // audio setters:
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

    // video setters:
    
    public void SetDraftFullscreen(bool on) { draftFullscreen = on; ApplyDraftVideo(); }
    public void SetDraftVSync(bool on) { draftVSync = on; ApplyDraftVideo(); }
    public void SetDraftFpsCap(int cap) { draftFpsCap = cap; ApplyDraftVideo(); }
    public bool DraftFullscreen => draftFullscreen;
    public int DraftResolutionIndex => draftResolutionIndex;
    public bool DraftVSync => draftVSync;
    public int DraftFpsCap => draftFpsCap;

    public void Save()
{
    // Audio
    PlayerPrefs.SetFloat(KEY_MASTER, master);
    PlayerPrefs.SetFloat(KEY_MUSIC, music);
    PlayerPrefs.SetFloat(KEY_SFX, sfx);
    PlayerPrefs.SetInt(KEY_MUTE, mute ? 1 : 0);

    // Video
    PlayerPrefs.SetInt(KEY_FULLSCREEN, fullscreen ? 1 : 0);
    PlayerPrefs.SetInt(KEY_RESINDEX, resolutionIndex);
    PlayerPrefs.SetInt(KEY_VSYNC, vSync ? 1 : 0);
    PlayerPrefs.SetInt(KEY_FPSCAP, fpsCap);

    PlayerPrefs.Save();
}

    public void Load()
    {
        // Audio loading:
        master = PlayerPrefs.GetFloat(KEY_MASTER, 1f);
        music = PlayerPrefs.GetFloat(KEY_MUSIC, 1f);
        sfx = PlayerPrefs.GetFloat(KEY_SFX, 1f);
        mute = PlayerPrefs.GetInt(KEY_MUTE, 0) == 1;
        // Video loading:
        fullscreen = PlayerPrefs.GetInt(KEY_FULLSCREEN, 1) == 1;
        resolutionIndex = PlayerPrefs.GetInt(KEY_RESINDEX, 0);
        vSync = PlayerPrefs.GetInt(KEY_VSYNC, 1) == 1;
        fpsCap = PlayerPrefs.GetInt(KEY_FPSCAP, 60);
    }

    // --- audio application ---

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
        // Audio draft sync:
        draftMaster = master;
        draftMusic = music;
        draftSfx = sfx;
        draftMute = mute;
        // Video draft sync:
        draftFullscreen = fullscreen;
        draftResolutionIndex = resolutionIndex;
        draftVSync = vSync;
        draftFpsCap = fpsCap;

        ClampResolutionIndices();
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
        // commit audio
        master = draftMaster;
        music = draftMusic;
        sfx = draftSfx;
        mute = draftMute;

        // commit video
        fullscreen = draftFullscreen;
        resolutionIndex = draftResolutionIndex;
        vSync = draftVSync;
        fpsCap = draftFpsCap;

        Save();

        // after commit, draft == saved, so applying draft is fine
        ApplyDraftAudio();
        ApplyDraftVideo();
    }

    public void RevertDraft()
    {
        SyncDraftFromSaved();
        ApplyDraftAudio();
        ApplyDraftVideo();
    }

    public void ResetDraftAudioToDefaults()
    {
        draftMaster = 1f;
        draftMusic = 1f;
        draftSfx = 1f;
        draftMute = false;

        ApplyDraftAudio();
    }

    // --- video application---

    // Clamps resolution indices to available resolutions to avoid out of range errors
    private void ClampResolutionIndices()
    {
        if (availableResolutions == null || availableResolutions.Length == 0) return;
        resolutionIndex = Mathf.Clamp(resolutionIndex, 0, availableResolutions.Length - 1);
        draftResolutionIndex = Mathf.Clamp(draftResolutionIndex, 0, availableResolutions.Length - 1);
    }

    public void StepDraftResolution(int delta)
    {
        if (availableResolutions == null || availableResolutions.Length == 0) return;
        draftResolutionIndex = Mathf.Clamp(draftResolutionIndex + delta, 0, availableResolutions.Length - 1);
        ApplyDraftVideo();
    }

    public string GetDraftResolutionText()
    {
        if (availableResolutions == null || availableResolutions.Length == 0) return "N/A";
        var r = availableResolutions[draftResolutionIndex];
        return $"{r.width}x{r.height}";
    }

    public void ApplyDraftVideo()
    {
        if (availableResolutions != null && availableResolutions.Length > 0)
        {
            var r = availableResolutions[draftResolutionIndex];
            Screen.SetResolution(r.width, r.height, draftFullscreen);
        }
        else
        {
            Screen.fullScreen = draftFullscreen;
        }

        QualitySettings.vSyncCount = draftVSync ? 1 : 0;
        Application.targetFrameRate = draftVSync ? -1 : draftFpsCap; // -1 lets platform decide when vsync on
    }
    
    private void CacheResolutions()
{
    var all = Screen.resolutions;
    var unique = new System.Collections.Generic.List<Resolution>();
    unique.Sort((a, b) => a.width.CompareTo(b.width));

    foreach (var r in all)
    {
        bool exists = false;

        foreach (var u in unique)
        {
            if (u.width == r.width && u.height == r.height)
            {
                exists = true;
                break;
            }
        }

        if (!exists)
            unique.Add(r);
    }

    availableResolutions = unique.ToArray();
}

}