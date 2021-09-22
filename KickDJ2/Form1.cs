using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace KickDJ2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            var InitIni = new IniFile();
            InitIni = new IniFile("config.ini");
            if (!InitIni.KeyExists("ip") && !InitIni.KeyExists("port") && !InitIni.KeyExists("param") || InitIni.Read("ip") == "0" || InitIni.Read("port") == "0" || InitIni.Read("param") == "0")
            {
                InitIni.Write("ip", "0");
                InitIni.Write("port", "0");
                InitIni.Write("param", "0");
                MessageBox.Show("config.ini file settings are not set yet, open the file and enter your ShoutCast web station data.", "Error [config.ini]", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                System.Environment.Exit(1);
            }
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            pictureBox1.BackColor = Color.Navy;
            pictureBox2.BackColor = Color.Navy;
            label3.BackColor = Color.Navy;
            progressBar1.Visible = false;
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 10;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = null;
            progressBar1.Visible = true;
            button1.Enabled = false;
            label2.Text = "Checking .ini file...";
            var InitIni = new IniFile();
            InitIni = new IniFile("config.ini");
            var radio_ip = InitIni.Read("ip");
            var radio_port = InitIni.Read("port");
            var radio_pass = InitIni.Read("param");
            //
            string html = string.Empty;
            decimal sid = sidSel.Value;
            //ShoutCast V1
            //var url = @""+radio_ip+":"+radio_port+"/admin.cgi?sid=&pass="+radio_pass+"&mode=kicksrc";
            //ShoutCast V2
            var url = @"" + radio_ip + ":" + radio_port + "/admin.cgi?sid="+sid+"&pass="+radio_pass+"&mode=kicksrc";
            Console.WriteLine(url);
            label2.Text = "Connecting...";
            try
            {
                // Creates an HttpWebRequest for the specified URL. 
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                // Sends the HttpWebRequest and waits for a response.
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                if (myHttpWebResponse.StatusCode == HttpStatusCode.OK)
                    Console.WriteLine("\r\nResponse Status Code is OK and StatusDescription is: {0}",
                                         myHttpWebResponse.StatusDescription);
                label2.Text = "Source successfully disconnected.";
                label2.ForeColor = Color.Green;
                button1.Enabled = true;
                // Releases the resources of the response.
                myHttpWebResponse.Close();
            }
            catch (WebException ex)
            {
                Console.WriteLine("\r\nWebException Raised. The following error occurred : {0}", ex.Status);
                label2.Text = "Server is not available: "+ ex.Status;
                label2.ForeColor = Color.Red;
                button1.Enabled = true;
                progressBar1.Visible = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nThe following Exception was raised : {0}", ex.Message);
                label2.Text = "Error: " + ex.Message;
                label2.ForeColor = Color.Red;
                button1.Enabled = true;
                progressBar1.Visible = false;
            }
        }
    }
}
