using UnityEngine;

public class SettingsSceneBoot : MonoBehaviour
{
    private void Start()
    {
        SettingsManager.Instance.SyncDraftFromSaved();
        SettingsManager.Instance.ApplyDraft();
    }
}