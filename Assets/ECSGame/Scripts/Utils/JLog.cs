using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ECSGame.Scripts.Utils
{
    /// <summary>
    /// Newbie logger. Now only show log TODO, Refactor, Think
    /// </summary>
    public static class JLog
    {
        private static List<string> _msgList = new();
        private static readonly bool Show = DebugConfig.ShowDebug;
        private static readonly bool ShowLineNumber = DebugConfig.ShowLineNum;
        private static int _msgNum = 1;

        public static void Msg(object o, [CallerMemberName] string cName = "", [CallerFilePath] string cPath = "")
            => ShowMsg(o, cName, cPath);

        private static void ShowMsg(object o, string callerName, string callerPath)
        {
            if (!Show) return;

            ShowMsgNumLine(callerName, callerPath);
            Debug.Log(o);
        }

        /// <summary>
        /// Build multi lines msg
        /// </summary>
        public static JLoggerBuilder Builder() => new(ref _msgList);

        public readonly struct JLoggerBuilder
        {
            private readonly List<string> _list;

            public JLoggerBuilder(ref List<string> msgList) => _list = msgList;

            /// <summary>
            /// Add line to builder
            /// </summary>
            public JLoggerBuilder AddLine(object o)
            {
                _list.Add(o.ToString());
                return this;
            }

            /// <summary>
            /// Finishing build multi line msg and show
            /// </summary>
            public void Build([CallerMemberName] string cName = "", [CallerFilePath] string cPath = "")
                => ShowMultiMsg(cName, cPath);
        }

        private static void ShowMultiMsg(string callerName, string callerPath)
        {
            if (!Show) return;

            ShowMsgNumLine(callerName, callerPath);

            foreach (var line in _msgList) Debug.Log(line);

            _msgList.Clear();
        }

        private static void ShowMsgNumLine(string callerName, string callerPath)
        {
            if (!ShowLineNumber) return;

            Debug.Log($"\t{_msgNum}. \t{callerPath.Split('\\').Last().Split('.').First()} -> {callerName}()");
            _msgNum++;
        }
    }
}