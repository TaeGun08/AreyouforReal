using TMPro;
using UnityEngine;

public class ZoneUI : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI inGameTimerText;
    public TextMeshProUGUI bzTimerText;

    private float elapsedGameTime = 0f;
    public float ElapsedGameTime => elapsedGameTime;

    private float totalBzTime;
    private float remainingBzTime;

    private bool isInitialized = false;

    public void Init(float reductionTime, float waitTime)
    {
        Debug.Log($"[ZoneUI] Init 호출됨: {reductionTime}, {waitTime}");
        totalBzTime = reductionTime + waitTime;
        remainingBzTime = totalBzTime;
        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;

        // 1) 게임 플레이 타이머
        elapsedGameTime += Time.deltaTime;
        inGameTimerText.text = FormatTime(elapsedGameTime);

        // 2) BZ 타이머
        remainingBzTime -= Time.deltaTime;
        if (remainingBzTime <= 0f)
        {
            remainingBzTime += totalBzTime;
        }
        bzTimerText.text = remainingBzTime.ToString("F1") + "s";
    }

    public void SetSurvivorCount(int count)
    {
        Debug.Log($"[ZoneUI] 생존자 수 설정 요청: {count}명");
    }
    // ui 작업은 안했고 이거 가져다가 쓰면됩니당.
    
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}