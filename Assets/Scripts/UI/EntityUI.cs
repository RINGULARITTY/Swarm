using UnityEngine;
using ImGuiNET;

public class EntityInfoUI : MonoBehaviour {
    private void OnEnable() => ImGuiUn.Layout += OnLayout;

    private void OnDisable() => ImGuiUn.Layout -= OnLayout;

    private void OnLayout() {
        ImGui.Begin("Selected Entities");

        if (SelectionManager.Instance.SelectedEntities.Count > 0) {
            foreach (Entity entity in SelectionManager.Instance.SelectedEntities) {
                StatsComponent health = entity.GetComponent<StatsComponent>();
                if (health != null) {
                    ImGui.Text($"Entity ID: {entity.ID}");
                    ImGui.Text($"Health: {health.Health}");
                    ImGui.Text($"Damage: {health.Damage}");
                    ImGui.Separator();
                }
            }
        }
        else 
            ImGui.Text($"Nothing Selected");

        ImGui.End();
    }
}
