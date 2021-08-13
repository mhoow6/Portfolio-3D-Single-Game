﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
    public class PacketHandler
    {
        public static void S_ChatHandler(Session session, ArraySegment<byte> buffer)
        {
            C_Chat chat = new C_Chat();
            chat.Read(buffer);

            Console.WriteLine($"[전체채팅] {chat.playerId}: {chat.chat}");
        }

        public static void S_FileResponseHandler(Session session, ArraySegment<byte> buffer)
        {
            S_FileResponse response = new S_FileResponse();
            response.Read(buffer);

            FileRoom container = NetworkManager.Instance.session.fileRoom;
            container.Alloc(response.originSize);
            container.AddData(response);

            Console.WriteLine($"{response.fileName} 다운로드 상황 {container.seek} / {container.file.Length}");

            if (!container.IsFull()) // 한 번에 데이터를 못 받은 경우 뒷 부분에 이어서 전송시켜준다.
            {
                C_FileRequest request = new C_FileRequest(response.fileName, container.seek);
                NetworkManager.Instance.session.Send(request.Write());
            }

        }
    }
}
