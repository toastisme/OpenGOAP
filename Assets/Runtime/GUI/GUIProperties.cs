using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GOAP{
public class GUIProperties 
{
    /**
     * Static methods to store general properties
     * for visualising GOAP components
     */

    //// Sizes

    public static Vector2 NodeSize(){return new Vector2(200, 120);}
    public static Vector2 CallNumberSize(){
        Vector2 guiNodeSize = GUIProperties.NodeSize();
        return new Vector2(
            guiNodeSize.x/6f,
            guiNodeSize.x/6f
        );
    }

    //// Colors

    public static Color DefaultNodeColor(){
        return new Color(87f/255.0f, 117f/255.0f, 144f/255.0f);
    }
    public static Color CallNumberColor(){
        return GUIProperties.DefaultNodeColor();
    }

    public static Color RunningTint(){
        return new Color(0f, .9f, 0);        
    }

    public static Color InPlanTint(){
        return new Color(1f, 1f, 1f);
    }

    public static Color DefaultTint(){
        return new Color(.8f, .8f, .8f);
    }

    //// Styles

    public static GUIStyle GUINodeStyle(){
        GUIStyle nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load(
            "builtin skins/lightskin/images/node0.png"
            ) as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);
        nodeStyle.normal.textColor = Color.white;
        nodeStyle.alignment = TextAnchor.UpperLeft;
        nodeStyle.fontStyle = FontStyle.Bold;
        nodeStyle.padding=new RectOffset(10,0,0,0);
        return nodeStyle;
    }

    public static GUIStyle SelectedGUINodeStyle(){
        GUIStyle selectedNodeStyle = GUINodeStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load(
            "builtin skins/lightskin/images/node0 on.png"
            ) as Texture2D;
        return selectedNodeStyle;
    }


}
}