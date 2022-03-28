using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABDemoGUI : MonoBehaviour
{
    public ABGUI gui;

    int buttons = 4;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            buttons += 1;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            buttons -= 1;
        }

        if (buttons < 0) buttons = 0;

        gui.Label("Hola");

        for (int idx = 0; idx < buttons; idx++) {
            
            if (gui.Button("Boton")) {
                Debug.Log($"Hey! {idx}");
            }

        }

    }
}
