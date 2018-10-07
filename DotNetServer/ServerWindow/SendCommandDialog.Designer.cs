namespace Cge.ServerWindow
{
    partial class SendCommandDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.commands_comboBox = new System.Windows.Forms.ComboBox();
            this.send_button = new System.Windows.Forms.Button();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // commands_comboBox
            // 
            this.commands_comboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.commands_comboBox.FormattingEnabled = true;
            this.commands_comboBox.Location = new System.Drawing.Point(0, 0);
            this.commands_comboBox.Name = "commands_comboBox";
            this.commands_comboBox.Size = new System.Drawing.Size(324, 21);
            this.commands_comboBox.TabIndex = 0;
            this.commands_comboBox.SelectedValueChanged += new System.EventHandler(this.commands_comboBox_SelectedValueChanged);
            // 
            // send_button
            // 
            this.send_button.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.send_button.Location = new System.Drawing.Point(0, 361);
            this.send_button.Name = "send_button";
            this.send_button.Size = new System.Drawing.Size(324, 23);
            this.send_button.TabIndex = 1;
            this.send_button.Text = "SEND";
            this.send_button.UseVisualStyleBackColor = true;
            this.send_button.Click += new System.EventHandler(this.send_button_Click);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 21);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(324, 340);
            this.propertyGrid.TabIndex = 2;
            // 
            // SendCommandDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 384);
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.send_button);
            this.Controls.Add(this.commands_comboBox);
            this.Name = "SendCommandDialog";
            this.Text = "SendCommandDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox commands_comboBox;
        private System.Windows.Forms.Button send_button;
        private System.Windows.Forms.PropertyGrid propertyGrid;
    }
}