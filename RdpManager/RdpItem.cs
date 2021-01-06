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
            var path0 = Main.DataPath + "//" + textBox_server.Text;
            if (!Directory.Exists(path0))
                Directory.CreateDirectory(path0);

            var str = RdpHandler.Process(new LogInfo
            {
                Domain = textBox_Domain.Text,
                IpAddress = string.IsNullOrWhiteSpace(textBox_port.Text)
                    ? textBox_server.Text + ":" + textBox_port.Text
                    : textBox_server.Text,
                Password = textBox_password.Text,
                Username = textBox_username.Text,
                MultiMonitor = checkBox_multimon.Checked ? 1 : 0
            });

            var path = path0 + "//" + textBox_name.Text + ".rdp";
            File.WriteAllText(path, str);
            DialogResult = DialogResult.OK;
        }
    }
}