using System.Text;
using UnityEngine;


// 랜덤 방번호 생성
public static class Room
{
    public static string CreateRandomCode(int length = 4)
    {
        StringBuilder sb = new StringBuilder();
        char[] chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();
        string str = string.Empty;
        
        for (int i = 0; i < length; i++)
        {
            sb.Append(chars[Random.Range(0, chars.Length)]);
        }
        
        str = sb.ToString();
        sb.Clear();
        
        return str;
    }
}
