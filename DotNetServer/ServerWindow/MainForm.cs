using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cge.Server;
using Cge.Server.Entities;

namespace Cge.ServerWindow
{
    public partial class MainForm : Form
    {
        private readonly ThreadSafeEventDispatcher _dispatcher;
        private readonly CgeServerRoot _server;

        public MainForm()
        {
            _dispatcher = new ThreadSafeEventDispatcher();

            InitializeComponent();

            _server = new CgeServerRoot();
            _server.EntityManager.EntityAdded += _dispatcher.Create<Entity>(OnEntityAdded);

            _server.StartServer(11000);

        }

        private void OnEntityAdded(Entity entity)
        {
            listBox.Items.Add(entity);
        }

        private void listBox_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = listBox.SelectedItem;
        }

        private void send_command_button_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem is Entity entity)
                new SendCommandDialog(entity).ShowDialog();
        }
    }
}
