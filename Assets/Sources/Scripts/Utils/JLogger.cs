using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Sources.Scripts.Utils
{
    /// <summary>
    /// Newbie logger. Now only show log TODO, Refactor, Think
    /// </summary>
    public static class JLogger
    {
        private static List<string> _msgList = new();
        private static readonly bool Show = DebugConfig.ShowDebug;
        private static int MsgNum = 1;
        private static bool _emptyLine = true;

        /// <summary>
        /// Show single line Msg
        /// </summary>
        public static void Msg(object msg, [CallerMemberName] string callerName = "",
            [CallerFilePath] string callerPath = "") => ShowMsg(msg, callerName, callerPath);

        private static void ShowMsg(object msg, string callerName, string callerPath)
        {
            if (Show)
            {
                if (_emptyLine) ShowEmptyLine();
                ShowMsgNumLine(callerName, callerPath);
                Debug.Log(msg);
            }
        }

        /// <summary>
        /// Build multi lines msg
        /// </summary>
        public static JLoggerBuilder Builder()
        {
            var builder = new JLoggerBuilder(ref _msgList);

            return builder;
        }

        public readonly struct JLoggerBuilder
        {
            private readonly List<string> _list;

            public JLoggerBuilder(ref List<string> msgList)
            {
                _list = msgList;
            }

            /// <summary>
            /// Add line to builder
            /// </summary>
            public JLoggerBuilder AddLine(object s)
            {
                _list.Add(s.ToString());
                return this;
            }

            /// <summary>
            /// Finishing build multi line msg and show
            /// </summary>
            public void Build(
                [CallerMemberName] string callerName = "",
                [CallerFilePath] string callerPath = "") => ShowMultiMsg(_list, callerName, callerPath);
        }

        private static void ShowMultiMsg(List<string> list, string callerName, string callerPath)
        {
            if (!Show) return;


            if (_emptyLine) ShowEmptyLine();
            ShowMsgNumLine(callerName, callerPath);

            foreach (var line in _msgList) Debug.Log(line);


            _msgList.Clear();
        }

        private static void ShowEmptyLine()
        {
            Debug.Log(MsgNum);
        }

        private static void ShowMsgNumLine(string callerName, string callerPath)
        {
            Debug.Log($"___ Msg: {MsgNum}. {callerPath.Split('\\').Last()} -> {callerName}");
            MsgNum++;
        }
    }
}