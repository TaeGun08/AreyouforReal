using UnityEngine;
using TMPro;  // TextMeshPro 네임스페이스
using System;

public class KillLog : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI killLogText;
    private float entryLifetime = 5f;       // 화면에 남아있는 시간

  // private void OnEnable()
  // {
  //     PlayerController.OnPlayerKnockoutEvent += AddKillEntry;
  // }

  // private void OnDisable()
  // {
  //     PlayerController.OnPlayerKnockoutEvent -= AddKillEntry;
  // }

   //  private void AddKillEntry(PlayerController attacker, PlayerController victim)
   //  {
   //      // AI는 무시 (LocalPlayer가 없으면 AI)
   //      if (attacker.LocalPlayer == null || victim.LocalPlayer == null)
   //          return;
   // 
   //      // 예: LocalPlayer.UserName (혹은 PlayerController에 정의된 닉네임 프로퍼티) 사용
   //      string attackerName = attacker.LocalPlayer.UserName;
   
   
   
   
   
   
   
    
   //      string victimName   = victim.LocalPlayer.UserName;
   // 
   //      killLogText.text += $"{attackerName} hit {victimName}\n";
   //  }
}