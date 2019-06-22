using System;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace LuxRewrite {

    
    public partial class Form1 : Form {

        string filepath;

        public Form1() {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            label1.Text = "";
            SetStatusText("");
            button2.Enabled = false;

            comboBox1.DataSource = new ComboItem[] {
                new ComboItem{ ID = "unity19.1dark", Text = "Unity 2019.1 Dark mode" },
                new ComboItem{ ID = "unity19.1light", Text = "Unity 2019.1 Light mode" },

                new ComboItem{ ID = "unity19.2dark", Text = "Unity 2019.2 beta Dark mode" },
                new ComboItem{ ID = "unity19.2light", Text = "Unity 2019.2 beta Light mode" },

                new ComboItem{ ID = "unity19.3dark", Text = "Unity 2019.3 alpha Dark mode" },
                new ComboItem{ ID = "unity19.3light", Text = "Unity 2019.3 alpha Light mode" },

                new ComboItem{ ID = "unity18dark", Text = "Unity 2018 Dark mode" },
                new ComboItem{ ID = "unity18light", Text = "Unity 2018 Light mode" },
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

                Patcher.RewriteFile(((ComboItem)comboBox1.SelectedItem).ID, filepath);

            } catch (SecurityException ex) {
                MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");
            }
        }

        private void Button1_Click(object sender, EventArgs e) {
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
    }
}
