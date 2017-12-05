namespace CheckingManager
{
	partial class CheckingManagerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckingManagerForm));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.dataGridViewConnectionCheckingSummary = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxWithPluginCount = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBoxWithoutPluginCount = new System.Windows.Forms.TextBox();
			this.dataGridViewObjectCheckingSummary = new System.Windows.Forms.DataGridView();
			this.btnAddObject = new System.Windows.Forms.Button();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.btnRefreshConnections = new System.Windows.Forms.Button();
			this.btnRefreshBeams = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewConnectionCheckingSummary)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewObjectCheckingSummary)).BeginInit();
			this.tabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::CheckingManager.Properties.Resources.Logo;
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(171, 42);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 15;
			this.pictureBox1.TabStop = false;
			// 
			// dataGridViewConnectionCheckingSummary
			// 
			this.dataGridViewConnectionCheckingSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewConnectionCheckingSummary.Location = new System.Drawing.Point(7, 49);
			this.dataGridViewConnectionCheckingSummary.Name = "dataGridViewConnectionCheckingSummary";
			this.dataGridViewConnectionCheckingSummary.RowTemplate.Height = 24;
			this.dataGridViewConnectionCheckingSummary.Size = new System.Drawing.Size(1329, 449);
			this.dataGridViewConnectionCheckingSummary.TabIndex = 16;
			this.dataGridViewConnectionCheckingSummary.SelectionChanged += new System.EventHandler(this.dataGridViewConnectionCheckingSummary_SelectionChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(204, 17);
			this.label1.TabIndex = 17;
			this.label1.Text = "Connection Checking Summary";
			// 
			// textBoxWithPluginCount
			// 
			this.textBoxWithPluginCount.BackColor = System.Drawing.Color.PaleGreen;
			this.textBoxWithPluginCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxWithPluginCount.Location = new System.Drawing.Point(547, 21);
			this.textBoxWithPluginCount.Name = "textBoxWithPluginCount";
			this.textBoxWithPluginCount.Size = new System.Drawing.Size(49, 22);
			this.textBoxWithPluginCount.TabIndex = 19;
			this.textBoxWithPluginCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(321, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(211, 17);
			this.label2.TabIndex = 20;
			this.label2.Text = "Connections WITH Check Plugin";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(621, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(241, 17);
			this.label3.TabIndex = 22;
			this.label3.Text = "Connections WITHOUT Check Plugin";
			// 
			// textBoxWithoutPluginCount
			// 
			this.textBoxWithoutPluginCount.BackColor = System.Drawing.Color.SeaShell;
			this.textBoxWithoutPluginCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxWithoutPluginCount.Location = new System.Drawing.Point(877, 21);
			this.textBoxWithoutPluginCount.Name = "textBoxWithoutPluginCount";
			this.textBoxWithoutPluginCount.Size = new System.Drawing.Size(49, 22);
			this.textBoxWithoutPluginCount.TabIndex = 21;
			this.textBoxWithoutPluginCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// dataGridViewObjectCheckingSummary
			// 
			this.dataGridViewObjectCheckingSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewObjectCheckingSummary.Location = new System.Drawing.Point(6, 48);
			this.dataGridViewObjectCheckingSummary.Name = "dataGridViewObjectCheckingSummary";
			this.dataGridViewObjectCheckingSummary.RowTemplate.Height = 24;
			this.dataGridViewObjectCheckingSummary.Size = new System.Drawing.Size(1330, 450);
			this.dataGridViewObjectCheckingSummary.TabIndex = 23;
			this.dataGridViewObjectCheckingSummary.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridBeams_RowValidated);
			this.dataGridViewObjectCheckingSummary.SelectionChanged += new System.EventHandler(this.dataGridViewConnectionCheckingSummary_SelectionChanged);
			// 
			// btnAddObject
			// 
			this.btnAddObject.Location = new System.Drawing.Point(6, 6);
			this.btnAddObject.Name = "btnAddObject";
			this.btnAddObject.Size = new System.Drawing.Size(137, 36);
			this.btnAddObject.TabIndex = 24;
			this.btnAddObject.Text = "Add Beam";
			this.btnAddObject.UseVisualStyleBackColor = true;
			this.btnAddObject.Click += new System.EventHandler(this.btnAddBeam_Click);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPage1);
			this.tabControl.Controls.Add(this.tabPage2);
			this.tabControl.Location = new System.Drawing.Point(12, 73);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(1350, 533);
			this.tabControl.TabIndex = 26;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.btnRefreshConnections);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.dataGridViewConnectionCheckingSummary);
			this.tabPage1.Controls.Add(this.textBoxWithPluginCount);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.textBoxWithoutPluginCount);
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1342, 504);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Connections";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.btnRefreshBeams);
			this.tabPage2.Controls.Add(this.btnAddObject);
			this.tabPage2.Controls.Add(this.dataGridViewObjectCheckingSummary);
			this.tabPage2.Location = new System.Drawing.Point(4, 25);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(1342, 504);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Beams";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// btnRefreshConnections
			// 
			this.btnRefreshConnections.Location = new System.Drawing.Point(1198, 7);
			this.btnRefreshConnections.Name = "btnRefreshConnections";
			this.btnRefreshConnections.Size = new System.Drawing.Size(138, 36);
			this.btnRefreshConnections.TabIndex = 23;
			this.btnRefreshConnections.Text = "Refresh";
			this.btnRefreshConnections.UseVisualStyleBackColor = true;
			this.btnRefreshConnections.Click += new System.EventHandler(this.btnRefreshConnections_Click);
			// 
			// btnRefreshBeams
			// 
			this.btnRefreshBeams.Location = new System.Drawing.Point(1198, 6);
			this.btnRefreshBeams.Name = "btnRefreshBeams";
			this.btnRefreshBeams.Size = new System.Drawing.Size(138, 36);
			this.btnRefreshBeams.TabIndex = 26;
			this.btnRefreshBeams.Text = "Refresh";
			this.btnRefreshBeams.UseVisualStyleBackColor = true;
			this.btnRefreshBeams.Click += new System.EventHandler(this.btnRefreshBeams_Click);
			// 
			// CheckingManagerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(1374, 622);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.pictureBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CheckingManagerForm";
			this.Text = "Checking Manager";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewConnectionCheckingSummary)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewObjectCheckingSummary)).EndInit();
			this.tabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.DataGridView dataGridViewConnectionCheckingSummary;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxWithPluginCount;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBoxWithoutPluginCount;
		private System.Windows.Forms.DataGridView dataGridViewObjectCheckingSummary;
		private System.Windows.Forms.Button btnAddObject;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.Button btnRefreshConnections;
		private System.Windows.Forms.Button btnRefreshBeams;
	}
}

