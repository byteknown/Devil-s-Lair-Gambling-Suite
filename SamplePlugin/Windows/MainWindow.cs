using Dalamud.Interface.Windowing;
using ImGuiNET;
using SamplePlugin.Utils;
using System;
using System.Numerics;

namespace SamplePlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly DalamudUtils dutils;

    private int player1Bet = 100;
    bool activated = false;
    float width = 2.0f;

    private int lastRoll = 0;

    public MainWindow(DalamudUtils dutils) : base("Baccarat by Moonhell", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
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

    public void GeneratePlayers(string playerName)
    {
        ImGui.Text(playerName);
    }

    public void BetsPlacement(string bets)
    {
        ImGui.InputInt("", ref player1Bet, 0);
    }

    public void CardsButton()
    {
        if (ImGui.Button("1"))
        {
            // Do something when pressed -> Hit a card
        }
    }

    public override void Draw()
    {

        string host = "Moonhell";
        string totalBet = "members " + dutils.GetGroupMembersCount();
        ImGui.Text($"This is your main window. You won't need any more window if you plan on using tabs.");
        if (ImGui.Checkbox("Enable", ref activated))
        {
             
        }
        ImGui.BeginTabBar("MyTabBar");

        if (ImGui.BeginTabItem("Game"))
        {
            ImGui.BeginTable("Table1", 3);
            ImGui.TableSetupColumn("Player", 0, width = 2.0f);
            ImGui.TableSetupColumn("Bets", 0, width = 1.0f);
            ImGui.TableSetupColumn("Cards", 0, width = 1.0f);
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
                BetsPlacement("player " + player1Bet);
                ImGui.TableNextColumn();
                CardsButton();
                ImGui.TableNextColumn();

            }

            ImGui.EndTable();

            ImGui.EndTabItem();
        }

        if (ImGui.BeginTabItem("Config"))
        {
            // Add your content for Tab 2 here
            ImGui.Text("This is Tab 2");

            ImGui.EndTabItem();
        }
        if (ImGui.BeginTabItem("Help"))
        {
            // Add your content for Tab 2 here
            ImGui.Text("This is Tab 2");

            ImGui.EndTabItem();
        }

        ImGui.EndTabBar();
        
        
        
    }
}
