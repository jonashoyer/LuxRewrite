using System;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace LuxRewrite {

    
    public partial class Form1 : Form {

        public Form1() {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Form1_DragEnter);
            this.DragDrop += new DragEventHandler(Form1_DragDrop);

            label1.Text = "";
            button2.Enabled = false;

            comboBox1.DataSource = new ComboItem[] {
                new ComboItem{ ID = "unity19dark", Text = "Unity 2019 Darkmode" },
                new ComboItem{ ID = "unity19light", Text = "Unity 2019 Lightmode" },
            };
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        void Form1_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void Form1_DragDrop(object sender, DragEventArgs e) {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files) Console.WriteLine(file);
            button2.Enabled = true;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }


        private void Button2_Click(object sender, EventArgs e) {
            try {

                Program.RewriteFile(((ComboItem)comboBox1.SelectedItem).ID, openFileDialog1.FileName);

            } catch (SecurityException ex) {
                MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");
            }
        }

        private void Button1_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                string fn = openFileDialog1.FileName;
                button2.Enabled = true;
                label1.Text = fn.Substring(Math.Max(0, fn.Length - 28)); ;
            }
        }
    }
}
