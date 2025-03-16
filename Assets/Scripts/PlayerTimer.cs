using UnityEngine;
using TMPro; // For TextMeshPro UI

public class PlayerTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI recordText; // UI Timer for the player
    private float playerTime = 0f;
    private bool timerRunning = true;

    void Start()
    {
        if (timerText == null)
        {
            GameObject textObj = GameObject.Find(gameObject.name + "_Timer");
            if (textObj != null)
                timerText = textObj.GetComponent<TextMeshProUGUI>();
        }

        if (recordText == null) {
            GameObject record = GameObject.Find("Record");
            if (record != null)
                recordText = record.GetComponent<TextMeshProUGUI>();
            recordText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (timerRunning)
        {
            playerTime += Time.deltaTime;
            UpdateTimerUI(playerTime);
        }
    }

    public void StopTimer()
    {
        timerRunning = false;
        recordText.text = "Record: " + playerTime + "s";
        recordText.gameObject.SetActive(true);
    }

    private void UpdateTimerUI(float time)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = $"Time: {minutes:D2}:{seconds:D2}";
        }
    }
}
