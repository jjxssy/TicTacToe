using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlsSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityText;
    [SerializeField] private Toggle tooltipsToggle;

    private void OnEnable()
    {
        RefreshFromDraft();
    }

    public void RefreshFromDraft()
    {
        var s = SettingsManager.Instance;

        sensitivitySlider.SetValueWithoutNotify(s.DraftMouseSensitivity);
        sensitivityText.text = s.DraftMouseSensitivity.ToString("0.0");

        tooltipsToggle.SetIsOnWithoutNotify(s.DraftTooltips);
    }

    public void OnSensitivityChanged(float value)
    {
        SettingsManager.Instance.SetDraftMouseSensitivity(value);
        sensitivityText.text = value.ToString("0.0");
    }

    public void OnTooltipsChanged(bool on)
    {
        SettingsManager.Instance.SetDraftTooltips(on);
    }
}