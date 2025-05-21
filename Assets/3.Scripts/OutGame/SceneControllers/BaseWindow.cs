using UnityEngine;
using UnityEngine.UI;

public class BaseWindow : MonoBehaviour
{
    // protected virtual void Open()
    // {
    //     gameObject.SetActive(true);
    // }
    
    // public virtual void Close()
    // {
    //     gameObject.SetActive(false);
    // }
    
    public virtual void OnClickedExitButton()
    {
        gameObject.SetActive(false);
    }
}