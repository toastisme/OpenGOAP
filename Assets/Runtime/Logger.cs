using UnityEngine;
using Object = UnityEngine.Object;


public class Logger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    bool showLogs;
    [SerializeField]
    string prefix;
    [SerializeField]
    Color prefixColor;

    string hexColor;

    void OnValidate(){
        hexColor = "#"+ColorUtility.ToHtmlStringRGBA(prefixColor);
    }

    public void Log(object message, Object sender, bool bold=false){
        if(!showLogs){
            return;
        }
        string s = $"<color={hexColor}>{prefix}: {message}, {sender} </color>";
        if (bold){
            s = "<b>" + s + "</b>";
        }
        Debug.Log(s);
    }
}
