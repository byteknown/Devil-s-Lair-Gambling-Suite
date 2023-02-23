using Dalamud.Game.ClientState.Party;
using System.Collections.Generic;

namespace SamplePlugin.Utils;

public class DalamudUtils
{
    private PartyList partyList;

    public DalamudUtils(PartyList partyList)
    {
        this.partyList = partyList;
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
