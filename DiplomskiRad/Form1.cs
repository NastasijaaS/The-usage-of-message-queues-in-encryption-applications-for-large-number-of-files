using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiplomskiRad
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            SidePanel.Height = button1.Height;
            SidePanel.Top = button1.Top;
            panelCont.Controls.Clear();
            FirstPage fp = new FirstPage();
            fp.Dock = DockStyle.Fill;
            panelCont.Controls.Add(fp);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button1.Height;
            SidePanel.Top = button1.Top;

            panelCont.Controls.Clear();
            FirstPage fp = new FirstPage();
            fp.Dock = DockStyle.Fill;
            panelCont.Controls.Add(fp);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button2.Height;
            SidePanel.Top = button2.Top;


            panelCont.Controls.Clear();
            RC6Page rp = new RC6Page();
            rp.Dock = DockStyle.Fill;
            panelCont.Controls.Add(rp);

            Cipher.SetActiveUserControl(rp);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button4.Height;
            SidePanel.Top = button4.Top;

            panelCont.Controls.Clear();
            AESPage ap = new AESPage();
            ap.Dock = DockStyle.Fill;
            panelCont.Controls.Add(ap);

            Cipher.SetActiveUserControl(ap);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SidePanel.Height = button3.Height;
            SidePanel.Top = button3.Top;

            panelCont.Controls.Clear();
            XXTEAPage xp = new XXTEAPage();
            xp.Dock = DockStyle.Fill;
            panelCont.Controls.Add(xp);

            Cipher.SetActiveUserControl(xp);
        }
    }
}
