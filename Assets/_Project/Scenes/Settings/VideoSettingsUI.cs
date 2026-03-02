using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoSettingsUI : MonoBehaviour
{
    [Header("Toggles")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vSyncToggle;

    [Header("Resolution")]
    [SerializeField] private TMP_Text resolutionText;
    [SerializeField] private Button resLeftButton;
    [SerializeField] private Button resRightButton;

    [Header("FPS Buttons")]
    [SerializeField] private Button fps30Button;
    [SerializeField] private Button fps60Button;
    [SerializeField] private Button fps120Button;

    private void OnEnable()
    {
        RefreshFromDraft();
    }

    public void RefreshFromDraft()
    {
        var s = SettingsManager.Instance;

        fullscreenToggle.SetIsOnWithoutNotify(s.DraftFullscreen);
        vSyncToggle.SetIsOnWithoutNotify(s.DraftVSync);

        resolutionText.text = s.GetDraftResolutionText();

        HighlightFps(s.DraftFpsCap);
    }

    public void OnFullscreenChanged(bool on)
    {
        SettingsManager.Instance.SetDraftFullscreen(on);
        resolutionText.text = SettingsManager.Instance.GetDraftResolutionText();
    }

    public void OnVSyncChanged(bool on)
    {
        SettingsManager.Instance.SetDraftVSync(on);
    }

    public void OnResolutionLeft()
    {
        SettingsManager.Instance.StepDraftResolution(-1);
        resolutionText.text = SettingsManager.Instance.GetDraftResolutionText();
    }

    public void OnResolutionRight()
    {
        SettingsManager.Instance.StepDraftResolution(+1);
        resolutionText.text = SettingsManager.Instance.GetDraftResolutionText();
    }

    public void OnFps30()  { SettingsManager.Instance.SetDraftFpsCap(30);  HighlightFps(30); }
    public void OnFps60()  { SettingsManager.Instance.SetDraftFpsCap(60);  HighlightFps(60); }
    public void OnFps120() { SettingsManager.Instance.SetDraftFpsCap(120); HighlightFps(120); }

    private void HighlightFps(int cap)
    {
        // simple selection style: selected = non-interactable
        fps30Button.interactable  = cap != 30;
        fps60Button.interactable  = cap != 60;
        fps120Button.interactable = cap != 120;
    }
}