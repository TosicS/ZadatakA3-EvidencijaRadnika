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

namespace ZadatakA3_EvidencijaRadnika
{
    public partial class Form1 : Form
    {
        SqlConnection konekcija = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Skola\MATURA\Programiranje\ZadatakA3-EvidencijaRadnika\ZadatakA3-EvidencijaRadnika\A3.mdf;Integrated Security=True;");
        public Form1()
        {
            InitializeComponent();
        }

        public void PrikaziLV()
        {
            string sqlUpit = "Select * from Projekat";
            SqlCommand komanda = new SqlCommand(sqlUpit, konekcija);
            SqlDataAdapter adapter = new SqlDataAdapter(komanda);
            DataTable tabela = new DataTable();

            try
            {
                listView1.Items.Clear();
                adapter.Fill(tabela);

                foreach(DataRow row in tabela.Rows)
                {
                    ListViewItem item = new ListViewItem(row[0].ToString());
                    item.SubItems.Add(row[1].ToString());
                    var datPoc = DateTime.Parse(row[2].ToString());
                    item.SubItems.Add(datPoc.ToString("dd.MM.yyyy"));
                    item.SubItems.Add(row[3].ToString());
                    item.SubItems.Add(row[4].ToString());
                    item.SubItems.Add(row[5].ToString());
                    // listItem.SubItems.Add(row["Opis"].ToString());
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ClearControls()
        {
            textBoxSifra.Text = "";
            textBoxNaziv.Text = "";
            textBoxDatumPocetka.Text = "";
            textBoxBudzet.Text = "";
            checkBoxZavrsen.Checked = false;
            richTextBoxOpis.Text = "";
        }

        public void UpisiUtxt()
        {
            string fileName = String.Format("log_{0}_{1}_{2}.txt", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);

            string path = @"C:\Skola\MATURA\Programiranje\ZadatakA3-EvidencijaRadnika\ZadatakA3-EvidencijaRadnika" + fileName;

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(String.Format("{0} - {1}", textBoxSifra.Text, textBoxNaziv.Text));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PrikaziLV();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxSifra.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBoxNaziv.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBoxDatumPocetka.Text = listView1.SelectedItems[0].SubItems[2].Text;
            textBoxBudzet.Text = listView1.SelectedItems[0].SubItems[3].Text;
            checkBoxZavrsen.Checked = Convert.ToBoolean(listView1.SelectedItems[0].SubItems[4].Text);
            richTextBoxOpis.Text = listView1.SelectedItems[0].SubItems[5].Text;
        }

        private void buttonObrisi_Click(object sender, EventArgs e)
        {
            if (textBoxSifra.Text != "")
            {
                DateTime datPoce = DateTime.ParseExact(textBoxDatumPocetka.Text, "dd.MM.yyyy", null);
                DateTime danDat = DateTime.Today;

                int starost = (danDat.Year - datPoce.Year);
                Boolean zavrsen = Convert.ToBoolean(checkBoxZavrsen.Checked);

                if (starost >= 5 && zavrsen == true) 
                {
                    string sqlUpit = "Delete from Projekat where ProjekatID = @parSifra";
                    SqlCommand komanda = new SqlCommand(sqlUpit, konekcija);
                    komanda.Parameters.AddWithValue("@parSifra", textBoxSifra.Text);
                    try
                    {
                        konekcija.Open();
                        komanda.ExecuteNonQuery();
                        MessageBox.Show("Uspesno ste obrisali projekat");
                        PrikaziLV();
                        UpisiUtxt();
                        ClearControls();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        konekcija.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Projekat nije zavrsen ili nije star 5 godina");
                }
            }
            else
            {
                MessageBox.Show("Izaberite projekat koji brišete");
            }
        }

        private void buttonIzadji_Click(object sender, EventArgs e)
        {
            //UpisiUtxt(); - tako pise u resenje, ali ne znam zasto bi se upisivalo u txt kad se izlazi iz programa
            this.Close(); //predpostavljam da treba tako
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Statistika stat = new Statistika();
            stat.Show();
        }
    }
}
