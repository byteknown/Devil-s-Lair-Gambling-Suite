using DLGS.Utils;
using ImGuiNET;

namespace DLGS.Games;

public class Baccarat
{
    private readonly DalamudUtils dutils;

    private int player1Bet = 100;
    private bool enabled = false;

    public Baccarat(DalamudUtils dutils)
    {
        this.dutils = dutils;
    }

    public void BaccaratTab()
    {
        if (ImGui.BeginTabItem("Game"))
        {
            string host = "Moonhell";
            string totalBet = "members " + dutils.GetGroupMembersCount();

            if (ImGui.Checkbox("Enable", ref enabled))
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

                ImGui.InputInt("", ref player1Bet, 0);

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
}
