using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DialogInfo
{
    public ushort id;
    public ushort npc_id;
    public string dialog;
}

public static class DialogInfoTableManager
{
    public static List<DialogInfo> dialogInfoList = new List<DialogInfo>();

    public static void LoadTable(string filePath)
    {
        List<string> lines = TableManager.instance.GetLinesFromTableFileStream(filePath);

        for (int i = 1; i < lines.Count; i++)
        {
            string[] datas = lines[i].Split(',');

            DialogInfo dialogInfo;

            dialogInfo.id = ushort.Parse(datas[0]);
            dialogInfo.npc_id = ushort.Parse(datas[1]);
            dialogInfo.dialog = datas[2];

            dialogInfoList.Add(dialogInfo);
        }
    }

    public static List<DialogInfo> GetDialogInfoFromNPCID(ushort npcID)
    {
        List<DialogInfo> npcDialogs = new List<DialogInfo>();

        foreach (DialogInfo dialogInfo in dialogInfoList)
        {
            if (npcID == dialogInfo.npc_id)
                npcDialogs.Add(dialogInfo);
        }

        if (npcDialogs.Count != 0)
            return npcDialogs;
        else
            throw new System.NotSupportedException(npcID + " �� �ش��ϴ� ��簡 �����ϴ�.");
    }

    public static string GetDialogFromDialogID(ushort dialogID)
    {
        foreach (DialogInfo dialogInfo in dialogInfoList)
        {
            if (dialogID == dialogInfo.id)
                return dialogInfo.dialog;
        }
        
        throw new System.NotSupportedException(dialogID + " �� �ش��ϴ� ��簡 �����ϴ�.");
    }

    public static DialogInfo GetDialogInfoFromDialogID(ushort dialogID)
    {
        foreach (DialogInfo dialogInfo in dialogInfoList)
        {
            if (dialogID == dialogInfo.id)
                return dialogInfo;
        }
        
        throw new System.NotSupportedException(dialogID + " �� �ش��ϴ� ��簡 �����ϴ�.");
    }
}
