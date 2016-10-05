using UnityEngine;
using System.Collections;

public class BuildingMenu : MonoBehaviour {
    private BuildingButton[] buttons;
    private bool menuOut;

    void Start()
    {
        buttons = transform.parent.GetComponentsInChildren<BuildingButton>();
        for ( int i=0;i<buttons.Length;i++ )
            buttons[i].Index = buttons.Length - i;
        menuOut = false;
    }
    
    public void ClickEvent()
    {
        menuOut = !menuOut;
        foreach ( BuildingButton button in buttons )
            button.Active = menuOut;
    }
}
