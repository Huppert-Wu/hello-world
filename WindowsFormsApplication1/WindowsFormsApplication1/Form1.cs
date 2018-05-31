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

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FileStream fd = new FileStream(@"G:\WindowsFormsApplication1\WindowsFormsApplication1\hello.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            string userinfo = textBox1.Text + ":" + textBox2.Text + "\n";


            byte[] byData = new byte[200];
            char[] charData = new Char[200];

            fd.Seek(0, SeekOrigin.Begin);
            fd.Read(byData, 0, 200);

            Decoder dd = Encoding.UTF8.GetDecoder();
            dd.GetChars(byData, 0, byData.Length, charData, 0);
            fd.Close();

            string str = "";
            foreach(char xx in charData)
            {
                str += xx;
                if(xx == '\n')
                {
                    if (str == userinfo)
                    {
                        MessageBox.Show("ok");
                        this.Hide();
                        Form2 f2 = new Form2();
                        f2.Show();
                        return;
                    }
                    str = "";
                }
            }
            MessageBox.Show("error");
            return;

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 fr3 = new Form3();
            fr3.Show();
          

        }

    }
    /*
    string a = string.Format("select 职务 from number where number.账号='{0}'", textBox1.Text);
    SqlCommand cmd = new SqlCommand(a, sql);
    string x = string.Format("select 姓名 from number where number.账号='{0}'", Convert.ToInt32(textBox1.Text));
    SqlCommand cmd1 = new SqlCommand(x, sql);
    xingming = cmd1.ExecuteScalar().ToString();
     */
    public class initial
    {
        public SqlConnection init()
        {
            SqlConnection sql;
            String constr = "Data Source=B405-07;Initial Catalog=新建数据库;Integrated Security=True";
            //string constr = @"Data Source=.\SQLEXPRESS;Initial Catalog=abcd;Integrated Security=True";
            sql = new SqlConnection(constr);
            sql.Open();
            return sql;



        }
    }

}
