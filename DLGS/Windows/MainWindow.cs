using Dalamud.Interface.Windowing;
using DLGS.Games;
using DLGS.Utils;
using ImGuiNET;
using System;
using System.Numerics;

namespace DLGS.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly Baccarat baccarat;
    private readonly DeathRoll deathRoll;

    public MainWindow(DalamudUtils dutils) : base("Devil's Lair Gambling Suite", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        deathRoll = new(dutils);
        baccarat = new(dutils);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public override void Draw()
    {
        ImGui.Text("Welcome to the Devil's Lair Gambling Suite.");
        ImGui.Spacing();

        ImGui.BeginTabBar("MainTabBar");

        baccarat.BaccaratTab();
        deathRoll.DeathRollTab();
        ConfigTab();
        HelpTab();

        ImGui.EndTabBar();
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
}
