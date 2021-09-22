using System.Collections.Generic;
using UnityEngine;

public class SortSection : DemoMenuSection
{
    public List<(string, string)> SortOptions;

    public delegate void DidSelectSortOption(int selectedIndex);

    public DidSelectSortOption Delegate;

    protected override string GetTitle()
    {
        return "Setup Sort Order";
    }

    protected override void DrawSectionBody()
    {
        GUILayoutOption[] options = new GUILayoutOption[2];
        options[0] = GUILayout.Width(Screen.width);
        options[1] = GUILayout.Height(60);
        for (var i = 0; i < SortOptions.Count; i++)
        {
            DemoGuiUtils.DrawButton(name: SortOptions[i].Item2 + SortOptions[i].Item1, opts: options, callback: () => {
                GoBack();
                Delegate(i);
            });
        }
    }
}
