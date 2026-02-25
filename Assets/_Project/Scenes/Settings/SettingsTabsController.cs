using UnityEngine;

public class SettingsTabsController : MonoBehaviour
{
    [SerializeField] private GameObject panelAudio;
    [SerializeField] private GameObject panelVideo;
    [SerializeField] private GameObject panelControls;
    [SerializeField] private GameObject panelGameplay;
    [SerializeField] private GameObject panelAccessibility;

    private GameObject[] panels;

    private void Awake()
    {
        panels = new[] { panelAudio, panelVideo, panelControls, panelGameplay, panelAccessibility };
        Show(0);
    }

    private void Show(int index)
    {
        for (int i = 0; i < panels.Length; i++)
            panels[i].SetActive(i == index);
    }

    public void ShowAudio() => Show(0);
    public void ShowVideo() => Show(1);
    public void ShowControls() => Show(2);
    public void ShowGameplay() => Show(3);
    public void ShowAccessibility() => Show(4);
}