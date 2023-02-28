using Dalamud.Logging;
using DLGS.Utils;
using ImGuiNET;
using System.Threading;
using System.Timers;

namespace DLGS.Games;

public class DeathRoll
{
    private readonly DalamudUtils dutils;

    private bool playing = false;
    private bool starting = false;
    private string player = "";
    private bool hostsTurn = false;
    private bool playersTurn = false;
    private int currentRoll = 0;
    private int lastRoll = 0;

    private string status = "Ready.";

    private int hostLastRoll = 0;
    private int playerLastRoll = 0;

    public DeathRoll(DalamudUtils dutils)
    {
        this.dutils = dutils;
    }

    public void DeathRollTab()
    {
        if (lastRoll != 0)
        {
            if (starting)
            {
                DetermineWhoStarts();
                lastRoll = 0;
                starting = false;
            }

            if (playersTurn)
            {
                // idk
            }
        }

        if (ImGui.BeginTabItem("DeathRoll"))
        {
            ImGui.Text("================ DEBUG =================");

            if (ImGui.Button("Random"))
            {
                _ = dutils.IngameRandom().ContinueWith(t => hostLastRoll = t.Result);
            }
            ImGui.Text("Host Last Roll = " + hostLastRoll);
            ImGui.Text("Player Last Roll = " + playerLastRoll);

            ImGui.EndTabItem();

            ImGui.Text("Is targeting player : " + dutils.IsTargetingPlayer());
            ImGui.Text("Target name : " + dutils.GetTargetName());

            if (dutils.IsTargetingPlayer())
            {
                ImGui.SameLine();
                if (ImGui.Button("Target Random"))
                {
                    _ = dutils.GetTargetRandom(dutils.GetTargetName()).ContinueWith(t => playerLastRoll = t.Result);
                }
            }

            ImGui.Text("================ ACTUAL UI ===============");

            ImGui.Text("Status : " + status);

            ImGui.Spacing();

            if (playing)
            {
                ImGui.Text("Last roll : " + currentRoll);

                if (starting)
                {
                    if (ImGui.Button("Determine who starts"))
                    {
                        dutils.SendChatMessage("I will now do a `/random 2` to determine who begins.\nIf it lands on 1, I start.\nIf it lands on 2, you start."); // TODO add to config
                        _ = dutils.IngameRandom(2).ContinueWith(t => lastRoll = t.Result);
                    }
                }
                else if (hostsTurn)
                {
                    if (ImGui.Button("Roll"))
                    {
                        // wtf do I put here
                    }
                }
                else if (!hostsTurn)
                {
                    // idk either
                } 
            }
            else
            {
                if (ImGui.Button("Start with Target"))
                {
                    if (dutils.IsTargetingPlayer())
                    {
                        player = dutils.GetTargetName();
                        playing = true;
                        starting = true;
                        status = "Playing with " + player + ", waiting on inital roll input.";

                        dutils.SendChatMessage("I am now playing Death Roll with " + player + " !");
                    }
                    else
                    {
                        status = "You need to target a valid player to start.";
                    }
                }
            }

            ImGui.Spacing();

            if (ImGui.Button("Reset (No confirmation !)"))
            {
                playing = false;
                starting = false;
                player = "";
                hostsTurn = false;
                currentRoll = 0;
                lastRoll = 0;

                status = "Ready.";
            }
        }
    }

    private void DetermineWhoStarts()
    {
        if (lastRoll == 1)
        {
            dutils.SendChatMessage("I start ! I will now do `/random`."); // TODO add to config
            status = "Host starts. Waiting on input.";
            hostsTurn = true;
        }
        else
        {
            dutils.SendChatMessage("You start ! Please do `/random` in the next 30s to begin."); // TODO add to config + include timeout
            status = "Player starts. Waiting on roll.";
            _ = dutils.GetTargetRandom(player).ContinueWith(t => lastRoll = t.Result);
        }
    }
}
