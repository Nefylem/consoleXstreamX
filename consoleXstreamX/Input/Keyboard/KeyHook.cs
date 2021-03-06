﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleXstreamX.Input.Keyboard
{
    internal static class KeyHook
    {
        private static bool _active;
        private static readonly KeyDefinitions KeyboardHook;
        private static List<string> _keys;

        static KeyHook()
        {
            KeyboardHook = new KeyDefinitions();
            _keys = new List<string>();
        }

        public static void Enable()
        {
            if (_active) return;
            _active = true;

            KeyboardHook.KeyDown += keyboardHook_KeyDown;
            KeyboardHook.KeyUp += keyboardHook_KeyUp;

            KeyboardHook.Install();
        }

        static void keyboardHook_KeyUp(KeyDefinitions.VKeys key) { SetKey(key, false); }
        static void keyboardHook_KeyDown(KeyDefinitions.VKeys key) { SetKey(key, true); }


        private static void SetKey(KeyDefinitions.VKeys obj, bool set)
        {
            var key = obj.ToString().ToLower();
            var index = _keys.IndexOf(key);

            if (set)
            {
                if (index == -1) _keys.Add(key);
                return;
            }

            if (index > -1) _keys.Remove(key);
        }

        public static bool GetKey(string key)
        {
            if (key == null) return false;
            var index = _keys.IndexOf(key.ToLower());
            if (index == -1) return false;
            _keys.RemoveAt(index);
            return true;
        }
    }
}
