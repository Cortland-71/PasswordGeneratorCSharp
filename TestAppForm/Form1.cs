using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TestAppForm
{
    public partial class PWG : Form
    {
        public PWG()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClearAll();
        }

        //----------------------------------------------------------------------------
        private List<object> ListOfBoxes()
        {
            List<object> boxes = new List<object>
            {
                checkBox0.Checked.Equals(true),
                checkBox1.Checked.Equals(true),
                checkBox2.Checked.Equals(true),
                checkBox3.Checked.Equals(true),
                checkBox4.Checked.Equals(true)
            };
            return boxes;
        }

        //(Part 1) checks what checkboxes are checked. Length of password and included string.
        //Passes them into the PrintPassword method.
        private void Button_Exe_Click(object sender, EventArgs e)
        {
            int passwordLength = Convert.ToInt32(LengthBox.Text);
            string included = textBox_Include.Text;

            PrintPassword(ListOfBoxes(), passwordLength, included);
        }

        private void PrintPassword(List<object> selectedBoxes, int length, string included)
        {
            var gen = new Generator();
            
            textBoxOP.Text = string.Join("", 
                gen.CreateFinalPassword(selectedBoxes, length, included));

            InfoBox.Text = textBoxOP.Text;
        }

        //----------------------------------------------------------------------------
        private void button_Clear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }
        //
        private void button_Save_Click(object sender, EventArgs e)
        {
            IOSystem saveObj = new IOSystem();

            string output = saveObj.Save(textBoxOP.Text, CommentBox.Text);

            InfoBox.Text = output;
        }
        //
        private void button_View_Click(object sender, EventArgs e)
        {
            IOSystem viewObj = new IOSystem();
            InfoBox.Text = viewObj.View();
        }
        //
        private void Leet_Button_CheckedChanged(object sender, EventArgs e)
        {
            bool check = Leet_Button.Checked;
            Generator leetObj = new Generator();

            textBox_Include.Text = leetObj.ConvertToLeetSpeak(textBox_Include.Text, check);
        }

        //Menu strip------------------------------------------------------------------
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string about = "About" +
                           "\n\n[PWG] Password Generator " +
                           "Version 1.0" +
                           "\n\nCortland Carrillo" +
                           "\nCSC 125 C# Programming 2018" +
                           "\nPath: C:\\PWG\\Passwords.txt";
            MessageBox.Show(about);
        }

        private void deleteLastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ShowDialogBox("Would you like to clear the last entry?","Delete Last Entry?"))
                TryCatch(RemoveLastEntry, "No past entries found.");
        }

        private void deleteDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ShowDialogBox("Would you like to delete entire directory?", "Delete Directory?"))
                TryCatch(DeleteDir, "No Directory was detected. " +
                    "Please save a new password to create a directory.");
        }

        //----------------------------------------------------------------------------
        delegate void toolBarDel();

        private void TryCatch(toolBarDel toolBarAction, string exception) 
        {
            try
            {
                toolBarAction();
            }
            catch (Exception)
            {
                InfoBox.Text = exception;
            }
            
        }

        private void RemoveLastEntry()
        {
            var io = new IOSystem();
            io.WriteUpdatedPasswords();
            InfoBox.Text = io.View();
        }

        private void DeleteDir()
        {
            var io = new IOSystem();
            io.DeleteDir();
            InfoBox.Text = "Directory was deleted successfully.";
        }

        //----------------------------------------------------------------------------
        private bool ShowDialogBox(string message, string title)
        {
            bool answer;
            answer = MessageBox.Show(message, title,
                     MessageBoxButtons.YesNo,
                     MessageBoxIcon.Question).Equals(DialogResult.Yes);
            return answer;
        }

        private void ClearAll()
        {
            LengthBox.SelectedIndex = 0;
            textBox_Include.Text = string.Empty;
            textBoxOP.Text = string.Empty;
            checkBox0.Checked = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            Leet_Button.Checked = false;
            textBox_Include.Text = string.Empty;
            textBoxOP.Text = string.Empty;
            InfoBox.Text = string.Empty;
            CommentBox.Text = string.Empty;
        }
    }
}