using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


// 랜덤 방번호 생성
public static class Room
{
    public static async Task<string> CreateRandomCode(int length = 4)
    {
        while (true)
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

            //아래로 중복방지코드
            //비동기로 모든 Rooms를 읽어들임
            List<string> roomKeys = await FirestoreManager.Instance.GetAllDocumentKeysAsync(FirebaseCollections.Rooms);

            if (roomKeys.All(roomKey => roomKey != str)) return str; //이미 Key가 있는지 검사
            length = 4;
            continue;

            break;
        }
    }
}
