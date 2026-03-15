using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnTimer : MonoBehaviour
{
    public float turnDuration = 45f;
    public TextMeshProUGUI timerText;

    private float timeRemaining;
    private bool isRunning = false;

    private void Update()
    {
        if (!isRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            isRunning = false;
            timerText.text = "0";
            GameManager.Instance.EndTurn();
            return;
        }

        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
        timerText.color = timeRemaining <= 10f ? Color.red : Color.white;
    }

    public void StartTimer()
    {
        timeRemaining = turnDuration;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}