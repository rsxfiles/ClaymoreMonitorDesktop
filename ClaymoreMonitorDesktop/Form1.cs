using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClaymoreMonitorDesktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            panel4.Show();

            panelLeft.Height = btnDashboard.Height;
            panelLeft.Top = btnDashboard.Top;
            txtIp.Text = "192.168.0.100";
            txtPort.Text = "3333";
            txtIp.Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                    }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            panelLeft.Height = btnDashboard.Height;
            panelLeft.Top = btnDashboard.Top;

            panel4.Show();
        }

        private void btnRates_Click(object sender, EventArgs e)
        {
            panelLeft.Height = btnRates.Height;
            panelLeft.Top = btnRates.Top;

            panel4.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            Rig rig = new Rig(txtIp.Text, (int.Parse(txtPort.Text)));

            try
            {
                rig.GetStatistics1();
                lblCurrHashRate.Text = rig.CurrentHashRates().ToString();
                lblInvalids.Text = rig.Invalids().ToString();
                lblShares.Text = rig.Shares().ToString();
                lblUptime.Text = rig.Uptime();
                lblVersion.Text = rig.Version();
                List<Gpu> list = rig.ListGPUs();

                BindingList<Gpu> bindingList = new BindingList<Gpu>(list);
                BindingSource source = new BindingSource(bindingList, null);
                dgvGpus.DataSource = source;

            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void lblCurrHashRate_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
