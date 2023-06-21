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
using System.Windows.Input;

namespace DisconnectedEnvironment
{
    public partial class Form3 : Form
    {
        private string stringConnection = "Data Source=CHLUNIDIA;Initial Catalog=mahasiswa;Integrated Security=True;User=sa;Password=Chluni2350719";
        private SqlConnection connection;
        private string nim, nama, alamat, jk, prodi;
        private DateTime tgl;
        BindingSource customersBindingSource = new BindingSource();
        public Form3()
        {
            InitializeComponent();
            connection = new SqlConnection(stringConnection);
            this.bindingNavigator1.BindingSource = this.customersBindingSource;
            refreshForm();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            connection.Open();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(new SqlCommand("SELECT m.nim, m.nama_mahasiswa, m.alamat, m.jenis_kelamin, m.tgl_lahir, p.nama_prodi FROM dbo.mahasiswa m JOIN dbo.prodi p ON m.id_prodi = p.id_prodi", connection));
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            this.customersBindingSource.DataSource = ds.Tables[0];
            this.txtNIM.DataBindings.Add(new Binding("Text", this.customersBindingSource, "nim", true));
            this.txtNama.DataBindings.Add(new Binding("Text", this.customersBindingSource, "nama_mahasiswa", true));
            this.txtAlamat.DataBindings.Add(new Binding("Text", this.customersBindingSource, "alamat", true));
            this.cbxJK.DataBindings.Add(new Binding("Text", this.customersBindingSource, "jenis_kelamin", true));
            this.dtTanggalLahir.DataBindings.Add(new Binding("Text", this.customersBindingSource, "tgl_lahir", true));
            this.cbxProdi.DataBindings.Add(new Binding("Text", this.customersBindingSource, "nama_prodi", true));
            connection.Close();
        }

        private void clearBinding()
        {
            this.txtNIM.DataBindings.Clear();
            this.txtNama.DataBindings.Clear();
            this.txtAlamat.DataBindings.Clear();
            this.cbxJK.DataBindings.Clear();
            this.dtTanggalLahir.DataBindings.Clear();
            this.cbxProdi.DataBindings.Clear();
        }

        private void refreshForm()
        {
            txtNIM.Enabled = false;
            txtNama.Enabled = false;
            txtAlamat.Enabled = false;
            cbxJK.Enabled = false;
            dtTanggalLahir.Enabled = false;
            cbxProdi.Enabled = false;
            btnAdd.Enabled = true;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            clearBinding();
        }

        private void cbxProdi_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {

        }

        private void Prodicbx()
        {
            connection.Open();
            string str = "SELECT id_prodi, nama_prodi FROM dbo.prodi";
            SqlCommand cmd = new SqlCommand(str, connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            connection.Close();
            cbxProdi.DisplayMember = "nama_prodi";
            cbxProdi.ValueMember = "id_prodi";
            cbxProdi.DataSource = dt;
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtNIM.Enabled = true;
            txtNama.Enabled = true;
            txtAlamat.Enabled = true;
            cbxJK.Enabled = true;
            dtTanggalLahir.Enabled = true;
            cbxProdi.Enabled = true;
            btnAdd.Enabled = true;
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            Prodicbx();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            nim = txtNIM.Text.Trim();
            nama = txtNama.Text.Trim();
            alamat = txtAlamat.Text.Trim();
            jk = cbxJK.SelectedItem.ToString();
            tgl = dtTanggalLahir.Value;
            prodi = cbxProdi.SelectedValue.ToString();
            if (string.IsNullOrEmpty(nim) || string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(alamat) || string.IsNullOrEmpty(jk) || string.IsNullOrEmpty(prodi))
            {
                MessageBox.Show("Please fill in all identity fields!");
            }
            else
            {
                connection.Open();
                string query = "INSERT INTO mahasiswa (nim, nama_mahasiswa, alamat, jenis_kelamin, tgl_lahir, id_prodi) VALUES (@nim, @nama_mahasiswa, @alamat, @jenis_kelamin, @tgl_lahir, @id_prodi)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@nim", nim);
                command.Parameters.AddWithValue("@nama_mahasiswa", nama);
                command.Parameters.AddWithValue("@alamat", alamat);
                command.Parameters.AddWithValue("@jenis_kelamin", jk);
                command.Parameters.AddWithValue("@id_prodi", prodi);
                command.Parameters.AddWithValue("@tgl_lahir", tgl);
                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Data has been saved to the database.");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtNIM.Text = "";
            txtNama.Text = "";
            txtAlamat.Text = "";
            cbxJK.SelectedIndex = -1;
            cbxProdi.SelectedIndex = -1;
            dtTanggalLahir.Value = DateTime.Now;
        }
    }
}
