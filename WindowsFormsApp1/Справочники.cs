using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Справочники : Form
    {
        public Справочники()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно открыть ссылку, по которой щелкнули.");
            }
        }

        private void VisitLink()
        {
            // Change the color of the link text by setting LinkVisited
            // to true.
            linkLabel1.LinkVisited = true;
            //Call the Process.Start method to open the default browser
            //with a URL:
            System.Diagnostics.Process.Start("http://www.consultant.ru/document/cons_doc_LAW_140174/");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink3();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно открыть ссылку, по которой щелкнули.");
            }
        }

        private void VisitLink3()
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://docs.cntd.ru/document/499032387");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink4();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно открыть ссылку, по которой щелкнули.");
            }
        }

        private void VisitLink4()
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://fgos.ru/");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink5();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно открыть ссылку, по которой щелкнули.");
            }
        }

        private void VisitLink5()
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://docs.cntd.ru/document/556185964");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink2();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно открыть ссылку, по которой щелкнули.");
            }
        }

        private void VisitLink2()
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://novosibirsk.hh.ru/employer");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink6();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно открыть ссылку, по которой щелкнули.");
            }
        }

        private void VisitLink6()
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://nsk.zarplata.ru/");
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink8();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно открыть ссылку, по которой щелкнули.");
            }
        }

        private void VisitLink8()
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://www.avito.ru/novosibirsk/vakansii");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink7();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно открыть ссылку, по которой щелкнули.");
            }
        }

        private void VisitLink7()
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://nsk.rabota.ru/vacancy");
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink7();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невозможно открыть ссылку, по которой щелкнули.");
            }
        }

        private void VisitLink9()
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.gczn.nsk.su/market/");
        }
    }
}
