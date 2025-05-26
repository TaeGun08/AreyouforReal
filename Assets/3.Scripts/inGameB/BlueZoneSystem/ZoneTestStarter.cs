using UnityEngine;

public class ZoneTestStarter : MonoBehaviour
{
    private void Start()
    {
        if (ZoneManager.Instance != null)
        {
            Debug.Log("ZoneManager 호출 테스트 중: StartZone()");
        }
        else
        {
            Debug.LogWarning("ZoneManager 인스턴스가 존재하지 않습니다.");
        }
    }
}