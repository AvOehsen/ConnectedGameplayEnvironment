using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cge.Server.Commands;
using Cge.Server.Entities;

namespace Cge.ServerWindow
{
    public partial class SendCommandDialog : Form
    {
        private readonly Entity _targetEntity;
        private AbstractCommand _selectedCommand;

        public SendCommandDialog(Entity targetEntity)
        {
            this.Text = "Send Command - " + targetEntity.DeviceId;

            _targetEntity = targetEntity;

            InitializeComponent();

            if (targetEntity != null)
            {
                foreach (var module in targetEntity.Modules)
                {
                    foreach (var commandTyp in module.SupportedCommands)
                    {
                        if (!commands_comboBox.Items.Contains(commandTyp))
                            commands_comboBox.Items.Add(commandTyp);
                    }
                }
            }
        }

        private void commands_comboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid.SelectedObject = null;
            _selectedCommand = null;

            if (commands_comboBox.SelectedItem is Type commandType)
            {
                _selectedCommand = (AbstractCommand)Activator.CreateInstance(commandType);
                propertyGrid.SelectedObject = _selectedCommand;
            }
        }

        private void send_button_Click(object sender, EventArgs e)
        {
            if (_selectedCommand != null)
                _targetEntity.SendCommand(_selectedCommand);
        }
    }
}
