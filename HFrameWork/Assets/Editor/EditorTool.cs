using Assets.HFrameWork.Script.Tool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

#region << 版 本 注 释 >>
/*----------------------------------------------------------------
* 项目名称 ：Assets.HFrameWork.Script.LuaBridge.Editor
* 类 名 称 ：Class1
* 命名空间 ：Assets.HFrameWork.Script.LuaBridge.Editor
* 作    者 ：myppc
* 创建时间 ：2022/2/21 10:19:41
* 更新时间 ：2022/2/21 10:19:41
* 版 本 号 ：v1.0.0.0
*******************************************************************
* Copyright @ myppc 2022. All rights reserved.
*******************************************************************
//----------------------------------------------------------------*/
#endregion
public class EditorTool
{ 

    [MenuItem("Tool/Gen Lua Hint")]
    static void GenLuaHint()
    {
        var path = Path.Combine(Application.dataPath, "HFrameWork/Script/LuaBridge/LuaCallCSharpHelper.cs");
        var content = FileHelper.ReadFile(path);
        var lines = content.Split('\n');
        var funcList = new List<string>();
        foreach (var line in lines)
        {
            if (line.Contains("//"))
            {
                continue;
            }
            if (line.Contains("public static "))
            {
                var newline = line.Replace("public static ", "");
                newline = newline.TrimStart();

                var funcStart = newline.IndexOf(" ");
                var funcLen = newline.IndexOf("(") - funcStart;
                var funcName = newline.Substring(funcStart, funcLen);
                var argStr = "";
                if (newline.Contains("()"))
                {
                    argStr = "()";
                }
                else
                {
                    var paramStart = newline.IndexOf('(');
                    var paramEnd = newline.IndexOf(')');
                    var paramStr = newline.Substring(paramStart + 1, paramEnd - paramStart -1);
                    //"string abName, Action<string, AssetBundle> finishCallback"
                    while (true)
                    {
                        if (!paramStr.Contains("<"))
                        {
                            break;
                        }
                        else
                        {
                            var s = paramStr.IndexOf("<");
                            var e = paramStr.IndexOf(">") +1;
                            var r = paramStr.Substring(s, e - s);
                            paramStr = paramStr.Replace(r, "");
                        }
                    }
                    paramStr = paramStr.Replace("params", "");
                    var paramList = paramStr.Split(',');
                    foreach (var param in paramList)
                    {
                        var newparam = param.TrimStart();
                        var splitIndex = newparam.IndexOf(" ");
                        var arg = newparam.Substring(splitIndex, newparam.Length - splitIndex);
                        if (arg.Contains("="))
                        {
                            var e = arg.IndexOf("=");
                            arg = arg.Substring(0, e);
                        }
                        argStr = argStr + arg + ",";
                    }
                    argStr = argStr.Substring(0, argStr.Length - 1);
                    argStr = "(" + argStr + ")";
                }
                funcList.Add($"{funcName} = function{argStr} end");
            }


        }

        var luaMsg = "local code_hints_lua_bridge = {\n";
        foreach (var func in funcList)
        {
            luaMsg = luaMsg + $"    {func} ,\n";
        }
        luaMsg = luaMsg + "}\nreturn code_hints_lua_bridge";
        var savaPath = Path.Combine(Application.dataPath, "LuaScripts/system/code_hints/code_hints_lua_bridge.lua");
        FileHelper.SaveFile(savaPath,luaMsg);
        Debug.Log("Gen lua hint Succ");
    }
}
