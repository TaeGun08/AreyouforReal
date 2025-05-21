using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI inGameTimerText;
    public TextMeshProUGUI bzTimerText;

    private float elapsedGameTime = 0f;
    private ReduceCircle reduceCircle;

    // BlZ 타이머
    private float totalBzTime;
    private float remainingBzTime;

    private void Start()
    {
        reduceCircle = FindObjectOfType<ReduceCircle>();
        if (reduceCircle == null)
        {
            Debug.LogError("ReduceCircle 컴포넌트를 찾을 수 없습니다.");
            enabled = false;
            return;
        }

        // 총 BZ 시간 계산 및 초기화
        totalBzTime = reduceCircle.reductionTime + reduceCircle.waitTime;
        remainingBzTime = totalBzTime;
    }

    private void Update()
    {
        // 1) 게임 플레이 타이머
        elapsedGameTime += Time.deltaTime;
        inGameTimerText.text = FormatTime(elapsedGameTime);

        // 2) BZ 카운트다운
        remainingBzTime -= Time.deltaTime;
        if (remainingBzTime <= 0f)
        {
            remainingBzTime += totalBzTime; // 0 이하가 되면 다시 총시간만큼 더해서 루프
        }
        bzTimerText.text = remainingBzTime.ToString("F1") + "s";
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}