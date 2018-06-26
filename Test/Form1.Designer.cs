using XPTable.Models;

namespace xptable_test
{
	partial class Form1
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
			XPTable.Models.DataSourceColumnBinder dataSourceColumnBinder1 = new XPTable.Models.DataSourceColumnBinder();
			XPTable.Renderers.DragDropRenderer dragDropRenderer1 = new XPTable.Renderers.DragDropRenderer();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.xp_table = new XPTable.Models.Table();
			this.column_model = new XPTable.Models.ColumnModel();
			this.groupColumn1 = new XPTable.Models.GroupColumn();
			this.textColumn1 = new XPTable.Models.TextColumn();
			this.textColumn2 = new XPTable.Models.TextColumn();
			this.table_model = new XPTable.Models.TableModel();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.xp_table)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.addToolStripMenuItem,
            this.removeToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(713, 29);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(46, 25);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(104, 26);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// addToolStripMenuItem
			// 
			this.addToolStripMenuItem.Name = "addToolStripMenuItem";
			this.addToolStripMenuItem.Size = new System.Drawing.Size(50, 25);
			this.addToolStripMenuItem.Text = "Add";
			this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(79, 25);
			this.removeToolStripMenuItem.Text = "Remove";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.label1.Location = new System.Drawing.Point(0, 450);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "label1";
			// 
			// xp_table
			// 
			this.xp_table.AllowDrop = true;
			this.xp_table.AllowRMBSelection = true;
			this.xp_table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.xp_table.BorderColor = System.Drawing.Color.Black;
			this.xp_table.ColumnModel = this.column_model;
			this.xp_table.DataMember = null;
			this.xp_table.DataSourceColumnBinder = dataSourceColumnBinder1;
			dragDropRenderer1.ForeColor = System.Drawing.Color.Red;
			this.xp_table.DragDropRenderer = dragDropRenderer1;
			this.xp_table.EnableToolTips = true;
			this.xp_table.FullRowSelect = true;
			this.xp_table.GridLinesContrainedToData = false;
			this.xp_table.Location = new System.Drawing.Point(12, 32);
			this.xp_table.Name = "xp_table";
			this.xp_table.Size = new System.Drawing.Size(689, 310);
			this.xp_table.TabIndex = 0;
			this.xp_table.TableModel = this.table_model;
			this.xp_table.UnfocusedBorderColor = System.Drawing.Color.Black;
			this.xp_table.DragEnterExternalTypeEvent += new XPTable.Models.DragEnterExternalTypeEventHandler(this.xp_table_DragEnterExternalTypeEvent);
			this.xp_table.DragOverExternalTypeEvent += new XPTable.Models.DragOverExternalTypeEventHandler(this.xp_table_DragOverExternalTypeEvent);
			this.xp_table.DragDropExternalTypeEvent += new XPTable.Models.DragDropExternalTypeEventHandler(this.xp_table_DragDropExternalTypeEvent);
			this.xp_table.DragLeaveExternalTypeEvent += new XPTable.Models.DragLeaveExternalTypeEventHandler(this.xp_table_DragLeaveExternalTypeEvent);
			this.xp_table.DragDropRowInsertedAtEvent += new XPTable.Models.DragDropRowInsertedAtEventHandler(this.xp_table_DragDropRowInsertedAtEvent);
			this.xp_table.DragDropRowMovedEvent += new XPTable.Models.DragDropRowMovedEventHandler(this.xp_table_DragDropRowMovedEvent);
			this.xp_table.DragDropRowGetAllowedDropEffectsEvent += new XPTable.Models.DragDropRowGetAllowedDropEffectsHandler(this.xp_table_DragDropCanMoveRowEvent);
			// 
			// column_model
			// 
			this.column_model.Columns.AddRange(new XPTable.Models.Column[] {
            this.groupColumn1,
            this.textColumn1,
            this.textColumn2});
			// 
			// groupColumn1
			// 
			this.groupColumn1.DrawText = false;
			this.groupColumn1.IsTextTrimmed = false;
			this.groupColumn1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(218)))), ((int)(((byte)(249)))));
			this.groupColumn1.Resizable = false;
			this.groupColumn1.Selectable = false;
			this.groupColumn1.Sortable = false;
			this.groupColumn1.ToggleOnSingleClick = true;
			this.groupColumn1.Width = 22;
			// 
			// textColumn1
			// 
			this.textColumn1.IsTextTrimmed = false;
			this.textColumn1.Text = "Id";
			// 
			// textColumn2
			// 
			this.textColumn2.IsTextTrimmed = false;
			this.textColumn2.Text = "Text";
			// 
			// table_model
			// 
			this.table_model.RowHeight = 22;
			// 
			// Form1
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(713, 463);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.xp_table);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
			this.DragOver += new System.Windows.Forms.DragEventHandler(this.Form1_DragOver);
			this.DragLeave += new System.EventHandler(this.Form1_DragLeave);
			this.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.Form1_GiveFeedback);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.xp_table)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Table xp_table;
		private ColumnModel column_model;
		private TableModel table_model;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private GroupColumn groupColumn1;
		private TextColumn textColumn1;
		private TextColumn textColumn2;
		private System.Windows.Forms.Label label1;
	}
}

