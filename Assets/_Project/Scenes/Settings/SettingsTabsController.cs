using UnityEngine;

public class SettingsTabsController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject panelAudio;
    [SerializeField] private GameObject panelVideo;
    [SerializeField] private GameObject panelControls;
    [SerializeField] private GameObject panelGameplay;
    [SerializeField] private GameObject panelAccessibility;

    [Header("Tab UI Scripts (for refreshing UI after reset)")]
    [SerializeField] private AudioSettingsUI audioUI;
    [SerializeField] private VideoSettingsUI videoUI;
    // later: ControlsSettingsUI controlsUI, GameplaySettingsUI gameplayUI, AccessibilitySettingsUI accessibilityUI

    private GameObject[] panels;
    private int currentIndex = 0;

    private void Awake()
    {
        panels = new[] { panelAudio, panelVideo, panelControls, panelGameplay, panelAccessibility };
        Show(0);
    }

    private void Show(int index)
{
    // if leaving a tab without Apply, discard preview changes
    SettingsManager.Instance.RevertDraft();

    currentIndex = index;

    for (int i = 0; i < panels.Length; i++)
        panels[i].SetActive(i == index);

    // refresh the UI of the newly opened tab so it shows reverted values
    if (currentIndex == 0 && audioUI != null) audioUI.RefreshFromDraft();
    if (currentIndex == 1 && videoUI != null) videoUI.RefreshFromDraft();
}

    public void ShowAudio() => Show(0);
    public void ShowVideo() => Show(1);
    public void ShowControls() => Show(2);
    public void ShowGameplay() => Show(3);
    public void ShowAccessibility() => Show(4);

    public void ResetCurrentTabToDefaults()
    {
        switch (currentIndex)
        {
            case 0: // Audio
                SettingsManager.Instance.ResetDraftAudioToDefaults();
                if (audioUI != null) audioUI.RefreshFromDraft();
                break;

            case 1: // Video
                SettingsManager.Instance.ResetDraftVideoToDefaults();
                if (videoUI != null) videoUI.RefreshFromDraft();
                break;

            case 2: // Controls
                // SettingsManager.Instance.ResetDraftControlsToDefaults();
                // controlsUI.RefreshFromDraft();
                break;

            case 3: // Gameplay
                // SettingsManager.Instance.ResetDraftGameplayToDefaults();
                // gameplayUI.RefreshFromDraft();
                break;

            case 4: // Accessibility
                // SettingsManager.Instance.ResetDraftAccessibilityToDefaults();
                // accessibilityUI.RefreshFromDraft();
                break;
        }
    }

    public void ResetAllToDefaults()
    {
        SettingsManager.Instance.ResetDraftAllToDefaults();

        if (audioUI != null) audioUI.RefreshFromDraft();
        if (videoUI != null) videoUI.RefreshFromDraft();
        // later refresh other tabs too
    }
    
    public void CancelAndRefresh()
{
    SettingsManager.Instance.RevertDraft();

    // refresh UI so sliders/toggles snap back visually
    if (audioUI != null) audioUI.RefreshFromDraft();
    if (videoUI != null) videoUI.RefreshFromDraft();
}
}