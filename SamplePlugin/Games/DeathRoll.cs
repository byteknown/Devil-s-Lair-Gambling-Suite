using DLGS.Utils;
using ImGuiNET;

namespace DLGS.Games;

public class DeathRoll
{
    private readonly DalamudUtils dutils;

    private int hostLastRoll = 0;
    private int playerLastRoll = 0;

    public DeathRoll(DalamudUtils dutils)
    {
        this.dutils = dutils;
    }

    public void DeathRollTab()
    {
        if (ImGui.BeginTabItem("DeathRoll"))
        {
            if (ImGui.Button("Random"))
            {
                _ = dutils.IngameRandom().ContinueWith(t => hostLastRoll = t.Result);
            }
            ImGui.Text("Result = " + hostLastRoll);

            ImGui.EndTabItem();
        }
    }
}
