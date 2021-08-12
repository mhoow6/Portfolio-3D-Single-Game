using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ServerCore;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    public PacketQueue queue = new PacketQueue();
    

    public ServerSession session
    {  
        get;
        private set;
    }
    = new ServerSession();

    private void Awake()
    {
        Instance = this;

        Connector connector = new Connector();
        connector.Connect(Container.Instance.host, () => { return session; }, 1);
    }

    private void Update()
    {
        List<ArraySegment<byte>> list = queue.PopAll();

        if (list.Count > 0)
        {
            foreach (ArraySegment<byte> buff in list)
                PacketManager.Instance.OnRecvPacket(session, buff);
        }
        
    }

    public void Send(ArraySegment<byte> sendBuff)
    {
        session.Send(sendBuff);
    }

    public void Push(Session session, ArraySegment<byte> buff)
    {
        queue.Push(buff);
    }

    public bool IsFileDownloadCompleted()
    {
        return session.fileRoom.IsFull();
    }

    private void OnDestroy()
    {
        session.Disconnect();
    }
}
