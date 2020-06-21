using System;

namespace DotaLightApp
{
    partial class Form2
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
            this.infoText = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.Header = new System.Windows.Forms.Label();
            this.HealthBox = new System.Windows.Forms.CheckBox();
            this.BountyBox = new System.Windows.Forms.CheckBox();
            this.BuntyColorLight = new System.Windows.Forms.Button();
            this.GroupRooms = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DeathBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // infoText
            // 
            this.infoText.AutoSize = true;
            this.infoText.Location = new System.Drawing.Point(12, 108);
            this.infoText.Name = "infoText";
            this.infoText.Size = new System.Drawing.Size(224, 17);
            this.infoText.TabIndex = 1;
            this.infoText.Text = "Select groups the then press Start";
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(288, 38);
            this.StartButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(131, 34);
            this.StartButton.TabIndex = 2;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // Header
            // 
            this.Header.AutoSize = true;
            this.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Header.Location = new System.Drawing.Point(268, 6);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(165, 31);
            this.Header.TabIndex = 3;
            this.Header.Text = "Dota 2 lights";
            // 
            // HealthBox
            // 
            this.HealthBox.AutoSize = true;
            this.HealthBox.Checked = true;
            this.HealthBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HealthBox.Location = new System.Drawing.Point(26, 257);
            this.HealthBox.Name = "HealthBox";
            this.HealthBox.Size = new System.Drawing.Size(137, 21);
            this.HealthBox.TabIndex = 5;
            this.HealthBox.Text = "Display Health %";
            this.HealthBox.UseVisualStyleBackColor = true;
            // 
            // BountyBox
            // 
            this.BountyBox.AutoSize = true;
            this.BountyBox.Checked = true;
            this.BountyBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BountyBox.Location = new System.Drawing.Point(26, 311);
            this.BountyBox.Name = "BountyBox";
            this.BountyBox.Size = new System.Drawing.Size(145, 21);
            this.BountyBox.TabIndex = 6;
            this.BountyBox.Text = "Bounty Rune Alert";
            this.BountyBox.UseVisualStyleBackColor = true;
            // 
            // BuntyColorLight
            // 
            this.BuntyColorLight.BackColor = System.Drawing.Color.White;
            this.BuntyColorLight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.BuntyColorLight.Location = new System.Drawing.Point(171, 309);
            this.BuntyColorLight.Name = "BuntyColorLight";
            this.BuntyColorLight.Size = new System.Drawing.Size(90, 23);
            this.BuntyColorLight.TabIndex = 7;
            this.BuntyColorLight.Text = "Alert Color";
            this.BuntyColorLight.UseVisualStyleBackColor = false;
            this.BuntyColorLight.Click += new System.EventHandler(this.BuntyColorLight_Click);
            // 
            // GroupRooms
            // 
            this.GroupRooms.FormattingEnabled = true;
            this.GroupRooms.Location = new System.Drawing.Point(360, 111);
            this.GroupRooms.Name = "GroupRooms";
            this.GroupRooms.Size = new System.Drawing.Size(336, 208);
            this.GroupRooms.TabIndex = 9;
            this.GroupRooms.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.GroupRooms_ItemCheck);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(357, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Avilable groups";
            // 
            // DeathBox
            // 
            this.DeathBox.AutoSize = true;
            this.DeathBox.Checked = true;
            this.DeathBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DeathBox.Location = new System.Drawing.Point(26, 284);
            this.DeathBox.Name = "DeathBox";
            this.DeathBox.Size = new System.Drawing.Size(148, 21);
            this.DeathBox.TabIndex = 11;
            this.DeathBox.Text = "Turn Off On Death";
            this.DeathBox.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(711, 359);
            this.Controls.Add(this.DeathBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GroupRooms);
            this.Controls.Add(this.BuntyColorLight);
            this.Controls.Add(this.BountyBox);
            this.Controls.Add(this.HealthBox);
            this.Controls.Add(this.Header);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.infoText);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form2";
            this.Text = "Dota 2 lights";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form2_FormClosed);
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label infoText;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label Header;
        private System.Windows.Forms.CheckBox HealthBox;
        private System.Windows.Forms.CheckBox BountyBox;
        private System.Windows.Forms.Button BuntyColorLight;
        private System.Windows.Forms.CheckedListBox GroupRooms;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox DeathBox;
    }
}

