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
    public partial class Form4 : Form
    {
        private string stringConnection = "Data Source=CHLUNIDIA;Initial Catalog=mahasiswa;Integrated Security=True;User=sa;Password=Chluni2350719";
        private SqlConnection connection;
        public Form4()
        {
            InitializeComponent();
            connection = new SqlConnection(stringConnection);
            refreshForm();
        }

        private void refreshForm()
        {
            cbxNama.Enabled = false;
            cbxStatus.Enabled = false;
            cbxTahun.Enabled = false;
            cbxStatus.SelectedIndex = -1;
            cbxTahun.SelectedIndex = -1;
            txtNIM.Enabled = false;
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
        }

        private void dataGridView()
        {
            connection.Open();
            string str = "SELECT * FROM dbo.status_mahasiswa";
            SqlDataAdapter da = new SqlDataAdapter(str, connection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            connection.Close();
        }

        private void cbx_Nama()
        {
            connection.Open();
            string query = "SELECT nama_mahasiswa FROM mahasiswa";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                cbxNama.Items.Add(reader.GetString(0));
            }
            reader.Close();
            connection.Close();
        }

        private void cbx_Tahun()
        {
            int y = DateTime.Now.Year - 2010;
            string[] type = new string[y];
            int i = 0;
            for (i = 0; i < type.Length; i++)
            {
                if (i == 0)
                {
                    cbxTahun.Items.Add("2010");
                }
                else
                {
                    int l = 2010 + i;
                    cbxTahun.Items.Add(l.ToString());
                }
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'mahasiswaDataSet1.status_mahasiswa' table. You can move, or remove it, as needed.
            this.status_mahasiswaTableAdapter.Fill(this.mahasiswaDataSet1.status_mahasiswa);

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cbxNama.Enabled = true;
            cbxStatus.Enabled = true;
            cbxTahun.Enabled = true;
            txtNIM.Visible = true;
            cbx_Tahun();
            cbx_Nama();
            btnClear.Enabled = true;
            btnSave.Enabled = true;
            btnAdd.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nim = txtNIM.Text;
            string statusMahasiswa = cbxStatus.Text;
            string tahunMasuk = cbxTahun.Text;
            int count = 0;
            string tempKodeStatus = "";
            string kodeStatus = "";
            connection.Open();
            string str = "SELECT COUNT (*) FROM dbo.status_mahasiswa";
            SqlCommand cm = new SqlCommand(str, connection);
            count = (int)cm.ExecuteScalar();
            if (count == 0)
            {
                kodeStatus = "1";
            }
            else
            {
                string query = "SELECT TOP 1 id_status FROM dbo.status_mahasiswa ORDER BY id_status DESC";
                cm = new SqlCommand(query, connection);
                SqlCommand cmStatusMahasiswa = new SqlCommand(query, connection);
                SqlDataReader dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    tempKodeStatus = dr.GetString(0);
                }
                dr.Close();
                int tempKode = int.Parse(tempKodeStatus);
                tempKode++;
                kodeStatus = tempKode.ToString();
            }
            string queryString = "INSERT INTO dbo.status_mahasiswa (id_status, nim, status_mahasiswa, tahun_masuk) VALUES (@id_status, @nim, @status_mahasiswa, @tahun_masuk)";
            SqlCommand cmd = new SqlCommand(queryString, connection);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new SqlParameter("id_status", kodeStatus));
            cmd.Parameters.Add(new SqlParameter("NIM", nim));
            cmd.Parameters.Add(new SqlParameter("status_mahasiswa", statusMahasiswa));
            cmd.Parameters.Add(new SqlParameter("tahun_masuk", tahunMasuk));
            cmd.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Data berhasil disimpan", "Sukses!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            refreshForm();
            dataGridView();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshForm();
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }

        private void txtNIM_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbxNama_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string selectedName = cbxNama.SelectedItem.ToString();


            connection.Open();
            string str = "SELECT nim FROM mahasiswa WHERE nama_mahasiswa = @nama_mahasiswa";
            SqlCommand cm = new SqlCommand(str, connection);
            cm.Parameters.AddWithValue("@nama_mahasiswa", selectedName);
            SqlDataReader dr = cm.ExecuteReader();

            if (dr.Read())
            {
                string nim = dr.GetString(0);
                txtNIM.Text = nim;
            }
            else
            {
                txtNIM.Text = "";
            }

            dr.Close();
            connection.Close();
        }
    }
}
