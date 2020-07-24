using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionDialog: MonoBehaviour
{
    private Rect windowRect = new Rect (40, 40, Screen.width - 80, Screen.height - 80);
    
    private bool show = false;

    private string Title;
    private readonly List<DialogAction> Actions = new List<DialogAction>();

    void OnGUI () 
    {
        if (show)
            windowRect = GUI.Window (0, windowRect, DialogWindow, "", style: new GUIStyle{ normal = { background = Texture2D.whiteTexture}});
    }

    // This is the actual window.
    void DialogWindow (int windowID)
    {
        float y = 5;
        GUI.Label(new Rect(5, y, windowRect.width, 72), Title, GSStyles.BigLabelText);
        
        foreach (var action in Actions)
        {
            y += 77;
            if (GUI.Button(new Rect(5, y, windowRect.width, 72), action.Title, GSStyles.Button))
            {
                show = false;
                action.Action();
            }
        }
    }

    // To open the dialogue from outside of the script.
    public void Show()
    {
        var size = 5 + 72 + (77 * Actions.Count) + 20;
        windowRect = new Rect (40, (Screen.height - size) / 2f, Screen.width - 80, size);
        show = true;
    }

    public ActionDialog WithTitle(string title)
    {
        Title = title;
        Actions.Clear();
        return this;
    }

    public ActionDialog AddAction(string title, Action action)
    {
        Actions.Add(new DialogAction {Action = action, Title = title});
        return this;
    }

    private class DialogAction
    {
        public Action Action;
        public string Title;
    }
    
}
