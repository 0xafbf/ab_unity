using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ABGUI Config")]
public class ABGUIConfig : ScriptableObject {

    [Header("Button")]
    public Color btnColorNormal = Color.white;
    public Color btnColorHover = Color.gray;
    public Color btnColorClick = Color.yellow;
}
