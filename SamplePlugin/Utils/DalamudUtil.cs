using Dalamud.Game;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SamplePlugin.Utils;

public class DalamudUtils
{
    private readonly PartyList partyList;

    private readonly SendMessageHelper sendMessageHelper;
    private bool randomNumberWaiting = false;
    private int lastRandomNumber = 0;

    public DalamudUtils(PartyList partyList, ChatGui chatGui, SigScanner sigScanner)
    {
        this.partyList = partyList;

        sendMessageHelper = new(sigScanner);

        chatGui.ChatMessage += OnChatMessage;
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
    
}
