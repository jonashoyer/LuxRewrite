using System;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace LuxRewrite {

    
    public partial class Form1 : Form {

        string filepath;
        bool isDarkMode = true;
        public Form1() {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            label1.Text = "";
            SetStatusText("");
            button2.Enabled = false;

            comboBox1.DataSource = new ComboItem[] {
                new ComboItem{ ID = "unity19.3", Text = "Unity 2019.3" },
                new ComboItem{ ID = "unity19.2", Text = "Unity 2019.2" },
                new ComboItem{ ID = "unity19.1", Text = "Unity 2019.1" },
                new ComboItem{ ID = "unity18", Text = "Unity 2018" },
            };
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        void Form1_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e) {

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            
            filepath = files[0];
            string[] arr = files[0].Split('\\');
            label1.Text = arr[arr.Length - 1];
            
            button2.Enabled = true;
            SetStatusText("");
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            SetStatusText("");
        }


        private void Button2_Click(object sender, EventArgs e) {

            try {
                string modeId = GetSelectedId() + (isDarkMode ? "dark" : "light");
                Patcher.RewriteFile(modeId, filepath);

            } catch (SecurityException ex) {
                MessageBox.Show($"Security\n\nMessage\n{ex.Message}", "Error");
            } catch(IOException ex) {
                MessageBox.Show($"IO\n\nMessage:\n{ex.Message}", "Error");
            }
        }

        private void Button1_Click(object sender, EventArgs e) {
            openFileDialog1.Filter = FilterById(GetSelectedId());
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                button2.Enabled = true;
                filepath = openFileDialog1.FileName;
                label1.Text = openFileDialog1.SafeFileName;
                SetStatusText("");
            }
        }

        public void SetStatusText(string str) {
            label2.Text = str;
        }

        private void DarkMode_CheckedChanged(object sender, EventArgs e) {
            isDarkMode = true;
        }

        private void LightMode_CheckedChanged(object sender, EventArgs e) {
            isDarkMode = false;
        }

        string GetSelectedId() {
            return ((ComboItem)comboBox1.SelectedItem).ID;
        }

        string FilterById(string id) {
            switch (id) {
                case "unity19.3":
                case "unity19.2":
                case "unity19.1":
                case "unity18":
                    return "Unity|unity.exe|All Files|*.*";
                default:
                    return "Exe Files|*.exe|All Files|*.*";
            }
        }
    }
}
