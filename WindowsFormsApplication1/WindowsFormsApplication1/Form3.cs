using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;


namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream fd = new FileStream(@"G:\WindowsFormsApplication1\WindowsFormsApplication1\hello.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            string userinfo = textBox1.Text + ":" + textBox2.Text + "\n";


            byte[] byData;
            char[] charData;

            charData = userinfo.ToCharArray();
            byData = new byte[charData.Length];
            Encoder eee = Encoding.UTF8.GetEncoder();
            eee.GetBytes(charData, 0, charData.Length, byData, 0, true);

            fd.Seek(0, SeekOrigin.End);
            fd.Write(byData, 0, byData.Length);
            fd.Close();
            MessageBox.Show("ok");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
