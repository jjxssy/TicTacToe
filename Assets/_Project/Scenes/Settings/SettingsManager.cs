using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    // =========================
    // AUDIO
    // =========================
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float master = 1f;
    [SerializeField] private float music  = 1f;
    [SerializeField] private float sfx    = 1f;
    [SerializeField] private bool mute    = false;

    // Audio (Draft / Preview)
    private float draftMaster, draftMusic, draftSfx;
    private bool draftMute;

    private const string KEY_MASTER = "settings_master";
    private const string KEY_MUSIC  = "settings_music";
    private const string KEY_SFX    = "settings_sfx";
    private const string KEY_MUTE   = "settings_mute";

    // =========================
    // VIDEO
    // =========================
    [Header("Video")]
    [SerializeField] private bool fullscreen = true;
    [SerializeField] private int resolutionIndex = 0;
    [SerializeField] private bool vSync = true;
    [SerializeField] private int fpsCap = 60;

    // Video (Draft / Preview)
    private bool draftFullscreen;
    private int draftResolutionIndex;
    private bool draftVSync;
    private int draftFpsCap;

    private Resolution[] availableResolutions;
    private int default1080pIndex = 0;

    private const string KEY_FULLSCREEN = "settings_fullscreen";
    private const string KEY_RESINDEX   = "settings_resindex";
    private const string KEY_VSYNC      = "settings_vsync";
    private const string KEY_FPSCAP     = "settings_fpscap";

    // =========================
    // LIFECYCLE
    // =========================
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        CacheResolutions();

        LoadSaved();
        SyncDraftFromSaved();

        ApplyDraft(); // start applied using draft (draft == saved at startup)
    }

    // =========================
    // PUBLIC READS (Saved)
    // =========================
    public float Master => master;
    public float Music  => music;
    public float Sfx    => sfx;
    public bool Mute    => mute;

    public bool Fullscreen => fullscreen;
    public int ResolutionIndex => resolutionIndex;
    public bool VSync => vSync;
    public int FpsCap => fpsCap;

    // =========================
    // PUBLIC READS (Draft)
    // =========================
    public float DraftMaster => draftMaster;
    public float DraftMusic  => draftMusic;
    public float DraftSfx    => draftSfx;
    public bool DraftMute    => draftMute;

    public bool DraftFullscreen => draftFullscreen;
    public int DraftResolutionIndex => draftResolutionIndex;
    public bool DraftVSync => draftVSync;
    public int DraftFpsCap => draftFpsCap;

    // =========================
    // DRAFT SETTERS (Preview)
    // =========================
    public void SetDraftMaster(float v) { draftMaster = Mathf.Clamp01(v); ApplyDraftAudio(); }
    public void SetDraftMusic(float v)  { draftMusic  = Mathf.Clamp01(v); ApplyDraftAudio(); }
    public void SetDraftSfx(float v)    { draftSfx    = Mathf.Clamp01(v); ApplyDraftAudio(); }
    public void SetDraftMute(bool on)   { draftMute   = on; ApplyDraftAudio(); }

    public void SetDraftFullscreen(bool on) { draftFullscreen = on; ApplyDraftVideo(); }
    public void SetDraftVSync(bool on)      { draftVSync = on; ApplyDraftVideo(); }
    public void SetDraftFpsCap(int cap)     { draftFpsCap = cap; ApplyDraftVideo(); }

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

    // =========================
    // APPLY (GENERAL)
    // =========================
    // Applies the CURRENT SAVED settings to the game
    public void ApplySaved()
    {
        ApplyAudio();
        ApplyVideo();
    }

    // Applies the CURRENT DRAFT settings to the game (preview)
    public void ApplyDraft()
    {
        ApplyDraftAudio();
        ApplyDraftVideo();
    }

    // =========================
    // COMMIT / REVERT
    // =========================
    // Apply button: make draft permanent, save, and apply saved
    public void CommitDraft()
    {
        // commit audio
        master = draftMaster;
        music  = draftMusic;
        sfx    = draftSfx;
        mute   = draftMute;

        // commit video
        fullscreen = draftFullscreen;
        resolutionIndex = draftResolutionIndex;
        vSync = draftVSync;
        fpsCap = draftFpsCap;

        SaveSaved();
        ApplySaved();
    }

    // Back/Cancel: restore draft from saved and apply draft (which equals saved now)
    public void RevertDraft()
    {
        SyncDraftFromSaved();
        ApplyDraft();
    }

    // =========================
    // DEFAULTS (Draft)
    // =========================
    public void ResetDraftAudioToDefaults()
    {
        draftMaster = 1f;
        draftMusic  = 1f;
        draftSfx    = 1f;
        draftMute   = false;

        ApplyDraftAudio();
    }

    public void ResetDraftVideoToDefaults()
    {
        draftFullscreen = true;
        draftVSync = true;
        draftFpsCap = 60;

        draftResolutionIndex = default1080pIndex;
        ClampResolutionIndices();

        ApplyDraftVideo();
    }

    // Optional helper if you ever want one big reset:
    public void ResetDraftAllToDefaults()
    {
        ResetDraftAudioToDefaults();
        ResetDraftVideoToDefaults();
        // ResetDraftControlsToDefaults();
    // ResetDraftGameplayToDefaults();
    // ResetDraftAccessibilityToDefaults();
    }

    // =========================
    // SYNC
    // =========================
    public void SyncDraftFromSaved()
    {
        // audio
        draftMaster = master;
        draftMusic  = music;
        draftSfx    = sfx;
        draftMute   = mute;

        // video
        draftFullscreen = fullscreen;
        draftResolutionIndex = resolutionIndex;
        draftVSync = vSync;
        draftFpsCap = fpsCap;

        ClampResolutionIndices();
    }

    private void ClampResolutionIndices()
    {
        if (availableResolutions == null || availableResolutions.Length == 0) return;
        resolutionIndex = Mathf.Clamp(resolutionIndex, 0, availableResolutions.Length - 1);
        draftResolutionIndex = Mathf.Clamp(draftResolutionIndex, 0, availableResolutions.Length - 1);
    }

    // =========================
    // SAVE / LOAD (Saved)
    // =========================
    public void SaveSaved()
    {
        PlayerPrefs.SetFloat(KEY_MASTER, master);
        PlayerPrefs.SetFloat(KEY_MUSIC,  music);
        PlayerPrefs.SetFloat(KEY_SFX,    sfx);
        PlayerPrefs.SetInt(KEY_MUTE, mute ? 1 : 0);

        PlayerPrefs.SetInt(KEY_FULLSCREEN, fullscreen ? 1 : 0);
        PlayerPrefs.SetInt(KEY_RESINDEX, resolutionIndex);
        PlayerPrefs.SetInt(KEY_VSYNC, vSync ? 1 : 0);
        PlayerPrefs.SetInt(KEY_FPSCAP, fpsCap);

        PlayerPrefs.Save();
    }

    public void LoadSaved()
    {
        master = PlayerPrefs.GetFloat(KEY_MASTER, 1f);
        music  = PlayerPrefs.GetFloat(KEY_MUSIC,  1f);
        sfx    = PlayerPrefs.GetFloat(KEY_SFX,    1f);
        mute   = PlayerPrefs.GetInt(KEY_MUTE, 0) == 1;

        fullscreen = PlayerPrefs.GetInt(KEY_FULLSCREEN, 1) == 1;

        // Default to 1080p if no saved value exists
        resolutionIndex = PlayerPrefs.HasKey(KEY_RESINDEX)
            ? PlayerPrefs.GetInt(KEY_RESINDEX)
            : default1080pIndex;

        vSync  = PlayerPrefs.GetInt(KEY_VSYNC, 1) == 1;
        fpsCap = PlayerPrefs.GetInt(KEY_FPSCAP, 60);
    }

    // =========================
    // APPLY AUDIO (Saved + Draft)
    // =========================
    private void ApplyAudio()
    {
        if (audioMixer == null) return;

        float m = mute ? 0.0001f : master;
        SetMixerVolume("MasterVol", m);
        SetMixerVolume("MusicVol",  mute ? 0.0001f : music);
        SetMixerVolume("SfxVol",    mute ? 0.0001f : sfx);
    }

    private void ApplyDraftAudio()
    {
        if (audioMixer == null) return;

        float m = draftMute ? 0.0001f : draftMaster;
        SetMixerVolume("MasterVol", m);
        SetMixerVolume("MusicVol",  draftMute ? 0.0001f : draftMusic);
        SetMixerVolume("SfxVol",    draftMute ? 0.0001f : draftSfx);
    }

    private void SetMixerVolume(string param, float linear)
    {
        float db = Mathf.Log10(Mathf.Max(linear, 0.0001f)) * 20f;
        audioMixer.SetFloat(param, db);
    }

    // =========================
    // APPLY VIDEO (Saved + Draft)
    // =========================
    private void ApplyVideo()
    {
        ApplyVideoInternal(fullscreen, resolutionIndex, vSync, fpsCap);
    }

    private void ApplyDraftVideo()
    {
        ApplyVideoInternal(draftFullscreen, draftResolutionIndex, draftVSync, draftFpsCap);
    }

    private void ApplyVideoInternal(bool fs, int resIndex, bool vs, int cap)
    {
        if (availableResolutions != null && availableResolutions.Length > 0)
        {
            resIndex = Mathf.Clamp(resIndex, 0, availableResolutions.Length - 1);
            var r = availableResolutions[resIndex];
            Screen.SetResolution(r.width, r.height, fs);
        }
        else
        {
            Screen.fullScreen = fs;
        }

        QualitySettings.vSyncCount = vs ? 1 : 0;
        Application.targetFrameRate = vs ? -1 : cap;
    }

    // =========================
    // RESOLUTION CACHE
    // =========================
    private void CacheResolutions()
    {
        var all = Screen.resolutions;
        List<Resolution> unique = new List<Resolution>();

        foreach (var r in all)
        {
            bool exists = false;
            foreach (var u in unique)
            {
                if (u.width == r.width && u.height == r.height) { exists = true; break; }
            }
            if (!exists) unique.Add(r);
        }

        unique.Sort((a, b) =>
        {
            int c = a.width.CompareTo(b.width);
            return c != 0 ? c : a.height.CompareTo(b.height);
        });

        availableResolutions = unique.ToArray();

        default1080pIndex = 0;
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            if (availableResolutions[i].width == 1920 && availableResolutions[i].height == 1080)
            {
                default1080pIndex = i;
                break;
            }
        }
    }
    
}