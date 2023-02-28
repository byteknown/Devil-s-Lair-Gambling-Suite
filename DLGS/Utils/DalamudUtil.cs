using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
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
    private readonly TargetManager targetManager;

    private readonly SendMessageHelper sendMessageHelper;
    private bool randomWaitingHost = false;
    private int lastRandomHost = 0;
    private bool randomWaitingPlayer = false;
    private int lastRandomPlayer = 0;
    private string playerName = "";

    public DalamudUtils(PartyList partyList, ChatGui chatGui, SigScanner sigScanner, TargetManager targetManager)
    {
        this.partyList = partyList;
        this.targetManager = targetManager;

        sendMessageHelper = new(sigScanner);

        chatGui.ChatMessage += OnChatMessage;
    }

    private void OnChatMessage(XivChatType type, uint senderId, ref SeString sender, ref SeString message, ref bool isHandled)
    {
        PlayerRandomEvent(type, ref message);
        TargetRandomEvent(type, ref message);
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

    public async Task<int> IngameRandom(int maxValue = 0)
    {
        randomWaitingHost = true;

        if (maxValue != 0)
        {
            SendChatMessage("/random " + maxValue);
        }
        else
        {
            SendChatMessage("/random");
        }

        int tries = 1;
        while (true)
        {
            if (!randomWaitingHost)
            {
                return lastRandomHost;
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

    private void PlayerRandomEvent(XivChatType type, ref SeString message)
    {
        if (randomWaitingHost && message.TextValue.Contains("You roll a "))
        {
            lastRandomHost = int.Parse(Regex.Match(message.TextValue, @"\d+").Value);
            randomWaitingHost = false;
        }
    }

    public async Task<int> GetTargetRandom()
    {
        if (!IsTargetingPlayer())
        {
            PluginLog.Warning("[GetTargetRandom] Tried to get random of non player target or no target.");
            return -2; // No target or non player
        }

        playerName = GetTargetName();
        randomWaitingPlayer = true;

        int tries = 1;
        while (true)
        {
            if (!randomWaitingPlayer)
            {
                return lastRandomPlayer;
            }

            tries++;
            if (tries > 150) // 30 seconds
            {
                PluginLog.Debug("[GetTargetRandom] Timeout reached, couldn't find message.");
                break;
            }

            await Task.Delay(200);
        }

        return -1; // Timeout
    }

    private void TargetRandomEvent(XivChatType type, ref SeString message)
    {
        if (randomWaitingPlayer && message.TextValue.Contains(playerName))
        {
            Match match = Regex.Match(message.TextValue, "Random! [a-zA-Z ']+ rolls a (\\d{1,3})\\.");

            if (match.Success)
            {
                lastRandomPlayer = int.Parse(match.Groups[1].Value);
                randomWaitingPlayer = false;
            }
        }
    }

    public bool IsTargetingPlayer()
    {
        if (targetManager.Target == null)
        {
            return false;
        }

        if (targetManager.Target.ObjectKind == Dalamud.Game.ClientState.Objects.Enums.ObjectKind.Player)
        {
            return true;
        }
        return false;
    }

    public string GetTargetName()
    {
        if (targetManager.Target == null)
        {
            return "No target";
        }

        return targetManager.Target.Name.ToString();
    }
}
