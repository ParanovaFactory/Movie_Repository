using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieRepository
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection sql = new SqlConnection(@"Data Source=PREDATOR;Initial Catalog=Db_Movie;Integrated Security=True;TrustServerCertificate=True");

        void list()
        {
            sql.Open();
            SqlCommand cmd = new SqlCommand("select * from TblMovies", sql);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            sql.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            list();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            sql.Open();
            SqlCommand cmd = new SqlCommand("insert into TblMovies (Name,Cathegory,Link,Status) values(@p1,@p2,@p3,@p4)", sql);
            cmd.Parameters.AddWithValue("@p1", txtName.Text);
            cmd.Parameters.AddWithValue("@p2", txtType.Text);
            cmd.Parameters.AddWithValue("@p3", txtUrl.Text);
            cmd.Parameters.AddWithValue("@p4", 0);
            cmd.ExecuteNonQuery();
            sql.Close();
            list();
        }

        private void btnWatched_Click(object sender, EventArgs e)
        {
            sql.Open();
            SqlCommand cmd = new SqlCommand("update TblMovies set Status=@p1 where Id = @p2", sql);
            cmd.Parameters.AddWithValue("@p1", 1);
            cmd.Parameters.AddWithValue("@p2", lblId.Text);
            cmd.ExecuteNonQuery();
            sql.Close();
            list();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string path = "";
            lblId.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            path = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            ChromiumWebBrowser chromiumWebBrowser = new ChromiumWebBrowser(path);
            this.panel2.Controls.Add(chromiumWebBrowser);
            chromiumWebBrowser.Dock = DockStyle.Fill;
            chromiumWebBrowser.Load(path);
        }
    }
}
