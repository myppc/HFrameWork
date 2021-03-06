using Assets.HFrameWork.Script.Tool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[InitializeOnLoad]
public class ComponentRefWindow : EditorWindow
{
    Dictionary<string, string> ComponentDict;
    static Texture2D iconTexture;
    private static EditorApplication.HierarchyWindowItemCallback hiearchyItemCallback;


    static ComponentRefWindow()
    {
        //初始化层级窗口项回调
        AddHiearchyListener();
    }

    static void AddHiearchyListener()
    {
        ComponentRefWindow.hiearchyItemCallback = new EditorApplication.HierarchyWindowItemCallback(ComponentRefWindow.DrawIcon);
        //加到委托列表
        EditorApplication.hierarchyWindowItemOnGUI = (EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(
            EditorApplication.hierarchyWindowItemOnGUI,
            ComponentRefWindow.hiearchyItemCallback);
    }

    [MenuItem("Tool/Auto Component Ref", false, 11)]
    static void AutoMenu()
    {

        var window = EditorWindow.GetWindow<ComponentRefWindow>(false, "AutoComponentRef");
        window.position = new Rect(200,200,200,100);
        window.Show();
    }

    private void OnFocus()
    {
        //EditorApplication.hierarchyWindowItemOnGUI = (EditorApplication.HierarchyWindowItemCallback)Delegate.Combine(
        //    EditorApplication.hierarchyWindowItemOnGUI,
        //    ComponentRefWindow.hiearchyItemCallback);
        AddHiearchyListener();
    }

    private void OnEnable()
    {
        ComponentDict = new Dictionary<string, string>();
        ComponentDict.Add("Image", "img");
        ComponentDict.Add("RectTransform", "rt");
        ComponentDict.Add("Animator", "atr");
        ComponentDict.Add("Text", "t");
        ComponentDict.Add("ScrollRect", "scr");
        ComponentDict.Add("InfiniteScroll", "iscr");
        ComponentDict.Add("Button", "btn");
    }

    private List<string> FilterComponent()
    {
        GameObject targetObj = null;
        if (Selection.transforms.Length > 0)
        {
            targetObj = Selection.gameObjects[0];
        }
        if (targetObj == null)
        {
            return null;
        }
        var nameList = targetObj.name.Split('#');
        var goName = nameList[0];
        var components = targetObj.GetComponents<Component>();
        var cpList = new List<string>();
        foreach (var item in components)
        {
            //当前有的组件
            var componentName = item.GetType().ToString().Split('.').Last();
            cpList.Add(componentName);
        }

        var ret = new List<string>();

        foreach (var cp in cpList)
        {
            if (ComponentDict.ContainsKey(cp))
            {
                if (!nameList.Contains(ComponentDict[cp]))
                {
                    ret.Add(cp);
                }
            }
        }


        return ret;
    }


    private void GUISelectComponent()
    {
        GameObject targetObj = null;
        if (Selection.transforms.Length > 0)
        {
            targetObj = Selection.gameObjects[0];
        }
        if (targetObj == null)
        {
            return;
        }
        var nameList = targetObj.name.Split('#');
        //显示名字
        var goName = nameList[0];
        GUILayout.Label($"GameObject : {goName}");

        var components = targetObj.GetComponents<Component>();
        foreach (var item in components)
        {
            //当前有的组件
            var componentName = item.GetType().ToString().Split('.').Last();
            //componentName = "Button"
            //nameList = "btn"
            //该控件是配置到导出目录中的
            if (ComponentDict.ContainsKey(componentName))
            {
                if (nameList.Contains(ComponentDict[componentName]))
                {
                    GUILayout.Label($"Select : {componentName}");
                }
            }
        }
    }

    private void OnGUI()
    {

        if (GUILayout.Button("Add Auto Ref"))
        {
            popMenu();
        }

        if (GUILayout.Button("Clear Ref"))
        {
            ClearRef();
        }

        GUISelectComponent();
    }

    public void RecordLuaIndex()
    {
        
        if (Selection.gameObjects.Length <= 0)
        {
            Debug.Log("需要选中一个gameobject 进行文件命名");

            return;
        }
        var nameList = new List<string>();
        var allGos = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        var previousSelection = Selection.objects;
        Selection.objects = allGos;
        var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
        Selection.objects = previousSelection;
        foreach (var trans in selectedTransforms)
        {
            if (trans.name.Contains('#'))
            {
                var name = trans.name.Split('#')[0];
                nameList.Add(name);
            }
        }
        var go = Selection.gameObjects[0];
        var fileName = "";

        while (true)
        {
            if (go.transform.parent == null)
            {
                break;
            }
            if (go.transform.parent.gameObject.name == "Canvas (Environment)")
            {
                fileName = go.transform.name;
                break;
            }
            go = go.transform.parent.gameObject;
        }
        if (fileName == "")
        {
            Debug.Log("-----------save fail :没有找到正确的根节点");
        }
        var content = "";
        foreach (var index in nameList)
        {
            var itemName = index.ToUpper();
            itemName = itemName.Replace(" ", "_");
            content = $"{content}    {itemName.ToUpper()} = '{index}',\n";
        }
        var str = "local go_index = {\n" + content + "\n}\nreturn  go_index";
        var result = FileHelper.SaveFile(Path.Combine(AppConfig.LUA_GO_INDEX, $"{fileName}_index.lua"), str);
    }

    public void ClearRef()
    {
        GameObject targetObj = null;
        if (Selection.transforms.Length > 0)
        {
            targetObj = Selection.gameObjects[0];
        }
        if (targetObj == null)
        {
            return;
        }
        targetObj.name = targetObj.name.Split('#')[0];
        EditorUtility.SetDirty(targetObj);
        RecordLuaIndex();
    }

    public void SelectComponent(object cpName)
    {
        GameObject targetObj = null;
        if (Selection.transforms.Length > 0)
        {
            targetObj = Selection.gameObjects[0];
        }
        if (targetObj == null)
        {
            return;
        }
        if ("GameObject" == cpName)
        {
            targetObj.name = targetObj.name.Contains("#") ? targetObj.name : $"{targetObj.name}#";
        }
        else
        {
            if (targetObj.name.Last() == '#')
            {
                targetObj.name += $"{ComponentDict[(string)cpName]}";
            }
            else 
            { 
                targetObj.name += $"#{ComponentDict[(string)cpName]}";
            }
        }
        EditorUtility.SetDirty(targetObj);
        RecordLuaIndex();
    }

    public  void popMenu()
    {
        var cpList = FilterComponent();
        if (cpList == null)
        {
            return;
        }
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("GameObject"), false, SelectComponent, "GameObject");
        foreach (var item in cpList)
        {
            menu.AddItem(new GUIContent(item), false, SelectComponent, item);
            menu.AddSeparator("");
        }
        menu.ShowAsContext();
    }
    

    public static void DrawIcon(int instanceID,Rect selectionRect)
    {
        var go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (go == null)
        {
            return;
        }
        if (!go.name.Contains("#"))
        {
            return;
        }
        if (iconTexture == null)
        {
            iconTexture = Resources.Load<Texture2D>("Editor/nfs_common_hpngtaiyang");
        }
        Rect r = new Rect(selectionRect);
        //矩形赋值
        r.xMin = 0;
        r.x = r.width - 15;
        r.width = 15;
        //GUI绘制
        GUI.Label(r, iconTexture);
    }
}
