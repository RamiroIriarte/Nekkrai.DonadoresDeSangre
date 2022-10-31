using BE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FE
{
    public partial class Form1 : Form
    {
        int a = 0;
        int b = 0;
        int ab = 0;
        int o = 0;

        Donante donante = new Donante();
        DataTable dtDonantes = new DataTable() { TableName = "Donantes" };
        DataTable dtGrupoSanguineo = new DataTable() { TableName = "Cargados"};

        const string DIRECCION_XML = @"C:\Users\Ramiro\source\repos\DonadoresDeSangre\";
        public Form1()
        {
            InitializeComponent();

            //Creamos columnas del Data table Donantes
            dtDonantes.Columns.Add("Nombre");
            dtDonantes.Columns.Add("Edad");
            dtDonantes.Columns.Add("DNI");
            dtDonantes.Columns.Add("Grupo Sanguineo");

            //Creamos columnas del Data table GrupoSanguineo
            dtGrupoSanguineo.Columns.Add("Grupo Sanguineo");
            dtGrupoSanguineo.Columns.Add("Cargados");

            //Lectura y cargado de datos
            Leer_DT();
            Leer_dtGrupos();
            dgvDonantes.DataSource = dtDonantes;
            dgvGrupoCargados.DataSource = dtGrupoSanguineo;


            if (System.IO.File.Exists(@"C:\Users\Ramiro\source\repos\DonadoresDeSangre\cargados.xml"))
            {
                //Si hay datos cargados le damos el valor ya existente.
                a = Convert.ToInt32(dgvGrupoCargados.Rows[0].Cells[1].Value);
                b = Convert.ToInt32(dgvGrupoCargados.Rows[1].Cells[1].Value);
                ab = Convert.ToInt32(dgvGrupoCargados.Rows[2].Cells[1].Value);
                o = Convert.ToInt32(dgvGrupoCargados.Rows[3].Cells[1].Value);
            }
            else
            {
                //Sino las inicializamos en 0 y empezamos a contar.
                a = 0;
                b = 0;
                ab = 0;
                o= 0;
                dgvGrupoCargados.Rows[0].Cells[1].Value = a;
                dgvGrupoCargados.Rows[1].Cells[1].Value = b;
                dgvGrupoCargados.Rows[2].Cells[1].Value = ab;
                dgvGrupoCargados.Rows[3].Cells[1].Value = o;

            }

        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            //Limpieza de error provider
            epvValidar.Clear();

            //Validacion de llenado de campos
            if (Validar())
            {

            }
            else
            {
                //Seteamos los atributos del donante
                donante.Nombre = txtNombre.Text;
                donante.Dni = Convert.ToInt32(txtDni.Text);
                donante.Edad = Convert.ToInt32(txtEdad.Text);
                donante.GrupoSanguineo = cmbGrupoSanguineo.Text;

                //cargamos el donante al datatable
                dtDonantes.Rows.Add(new object[] { donante.Nombre, donante.Edad, donante.Dni, donante.GrupoSanguineo });
                dtDonantes.WriteXml(DIRECCION_XML + "donante.xml");

                contar(ref a, ref b, ref ab, ref o, donante.GrupoSanguineo);

                dgvGrupoCargados.Rows[0].Cells[1].Value = Convert.ToString(a);
                dgvGrupoCargados.Rows[1].Cells[1].Value = Convert.ToString(b);
                dgvGrupoCargados.Rows[2].Cells[1].Value = Convert.ToString(ab);
                dgvGrupoCargados.Rows[3].Cells[1].Value = Convert.ToString(o);

                dtGrupoSanguineo.WriteXml(DIRECCION_XML + "cargados.xml");

                //Limpieza de txt y cmb
                txtNombre.Clear();
                txtEdad.Clear();
                txtDni.Clear();
                cmbGrupoSanguineo.SelectedIndex = -1;
            }
        }
        //Metodo para Buscar un Donante
        public int BuscarDonante(string dni)
        {
            int fila = -1;

            for (int i = 0; i < dtDonantes.Rows.Count; i++)
            {
                if (dtDonantes.Rows[i]["DNI"].ToString() == dni)
                {
                    fila = i;
                    break;
                }
            }

            return fila;
        }
        //Metodo para leer data table Donantes
        private void Leer_DT()
        {
            if (System.IO.File.Exists(DIRECCION_XML + "donante.xml"))
            {
                dtDonantes.ReadXml(DIRECCION_XML + "donante.xml");
            }
        }
        //Metodo para leer data table GruposCargados
        public void Leer_dtGrupos()
        {
            //Si existen datos los leemos.
            if (System.IO.File.Exists(DIRECCION_XML + "cargados.xml"))
            {
                dtGrupoSanguineo.ReadXml(DIRECCION_XML + "cargados.xml");
            }
            else
            {
                //Sino lo añadimos.
                dtGrupoSanguineo.Rows.Add("A");
                dtGrupoSanguineo.Rows.Add("B");
                dtGrupoSanguineo.Rows.Add("AB");
                dtGrupoSanguineo.Rows.Add("O");

            }
        }

        public void contar(ref int a, ref int b, ref int ab, ref int o, string grupoSanguineo)
        {
            if (grupoSanguineo == "A+" || grupoSanguineo == "A-")
            {
                a++;
            }
            else if (grupoSanguineo == "B+" || grupoSanguineo == "B-")
            {
                b++;
            }
            else if (grupoSanguineo == "AB+" || grupoSanguineo == "AB-")
            {
                ab++;
            }
            else if (grupoSanguineo == "O+" || grupoSanguineo == "O-")
            {
                o++;
            }
        }

        public void descontar(ref int a, ref int b, ref int ab, ref int o, string grupoSanguineo)
        {
            if (grupoSanguineo == "A+" || grupoSanguineo == "A-")
            {
                a--;
            }
            else if (grupoSanguineo == "B+" || grupoSanguineo == "B-")
            {
                b--;
            }
            else if (grupoSanguineo == "AB+" || grupoSanguineo == "AB-")
            {
                ab--;
            }
            else if (grupoSanguineo == "O+" || grupoSanguineo == "O-")
            {
                o--;
            }
        }

        private bool Validar()
        {
            bool validar = false;
            if (txtNombre.Text == "")
            {
                epvValidar.SetError(txtNombre, "Llenar campo");
                validar = true;
            }
            if (txtEdad.Text == "")
            {
                epvValidar.SetError(txtEdad, "Llenar campo");
                validar = true;
            }
            if (txtDni.Text == "")
            {
                epvValidar.SetError(txtDni, "Llenar campo");
                validar = true;
            }
            if (cmbGrupoSanguineo.SelectedIndex == -1)
            {
                epvValidar.SetError(cmbGrupoSanguineo, "Llenar campo");
                validar = true;
            }
            return validar;
        }

        private void txtFiltroDni_TextChanged(object sender, EventArgs e)
        {
            dtDonantes.DefaultView.RowFilter = $"DNI LIKE '{txtFiltroDni.Text}%'";
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvDonantes.CurrentRow == null)
            {
                MessageBox.Show("Donante no seleccionado");
            }
            else
            {
                int fila = BuscarDonante(dgvDonantes.CurrentRow.Cells[2].Value.ToString());
                if (fila != -1)
                {
                    dtDonantes.Rows[fila].Delete();
                    dtDonantes.WriteXml(DIRECCION_XML + "donante.xml");

                    descontar(ref a, ref b, ref ab, ref o, donante.GrupoSanguineo);

                    dgvGrupoCargados.Rows[0].Cells[1].Value = Convert.ToString(a);
                    dgvGrupoCargados.Rows[1].Cells[1].Value = Convert.ToString(b);
                    dgvGrupoCargados.Rows[2].Cells[1].Value = Convert.ToString(ab);
                    dgvGrupoCargados.Rows[3].Cells[1].Value = Convert.ToString(o);

                    dtGrupoSanguineo.WriteXml(DIRECCION_XML + "cargados.xml");

                }
            }
        }

        private void txtDni_KeyPress(object sender, KeyPressEventArgs e)

        {
            if (!char.IsNumber(e.KeyChar) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtEdad_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtFiltroDni_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

    }
}

