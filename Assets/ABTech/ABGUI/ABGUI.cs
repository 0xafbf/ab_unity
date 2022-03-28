using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasRenderer))]
public class ABGUI : Graphic
{

    public ABGUIConfig config;

    EventSystem eventSystem;
    ABInputModule input;

    float lineHeight = 20;
    float cursorY = 0;

    Vector2 mousePos;

    bool mouseIsPressed;
    bool mouseWasPressed;
    bool mouseJustPressed =>  mouseIsPressed && !mouseWasPressed;
    bool mouseJustReleased => !mouseIsPressed && mouseWasPressed;

    public enum IMDrawType {
        Quad,
        Text,
    }

    List<IMDrawType> targetDrawType = new List<IMDrawType>();
    List<int> targetIndex = new List<int>();

    struct IMQuad {
        public Vector4 rect;
        public Color color;
    }

    List<IMQuad> quadList = new List<IMQuad>();

    void Start() {
        eventSystem = EventSystem.current;
        Debug.Assert(eventSystem != null);
    }

    void Reset() {

        targetDrawType.Clear();
        targetIndex.Clear();
        cursorY = 0;
        quadList.Clear();
    }

    void Update() {
        if (eventSystem != null && input == null) {
            if (eventSystem.currentInputModule is ABInputModule inputModule) {
                input = inputModule;
            }

        }

        if (input != null) {
            PointerEventData pointer = input.GetLastPointerEvent();


            Camera currentEventCamera = canvas.worldCamera;
            Ray ray = currentEventCamera.ScreenPointToRay(pointer.position);

            Vector3 rayDirection = ray.direction;

            Vector3 deltaFromUIOrigin = transform.position - ray.origin;
            float deltaFromPlane = Vector3.Dot(deltaFromUIOrigin, transform.forward);

            float invScale = Vector3.Dot(ray.direction, transform.forward);

            Vector3 projection = rayDirection * deltaFromPlane / invScale + ray.origin;

            mousePos = transform.InverseTransformPoint(projection);

            mouseWasPressed = mouseIsPressed;
            mouseIsPressed = Input.GetMouseButton(0);

        }


    }


    public void Label(string v) {
        // TODO: Revisar TMPro_Private.cs
        // en particular la funcion GenerateTextMesh es lo que podriamos copiar


    }

    protected override void OnPopulateMesh(VertexHelper vh) {

        vh.Clear();

        int primitives = targetIndex.Count;
        for (int idx = 0; idx < primitives; idx++) {
            IMDrawType type = targetDrawType[idx];
            int targetIdx = targetIndex[idx];
            if (type == IMDrawType.Quad) {
                HandleDrawQuad(vh, targetIdx);
            } else {
                Debug.LogError("Unhandled primitive");
            }
        }

        Reset();
    }

    public void DrawQuad(Vector4 inRect, Color inColor) {
        int index = quadList.Count;
        quadList.Add(new IMQuad() {
            rect = inRect,
            color = inColor,
        });

        targetIndex.Add(index);
        targetDrawType.Add(IMDrawType.Quad);
    }

    public void HandleDrawQuad(VertexHelper vh, int quadIndex) {
        IMQuad quad = quadList[quadIndex];
        int vertIdx = vh.currentVertCount;
        Color32 color32 = quad.color;
        ref Vector4 v = ref quad.rect;
        vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(0f, 0f));
        vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(0f, 1f));
        vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(1f, 1f));
        vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(1f, 0f));

        
        vh.AddTriangle(vertIdx,   vertIdx + 1, vertIdx + 2);
        vh.AddTriangle(vertIdx+2, vertIdx + 3, vertIdx);

    }

    public static bool RectHasPoint(Vector4 rect, Vector2 point) {
        if (point.x < rect.x) return false;
        if (point.x > rect.z) return false;
        if (point.y < rect.y) return false;
        if (point.y > rect.w) return false;
        return true;
    }

    public bool Button(string v)
    {
        bool retval = false;

        float width = 100;
        float height = lineHeight;

        Vector4 rect = new Vector4();
        rect.x = 0;
        rect.y = cursorY;
        rect.z = width;
        rect.w = cursorY + height;

        cursorY += lineHeight;

        
        Color btnColor = config.btnColorNormal;

        bool pointerInRect = RectHasPoint(rect, mousePos);

        if (pointerInRect) {
            btnColor = config.btnColorHover;

            if (mouseIsPressed) {
                btnColor = config.btnColorClick;
            }

            if (mouseJustReleased) {
                retval = true;
            }

        }

        DrawQuad(rect, btnColor);


        return retval;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        SetVerticesDirty();


    }

    
}
