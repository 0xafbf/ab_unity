using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ABInputModule : StandaloneInputModule {

    public PointerEventData GetLastPointerEvent() {
        var mouseData = GetMousePointerEventData();
        var leftButtonData = mouseData.GetButtonState(PointerEventData.InputButton.Left).eventData;
        return leftButtonData.buttonData;
    }
}
