using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle muteToggle;

    [SerializeField] private TMP_Text masterPercent;
    [SerializeField] private TMP_Text musicPercent;
    [SerializeField] private TMP_Text sfxPercent;

    private void OnEnable()
    {
        var s = SettingsManager.Instance;

        masterSlider.SetValueWithoutNotify(s.DraftMaster * 100f);
        musicSlider.SetValueWithoutNotify(s.DraftMusic * 100f);
        sfxSlider.SetValueWithoutNotify(s.DraftSfx * 100f);

        muteToggle.SetIsOnWithoutNotify(s.DraftMute);

        bool muted = s.DraftMute;
        masterSlider.interactable = !muted;
        musicSlider.interactable = !muted;
        sfxSlider.interactable = !muted;

        UpdateLabel(masterSlider, masterPercent);
        UpdateLabel(musicSlider, musicPercent);
        UpdateLabel(sfxSlider, sfxPercent);
    }

    public void OnMasterChanged(float v)
    {
        SettingsManager.Instance.SetDraftMaster(v / 100f);
        masterPercent.text = Mathf.RoundToInt(v) + "%";
    }

    public void OnMusicChanged(float v)
    {
        SettingsManager.Instance.SetDraftMusic(v / 100f);
        musicPercent.text = Mathf.RoundToInt(v) + "%";
    }

    public void OnSfxChanged(float v)
    {
        SettingsManager.Instance.SetDraftSfx(v / 100f);
        sfxPercent.text = Mathf.RoundToInt(v) + "%";
    }

    private void UpdateLabel(Slider slider, TMP_Text label)
    {
        label.text = Mathf.RoundToInt(slider.value) + "%";
    }

    public void OnMuteChanged(bool on)
    {
        SettingsManager.Instance.SetDraftMute(on);

        masterSlider.interactable = !on;
        musicSlider.interactable = !on;
        sfxSlider.interactable = !on;
    }

    public void RefreshFromDraft()
    {
        var s = SettingsManager.Instance;

        masterSlider.SetValueWithoutNotify(s.DraftMaster * 100f);
        musicSlider.SetValueWithoutNotify(s.DraftMusic * 100f);
        sfxSlider.SetValueWithoutNotify(s.DraftSfx * 100f);

        muteToggle.SetIsOnWithoutNotify(s.DraftMute);

        bool muted = s.DraftMute;
        masterSlider.interactable = !muted;
        musicSlider.interactable = !muted;
        sfxSlider.interactable = !muted;

        masterPercent.text = Mathf.RoundToInt(masterSlider.value) + "%";
        musicPercent.text = Mathf.RoundToInt(musicSlider.value) + "%";
        sfxPercent.text = Mathf.RoundToInt(sfxSlider.value) + "%";
    }
}