using TMPro;
using UnityEngine;

public class Rank : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text rankPointText;

    public void RankSetting(string rankIndex, RankData rankData)
    {
        rankText.text = rankIndex;
        playerNameText.text = rankData.PlayerName;
        rankPointText.text = rankData.RankPoint.ToString();
    }
}
