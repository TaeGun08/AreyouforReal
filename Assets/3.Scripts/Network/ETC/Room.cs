using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


// 랜덤 방번호 생성
public static class Room
{
    public static async Task<string>  CreateRandomCode(int length = 4)
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
        List<RoomData> roomData = await FirestoreManager.Instance.GetAllDocumentsAsync<RoomData>(FirebaseCollections.Rooms);

        if (roomData.Any(data => data.RoomCode == str)) //str과 일치하는 방이 이미 있는지 확인
        {
            return await CreateRandomCode(); //재귀
        }
        
        return str;
    }
}
