using System.Collections.Generic;
using System.Linq;
using UnityEngine;
    
/**
    * Tools to let you group components together and hide/show them in the inspector
    * Taken from https://forum.unity.com/threads/group-components-in-inspector.513931/
    * (the version modified by Kitsuba)
    */ 

public class ComponentGrouper : MonoBehaviour
{
    public List<ComponentGroup> groups = new List<ComponentGroup>();

    public ComponentGroup FindGroup(Component component)
    {
        return groups.FirstOrDefault(g => g.Components.Contains(component));
    }
}

[System.Serializable]
public class ComponentGroup
{
    public string          Name;
    public List<Component> Components = new List<Component>();

    public bool IsVisible  { get; set; } = true;
    public bool IsEditable { get; set; }
}

#if UNITY_EDITOR
    namespace Editor
    {
        using UnityEditor;
    
        [CustomEditor(typeof(ComponentGrouper))]
        public class ComponentGroupEditor : Editor
        {
            ComponentGrouper groups;
    
            private void OnEnable()
            {
                groups = (ComponentGrouper) target;
                foreach (var group in groups.groups)
                {
                    for (int i = group.Components.Count - 1; i >= 0; i--)
                    {
                        if (group.Components[i] == null)
                            group.Components.RemoveAt(i);
                        else
                        {
                            ChangeVisibility(group, group.IsVisible);
                        }
                    }
                }
            }
    
            private void ChangeVisibility(ComponentGroup group, bool aVisible)
            {
                for (int i = group.Components.Count - 1; i >= 0; i--)
                {
                    var c = group.Components[i];
                    if (c == null)
                    {
                        group.Components.RemoveAt(i);
                        continue;
                    }
    
                    if (aVisible)
                    {
                        c.hideFlags &= ~HideFlags.HideInInspector;
                        // required if the object was deselected in between
                        CreateEditor(c);
                    }
                    else
                        c.hideFlags |= HideFlags.HideInInspector;
                }
    
                group.IsVisible = aVisible;
    
                EditorUtility.SetDirty(target);
            }
    
            public override void OnInspectorGUI()
            {
                var oldColor   = GUI.color;
                var oldEnabled = GUI.enabled;
    
                if (GUILayout.Button("Add Group"))
                {
                    groups.groups.Add(new ComponentGroup());
                }
    
                for (var i = 0; i < groups.groups.Count; i++)
                {
                    var group = groups.groups[i];
                    if (group.IsEditable)
                    {
                        var components = groups.gameObject.GetComponents<Component>();
                        GUILayout.BeginHorizontal();
                        group.Name = GUILayout.TextField(group.Name);
                        if (GUILayout.Button("done", GUILayout.Width(40)))
                            group.IsEditable = false;
                        GUILayout.EndHorizontal();
                        foreach (var comp in components)
                        {
                            string name = comp.GetType().Name;
                            if (comp is ComponentGrouper g)
                                name = "ComponentGroup";
                            bool           isInList         = group.Components.Contains(comp);
                            ComponentGroup componentInGroup = groups.FindGroup(comp);
    
                            GUI.color   = isInList ? Color.green : oldColor;
                            GUI.enabled = comp != groups && (componentInGroup == null || componentInGroup == group);
                            if ((comp.hideFlags & HideFlags.HideInInspector) != 0)
                                name += "(hidden)";
                            if (GUILayout.Toggle(isInList, name) != isInList)
                            {
                                if (isInList)
                                    group.Components.Remove(comp);
                                else
                                    group.Components.Add(comp);
                            }
                        }
    
                        GUI.enabled = oldEnabled;
                    }
                    else
                    {
                        GUILayout.BeginHorizontal();
                        GUI.color = group.IsVisible ? Color.green : Color.red;
                        if (GUILayout.Button(group.Name))
                            ChangeVisibility(group, !group.IsVisible);
                        GUI.color = Color.red;
                        if (GUILayout.Button("hide", GUILayout.Width(40)))
                            ChangeVisibility(group, false);
                        GUI.color = Color.green;
                        if (GUILayout.Button("show", GUILayout.Width(40)))
                            ChangeVisibility(group, true);
                        GUI.color = oldColor;
                        if (GUILayout.Button("edit", GUILayout.Width(40)))
                        {
                            //ChangeVisibility(group, true);
                            group.IsEditable = true;
                        }
    
                        GUI.enabled = i > 0;
                        if (GUILayout.Button("^", GUILayout.Width(20)))
                        {
                            ComponentGroup moveGroup = groups.groups[i];
                            groups.groups.RemoveAt(i);
                            groups.groups.Insert(i - 1, moveGroup);
                            EditorUtility.SetDirty(target);
                        }
    
                        GUI.enabled = i < groups.groups.Count - 1;
                        if (GUILayout.Button("v", GUILayout.Width(20)))
                        {
                            ComponentGroup moveGroup = groups.groups[i];
                            groups.groups.RemoveAt(i);
                            groups.groups.Insert(i + 1, moveGroup);
                            EditorUtility.SetDirty(target);
                        }
    
                        GUI.enabled = true;
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            ChangeVisibility(groups.groups[i], true);
                            groups.groups.RemoveAt(i);
                        }
    
                        GUILayout.EndHorizontal();
                    }
                }
            }
        }
    }
#endif
