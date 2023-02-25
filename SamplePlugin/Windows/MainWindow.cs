using Dalamud.Interface.Windowing;
using ImGuiNET;
using DLGS.Utils;
using System;
using System.Numerics;

namespace DLGS.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly DalamudUtils dutils;

    // Baccarat
    private int baccaratPlayer1Bet = 100;
    private bool baccaratEnabled = false;

    // Death Roll
    private int deathRollHostLastRoll = 0;
    private int deathRollPlayerLastRoll = 0;

    public MainWindow(DalamudUtils dutils) : base("Devil's Lair Gambling Suite", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.dutils = dutils;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    private void BaccaratTab()
    {
        if (ImGui.BeginTabItem("Game"))
        {
            string host = "Moonhell";
            string totalBet = "members " + dutils.GetGroupMembersCount();

            if (ImGui.Checkbox("Enable", ref baccaratEnabled))
            {

            }

            ImGui.BeginTable("Table1", 3);
            ImGui.TableSetupColumn("Player", 0, 2.0f);
            ImGui.TableSetupColumn("Bets", 0, 1.0f);
            ImGui.TableSetupColumn("Cards", 0, 1.0f);
            ImGui.TableNextColumn();
            ImGui.Text(host);
            ImGui.TableNextColumn();
            ImGui.Text(totalBet);
            ImGui.TableNextColumn();
            if (ImGui.Button("This is a button"))
            {
                // Do something when pressed
            }
            ImGui.TableNextColumn();
            for (int i = 1; i <= 8; i++)
            {
                foreach (string name in dutils.GetGroupMembersNames())
                {
                    ImGui.Text("Player : " + name);
                }
                ImGui.TableNextColumn();

                ImGui.InputInt("", ref baccaratPlayer1Bet, 0);

                ImGui.TableNextColumn();

                if (ImGui.Button("1"))
                {
                    // Do something when pressed -> Hit a card
                }

                ImGui.TableNextColumn();
            }

            ImGui.EndTable();

            ImGui.EndTabItem();
        }
    }

    private void DeathRollTab()
    {
        if (ImGui.BeginTabItem("DeathRoll"))
        {
            if (ImGui.Button("Random"))
            {
                _ = dutils.IngameRandom().ContinueWith(t => deathRollHostLastRoll = t.Result);
            }
            ImGui.Text("Result = " + deathRollHostLastRoll);

            ImGui.EndTabItem();
        }
    }

    private void ConfigTab()
    {
        if (ImGui.BeginTabItem("Config"))
        {
            ImGui.Text("This is the Config tab");

            ImGui.EndTabItem();
        }
    }

    private void HelpTab()
    {
        if (ImGui.BeginTabItem("Help"))
        {
            ImGui.Text("This is the Help tab");

            ImGui.EndTabItem();
        }
    }

    public override void Draw()
    {
        ImGui.Text("Welcome to the Devil's Lair Gambling Suite.");
        ImGui.Spacing();

        ImGui.BeginTabBar("MainTabBar");

        BaccaratTab();
        DeathRollTab();
        ConfigTab();
        HelpTab();

        ImGui.EndTabBar();
    }
}
