using Dalamud.Game;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Logging;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DLGS.Utils;

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

    public void SendChatMessage(string message)
    {
        sendMessageHelper.SendMessage(message);
    }

    public async Task<int> IngameRandom()
    {
        randomNumberWaiting = true;
        SendChatMessage("/random");

        int tries = 1;
        while (true)
        {
            if (!randomNumberWaiting)
            {
                return lastRandomNumber;
            }

            tries++;
            if (tries > 10)
            {
                PluginLog.Debug("[IngameRandom] Timeout reached, couldn't find message.");
                break;
            }

            await Task.Delay(200);
        }

        return -1;
    }

    private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
    {
        if (randomNumberWaiting && type == (XivChatType)2122 && message.TextValue.Contains("You roll a "))
        {
            lastRandomNumber = int.Parse(Regex.Match(message.TextValue, @"\d+").Value);
            randomNumberWaiting = false;
        }
    }
}
