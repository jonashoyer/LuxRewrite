using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuxRewrite {
    static class Patcher {

        static Form1 mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new Form1();
            Application.Run(mainForm);
        }

        public static void RewriteFile(string mode, string filepath) {

            byte[] buf = File.ReadAllBytes(filepath);
            bool wasChanged = false;

            switch (mode) {
                case "unity19.1dark": {

                    SetUnityRegistry(0x1);

                    byte[] search = { 0x74, 0x04, 0x33, 0xC0, 0xEB, 0x02, 0x8B, 0x07 };
                    wasChanged = FindAndReplaceBytes(buf, search, new byte[] { 0x75 });

                    break;
                }

                case "unity19.1light": {

                    SetUnityRegistry(0x0);

                    byte[] search = { 0x75, 0x04, 0x33, 0xC0, 0xEB, 0x02, 0x8B, 0x07 };
                    wasChanged = FindAndReplaceBytes(buf, search, new byte[] { 0x74 });

                    break;
                }

                case "unity19.2dark": {

                        SetUnityRegistry(0x1);

                        byte[] search = { 0x75, 0x15, 0x33, 0xC0, 0xEB, 0x13, 0x90 };
                        wasChanged = FindAndReplaceBytes(buf, search, new byte[] { 0x76 });

                        break;
                }

                case "unity19.2light": {

                        SetUnityRegistry(0x0);

                        byte[] search = { 0x76, 0x15, 0x33, 0xC0, 0xEB, 0x13, 0x90 };
                        wasChanged = FindAndReplaceBytes(buf, search, new byte[] { 0x75 });

                        break;
                }

                case "unity19.3dark": {

                        SetUnityRegistry(0x1);

                        byte[] search = { 0x75, 0x15, 0x33, 0xC0, 0xEB, 0x13, 0x90 };
                        wasChanged = FindAndReplaceBytes(buf, search, new byte[] { 0x74 });

                        break;
                }

                case "unity19.3light": {

                        SetUnityRegistry(0x0);

                        byte[] search = { 0x74, 0x15, 0x33, 0xC0, 0xEB, 0x13, 0x90 };
                        wasChanged = FindAndReplaceBytes(buf, search, new byte[] { 0x75 });

                        break;
                }

                case "unity18dark": {

                    SetUnityRegistry(0x1);

                    byte[] search = { 0x75, 0x08, 0x33, 0xC0, 0x48, 0x83, 0xC4, 0x30, 0x5B, 0xC3, 0x8B, 0x03, 0x48 };
                    wasChanged = FindAndReplaceBytes(buf, search, new byte[] { 0x74 });

                    break;
                }

                case "unity18light": {

                    SetUnityRegistry(0x0);

                    byte[] search = { 0x74, 0x08, 0x33, 0xC0, 0x48, 0x83, 0xC4, 0x30, 0x5B, 0xC3, 0x8B, 0x03, 0x48 };
                    wasChanged = FindAndReplaceBytes(buf, search, new byte[] { 0x75 });

                    break;
                }
            }

            if (wasChanged) {
                File.WriteAllBytes(filepath, buf);
                mainForm.SetStatusText("Success!");
            } else {
                mainForm.SetStatusText("Failed");
            }

            Console.WriteLine("\nSearch done " + buf.Length);

        }

        static void SetUnityRegistry(byte data) {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Unity Technologies\Unity Editor 5.x");
            key.SetValue("UserSkin_h307680651", data, RegistryValueKind.DWord);
            key.Close();
        }

        static bool FindAndReplaceBytes(byte[] data, byte[] search, byte[] replace) {

            int i, x, y;
            for (i = 0; i < data.Length - Math.Max(search.Length, replace.Length); i++) {

                for (x = 0; x < search.Length; x++) {
                    if (data[i + x] != search[x]) break;

                    if (x != search.Length - 1) continue;

                    Console.WriteLine("\nSearch found at " + i);

                    for(y = 0; y < replace.Length; y++) {
                        data[i + y] = replace[y];
                    }

                    return true;
                }
            }

            return false;
        }
    }



    class ComboItem {
        public string ID { get; set; }
        public string Text { get; set; }
    }
}
