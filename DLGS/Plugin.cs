using Dalamud.Game;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Party;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin;
using DLGS.Utils;
using DLGS.Windows;

namespace DLGS;

public sealed class Plugin : IDalamudPlugin
{
    public string Name => "Devil's Lair Gambling Suite";
    private const string CommandName = "/dlgs";

    private DalamudPluginInterface PluginInterface { get; init; }
    private CommandManager CommandManager { get; init; }
    public WindowSystem WindowSystem = new("Devil's Lair Gambling Suite");

    private DalamudUtils dutils { get; init; }
    private MainWindow MainWindow { get; init; }

    public Plugin(
        [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
        [RequiredVersion("1.0")] CommandManager commandManager,
        [RequiredVersion("1.0")] PartyList partyList,
        [RequiredVersion("1.0")] ChatGui chatGui,
        [RequiredVersion("1.0")] SigScanner sigScanner,
        [RequiredVersion("1.0")] TargetManager targetManager)
    {
        PluginInterface = pluginInterface;
        CommandManager = commandManager;

        dutils = new DalamudUtils(partyList, chatGui, sigScanner, targetManager);
        MainWindow = new MainWindow(dutils);
        WindowSystem.AddWindow(MainWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Opens the DLGS main window."
        });

        PluginInterface.UiBuilder.Draw += DrawUI;
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        MainWindow.Dispose();

        CommandManager.RemoveHandler(CommandName);
    }

    private void OnCommand(string command, string args)
    {
        // in response to the slash command, just display our main ui
        MainWindow.IsOpen = true;
    }

    private void DrawUI()
    {
        WindowSystem.Draw();
    }
}
