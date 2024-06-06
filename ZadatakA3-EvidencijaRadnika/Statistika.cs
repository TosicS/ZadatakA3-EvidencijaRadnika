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

namespace ZadatakA3_EvidencijaRadnika
{
    public partial class Statistika : Form
    {
        SqlConnection konekcija = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Skola\MATURA\Programiranje\ZadatakA3-EvidencijaRadnika\ZadatakA3-EvidencijaRadnika\A3.mdf;Integrated Security=True;");

        public Statistika()
        {
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            string sqlUpit = "Select YEAR(Projekat.DatumPocetka) as Godina, COUNT(Distinct Projekat.ProjekatID) as 'Broj Projekata', " +
                "COUNT(Distinct Angazman.RadnikID) as 'Broj radnika' " +
                "from Projekat " +
                "inner join Angazman on Angazman.ProjekatID = Projekat.ProjekatID " +
                "where DATEDIFF(year, DatumPocetka, GETDATE()) < @parStarost " +
                "group by YEAR(Projekat.DatumPocetka) " +
                "order by YEAR(Projekat.DatumPocetka)";

            SqlCommand komanda = new SqlCommand(sqlUpit, konekcija);
            komanda.Parameters.AddWithValue("@parStarost", numericUpDown1.Value);
            SqlDataAdapter adapter = new SqlDataAdapter(komanda);
            DataTable tabela = new DataTable();

            try
            {
                adapter.Fill(tabela);

                dataGridView1.DataSource = tabela;

                chart1.DataSource = tabela;
                chart1.Series[0].XValueMember = "Godina";
                chart1.Series[0].YValueMembers = "Broj radnika";
                chart1.Series[0].IsValueShownAsLabel = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
