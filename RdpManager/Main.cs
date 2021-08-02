using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace RdpManager
{
    public partial class Main : Form
    {
        public static readonly string DataPath = AppDomain.CurrentDomain.BaseDirectory + "\\data";

        public Main()
        {
            InitializeComponent();

            if (!Directory.Exists(DataPath))
                Directory.CreateDirectory(DataPath);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshTree();
        }

        private void RefreshTree()
        {
            tree.Nodes.Clear();
            var servers = Directory.GetDirectories(DataPath);
            foreach (var server in servers)
            {
                var serverName = Path.GetFileName(server);
                var parent = tree.Nodes.Add(serverName, serverName, 0);
                var rdps = Directory.GetFiles(server, "*.rdp");
                foreach (var rdp in rdps)
                {
                    var name = Path.GetFileNameWithoutExtension(rdp);
                    parent.Tag = serverName;
                    parent.Nodes.Add(name, name, 1);
                }
            }

            tree.ExpandAll();
        }

        private static string BetweenStrings(string text, string start, string end)
        {
            var p1 = text.IndexOf(start, StringComparison.Ordinal) + start.Length;
            var p2 = text.IndexOf(end, p1, StringComparison.Ordinal);

            return end == "" ? text.Substring(p1) : text.Substring(p1, p2 - p1);
        }


        private void tree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tree.SelectedNode.Parent?.Tag is string server)
            {
                var path = DataPath + "\\" + server + "\\" + tree.SelectedNode.Text + ".rdp";
                var str = "\"" + path + "\"";
                RdpHandler.Mstsc("mstsc " + str);

                var link = new JumpListLink(path, Path.GetFileNameWithoutExtension(path));

                link.IconReference = new IconReference(path, 0);
                jumpList.AddUserTasks(link);
                jumpList.Refresh();
            }
        }
        JumpList jumpList;

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Add")
            {
                var rdpItem = new RdpItem();
                if (rdpItem.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshTree();
                }
            }

            if (e.ClickedItem.Text == "Remove")
            {
                if (tree.SelectedNode.Parent?.Tag is string server)
                {
                    var path = $"{DataPath}\\{server}\\{tree.SelectedNode.Text.Trim()}.rdp";
                    if (MessageBox.Show("Are You Sure?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                        DialogResult.Yes)
                    {
                        File.Delete(path);
                        RefreshTree();
                    }
                }
            }
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            jumpList = JumpList.CreateJumpList();
        }
    }
}