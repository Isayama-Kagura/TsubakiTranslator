using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsubakiTranslator.BasicLibrary
{
    public class HotkeyHandler
    {
        private IntPtr mainFormHandle;
        public IntPtr MainFormHandle { get => mainFormHandle; }

        private int id = 856;
        private byte modifiers;
        private int key;

        public int Id { get => id; }
        public void RegisterHotKey(IntPtr mainFormHandle, ScreenshotHotkey hotkey)
        {
            this.mainFormHandle = mainFormHandle;

            modifiers = hotkey.Modifiers;
            key = hotkey.Key;

            if (key != 0)
            {
                hotkey.Conflict = !User32.RegisterHotKey(mainFormHandle, id, modifiers, key);
            }

        }

        public void UnRegisterHotKey()
        {
            User32.UnregisterHotKey(mainFormHandle, id);
        }

        //public void ReRegisterHotKey(ScreenshotHotkey hotkey)
        //{
        //    if (key == 0)
        //    {
        //        User32.UnregisterHotKey(mainFormHandle, id);
        //    }
        //    else if (modifiers != hotkey.Modifiers || key != hotkey.Key)
        //    {
        //        User32.UnregisterHotKey(mainFormHandle, id);
        //        hotkey.Conflict = !User32.RegisterHotKey(mainFormHandle, id, hotkey.Modifiers, hotkey.Key);
        //    }
        //    modifiers = hotkey.Modifiers;
        //    key = hotkey.Key;

        //}


    }
}
