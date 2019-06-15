using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuxRewrite {
    static class Program {

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

            switch (mode) {
                case "unity19dark": {

                    byte[] search = { 0x74, 0x04, 0x33, 0xC0, 0xEB, 0x02, 0x8B, 0x07 };

                    bool changeDone = false;
                    for (int i = 0; i < buf.Length - search.Length; i++) {

                        for (int x = 0; x < search.Length; x++) {
                            if (buf[i + x] != search[x]) break;
                            if (x == search.Length - 1) {
                                Console.WriteLine("\nSearch found at " + i);
                                buf[i] = 0x75;
                                changeDone = true;

                                break;
                            }
                        }

                        if (changeDone) break;
                    }

                    if (changeDone) {
                        File.WriteAllBytes(filepath, buf);
                        MessageBox.Show("Change done!");
                    } else {
                        MessageBox.Show("Could not found expected!");
                    }

                    Console.WriteLine("\nSearch done " + buf.Length);

                    break;
                }

                case "unity19light": {
                        byte[] search = { 0x75, 0x04, 0x33, 0xC0, 0xEB, 0x02, 0x8B, 0x07 };

                        bool changeDone = false;
                        for (int i = 0; i < buf.Length - search.Length; i++) {

                            for (int x = 0; x < search.Length; x++) {
                                if (buf[i + x] != search[x]) break;

                                if (x != search.Length - 1) continue;

                                Console.WriteLine("\nSearch found at " + i);
                                buf[i] = 0x74;
                                changeDone = true;

                                break;
                            }

                            if (changeDone) break;
                        }

                        if (changeDone) {
                            File.WriteAllBytes(filepath, buf);
                            MessageBox.Show("Change done!");
                        } else {
                            MessageBox.Show("Could not found expected!");
                        }

                        Console.WriteLine("\nSearch done " + buf.Length);
                        break;
                }
            }

        }


    }

    class ComboItem {
        public string ID { get; set; }
        public string Text { get; set; }
    }
}
