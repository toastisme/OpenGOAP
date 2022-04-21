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

    public void Log(object message, Object sender){
        if(!showLogs){
            return;
        }
        Debug.Log($"<color={prefixColor}>{prefix}: {message}, {sender} </color>");
    }
}
