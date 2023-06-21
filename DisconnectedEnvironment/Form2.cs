using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisconnectedEnvironment
{
    public partial class Form2 : Form
    {
        private string stringConnection = "Data Source=CHLUNIDIA;Initial Catalog=mahasiswa;Integrated Security=True;User=sa;Password=Chluni2350719";
        private SqlConnection connection;

        private void refreshForm()
        {
            idp.Text = "";
            nmp.Text = "";
            idp.Enabled = false;
            nmp.Enabled = false;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
        }

        public Form2()
        {
            InitializeComponent();
            connection = new SqlConnection(stringConnection);
            refreshForm();
        }

        private void dataGridView()
        {
            connection.Open();
            string str = "SELECT id_prodi, nama_prodi FROM dbo.prodi";
            SqlDataAdapter da = new SqlDataAdapter(str, connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            connection.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'mahasiswaDataSet.prodi' table. You can move, or remove it, as needed.
            this.prodiTableAdapter.Fill(this.mahasiswaDataSet.prodi);

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            idp.Enabled = true;
            nmp.Enabled = true;
            btnSave.Enabled = true;
            btnClear.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nmProdi = nmp.Text;
            string idProdi = idp.Text;
            if (nmProdi == "")
            {
                MessageBox.Show("Masukkan Nama Prodi", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if(idProdi == "")
            {
                MessageBox.Show("Masukkan ID Prodi", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                connection.Open();
                string str = "INSERT INTO dbo.prodi (id_prodi, nama_prodi)" + "values(@id_prodi, @nama_prodi)";
                SqlCommand cmd = new SqlCommand(str, connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@id_prodi", idProdi);
                cmd.Parameters.AddWithValue("@nama_prodi", nmProdi);
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Data berhasil disimpan", "Sukses!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView();
                refreshForm();
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }
    }
}
