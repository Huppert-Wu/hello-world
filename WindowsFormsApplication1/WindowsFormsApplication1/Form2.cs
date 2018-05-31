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
namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
          }

        private void button1_Click(object sender, EventArgs e)
        {//更新
            try
            {
                initial start = new initial();
                SqlConnection sql = start.init();
                string str = string.Format("UPDATE Table SET houseID = '" + textBox2.Text + "',houseaddr='"+ textBox3.Text +"'houseowner = '"+textBox1.Text+"'");
                SqlDataAdapter da = new SqlDataAdapter(str, sql);
                DataSet ds = new DataSet();
                da.Fill(ds, "table");
                //dataGridView1.DataSource = ds.Tables[0];
                MessageBox.Show("ok");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return;
        }

        private void button4_Click(object sender, EventArgs e)
        {//查找
            initial start = new initial();
            SqlConnection sql = start.init();
            string str = string.Format("Select * from Table1");
            SqlDataAdapter da = new SqlDataAdapter(str, sql);
            DataSet ds = new DataSet();
            da.Fill(ds, "Table1");
            dataGridView1.DataSource = ds.Tables[0];

            return;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {//增加
            try
            {
                initial start = new initial();
                SqlConnection sql = start.init();
                string str = string.Format("INSERT INTO Table1 VALUES(" + textBox1.Text + ",'" + textBox2.Text + "','" + textBox3.Text + "')");
                SqlDataAdapter da = new SqlDataAdapter(str, sql);
                DataSet ds = new DataSet();
                da.Fill(ds, "table");
                //dataGridView1.DataSource = ds.Tables[0];
                MessageBox.Show("ok");
                return;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {//删除
            try
            {
                initial start = new initial();
                SqlConnection sql = start.init();
                string str = string.Format("DELETE FROM Table1 WHERE houseID = " + textBox1.Text);
                SqlDataAdapter da = new SqlDataAdapter(str, sql);
                DataSet ds = new DataSet();
                da.Fill(ds, "table");
                //dataGridView1.DataSource = ds.Tables[0];
                MessageBox.Show("ok");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return;
        }
    }
}
