using System;
using System.IO;
using System.Windows.Forms;

namespace RdpManager
{
    public partial class RdpItem : Form
    {
        public RdpItem()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var str = RdpHandler.Process(new LogInfo
            {
                Domain = textBox_Domain.Text,
                IpAddress = textBox_server.Text + ":" + textBox_port.Text,
                Password = textBox_password.Text,
                Username = textBox_username.Text
            });

            var path = Main.DataPath + "//" + textBox_server.Text + "//" + textBox_username.Text + ".rdp";
            File.WriteAllText(path, str);
            DialogResult = DialogResult.OK;
        }
    }
}
