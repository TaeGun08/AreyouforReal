using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Popup_Confirm : MonoBehaviour
{
    public static Popup_Confirm PopUpUI;
    
    [SerializeField] private Button confirmButton;
    // [SerializeField] private TMP_Text titleText;
    // [SerializeField] private TMP_Text messageText;

    [SerializeField] private GameObject confirmObject; //실제로 켤 팝업 (싱글톤을 위해)
    private void Awake()
    {
        PopUpUI = this;
    }

    public void OpenPopUI(Action onConfirmClick) //string title, string message, //델리게이트로 확인 버튼에 동적할당
    {
        // //title 셋팅
        // titleText.text = title;
        //
        // //내용 설정
        // messageText.text = message;
        
        //button.addlistner에 함수 등록
        confirmButton.onClick.AddListener(() => onConfirmClick());
        
        confirmObject.gameObject.SetActive(true);
    }


    // public Action OnEnterConfirmed; //Friend_Invite 명령
    //
    // public void OnClickedExitButton() // 나가기 클릭 시 구독 초기화
    // {
    //     OnEnterConfirmed = null;
    // }
    //
    // private void OnClickedConfirmButton() //확인 버튼 클릭
    // {
    //     // 구독된 메서드 호출
    //     OnEnterConfirmed?.Invoke();
    //     gameObject.SetActive(false);
    //     OnEnterConfirmed = null;
    // }
}
