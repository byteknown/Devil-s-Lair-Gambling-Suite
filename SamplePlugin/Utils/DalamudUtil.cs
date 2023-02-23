using Dalamud.Game;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Gui;
using System.Collections.Generic;

namespace SamplePlugin.Utils;

public class DalamudUtils
{
    private readonly PartyList partyList;
    private readonly ChatGui chatGui;

    private readonly SendMessageHelper sendMessageHelper;

    public DalamudUtils(PartyList partyList, ChatGui chatGui, SigScanner sigScanner)
    {
        this.partyList = partyList;
        this.chatGui = chatGui;

        sendMessageHelper = new(sigScanner);
    }

    public bool IsGrouped()
    {
        if (partyList.Length == 0)
        {
            return false;
        }
        return true;
    }

    public int GetGroupMembersCount()
    {
        return partyList.Length;
    }

    public string[] GetGroupMembersNames()
    {
        List<string> names = new();
        foreach (PartyMember p in partyList)
        {
            names.Add(p.Name.TextValue);
        }
        return names.ToArray();
    }

    public void SendChatMessage(string message)
    {
        sendMessageHelper.SendMessage(message);
    }
}
