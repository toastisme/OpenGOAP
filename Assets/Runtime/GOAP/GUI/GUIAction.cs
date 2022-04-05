using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace GOAP{
public class GUIAction 
{
    /*
    GOAPAction nodeAction; // The action being visualised

    public bool IsSelected{get; private set;}
    Action<GUIAction> UpdatePanelDetails;

    // Appearance
    GUIStyle defaultStyle;
    GUIStyle selectedStyle;
    GUIStyle activeStyle;
    Rect rect;
    Rect callNumberRect;
    string displayName;

    public GUIAction(
        GOAPAction nodeAction, 
        Vector2 pos,
        Action<GUIAction> UpdatePanelDetails
        ){

        this.nodeAction = nodeAction;
        displayName = nodeAction.GetType().ToString();
        Vector2 size = GUIProperties.NodeSize();
        this.rect = new Rect(
            pos.x,
            pos.y,
            size.x,
            size.y
            );
        Vector2 callNumberSize = GUIProperties.CallNumberSize();
        Vector2 relCallNumberPos = GUIProperties.RelCallNumberPos();
        this.callNumberRect = new Rect(
            rect.x + relCallNumberPos.x,
            rect.y + relCallNumberPos.y,
            callNumberSize.x,
            callNumberSize.y
        );
        this.UpdatePanelDetails = UpdatePanelDetails;

    }

    public virtual void SetSelected(bool selected){
        if (selected){
            IsSelected = true;
            activeStyle = selectedStyle;
        }
        else{
            IsSelected = false;
            activeStyle = defaultStyle;
        }
    }

    public void Draw(){
        GUI.Box(rect, displayName, activeStyle);
    }

    public void Draw(int callNumber){
        GUI.Box(rect, displayName, activeStyle);
        GUI.Box(callNumberRect, callNumber.ToString(), activeStyle);
    }
    */


}
}