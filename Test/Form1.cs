using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using XPTable.Models;

namespace xptable_test
{
	public partial class Form1 : Form
	{
		private int last_idx_ = 0;

		public Form1()
		{
			InitializeComponent();
			//var group_column = new GroupColumn("", 20);
			//group_column.ToggleOnSingleClick = true;
			//column_model.Columns.Add(group_column);
			//column_model.Columns.Add(new TextColumn("Id"));
			//column_model.Columns.Add(new TextColumn("Text"));

			for (int i = 0; i < 10; ++i)
			{
				var row = new Row();
				add_Cells(row, i + 1);
				table_model.Rows.Add(row);
				++last_idx_;
			}
			
			// +group
			var last_row = new Row();
			//add_Cells(last_row, 100);
			last_row.Cells.Add(new Cell());
			var cell = new Cell("archive");
			cell.ColSpan = 2;
			last_row.Cells.Add(cell);

			last_row.Editable = false;
			//last_row.ExpandSubRows = false;//bug: wrong grop +- glyph
			table_model.Rows.Add(last_row);
			for (int i = 0; i < 16; ++i)
			{
				var sub_row = new Row();
				add_Cells(sub_row, 200 + i);
				last_row.SubRows.Add(sub_row);
			}

			for (int i = 10; i < 20; ++i)
			{
				var row = new Row();
				add_Cells(row, i + 1);
				table_model.Rows.Add(row);
				++last_idx_;
			}

		}

		private void add_Cells(Row row, int idx)
		{
			var cells = row.Cells;
			cells.Add(new Cell());
			cells.Add(new Cell("" + idx));
			cells.Add(new Cell("Text " + idx));
			for (int i = 0; i < cells.Count; ++i)
				row.Cells[i].ToolTipText = "hint";
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void addToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var row = new Row();
			add_Cells(row, ++last_idx_);
			table_model.Rows.Add(row);
		}

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Row[] sel_rows = table_model.Selections.SelectedItems;
			if (sel_rows.Length > 0)
			{
				table_model.Rows.RemoveRange(sel_rows);
			}
		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{
			Debug.WriteLine("Form1::DragEnter");
			if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
			{
				if ((e.KeyState & 8) == 0)
					e.Effect = DragDropEffects.Link;
				else
					e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void Form1_DragOver(object sender, DragEventArgs e)
		{
			if (e.Effect != DragDropEffects.None)
			{
				if ((e.KeyState & 8) == 0)
					e.Effect = DragDropEffects.Link;
				else
					e.Effect = DragDropEffects.Copy;
			}
		}

		private void Form1_DragDrop(object sender, DragEventArgs e)
		{
			Debug.WriteLine("Form1::DragDrop");
			if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
			{
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				foreach (string s in files)
				{
					label1.Text = s;
					break;
				}
			}
		}

		private void Form1_DragLeave(object sender, EventArgs e)
		{
			Debug.WriteLine("Form1::DragLeave");
		}

		private void Form1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			//:GiveFeedback event is fired on the drag source, unlike the other events that fire on the drop target
			//:so no luck
			Debug.WriteLine("Form1::GiveFeedback");
		}

		private void xp_table_DragDropRowMovedEvent(Row row, int srcIndex, int destIndex)
		{
			Debug.WriteLine("DragDropRowMovedEvent(" + srcIndex + ", " + destIndex + ")");
		}

		private void xp_table_DragDropRowInsertedAtEvent(Row row, int destIndex)
		{
			Debug.WriteLine("DragDropRowInsertedAtEvent(" + destIndex + ")");
		}


		private void xp_table_DragEnterExternalTypeEvent(object sender, DragEventArgs drgevent)
		{
			Debug.WriteLine("xp_table::DragEnter");
			Form1_DragEnter(sender, drgevent);
		}

		private bool xp_table_DragOverExternalTypeEvent(object sender, DragEventArgs drgevent, Row row, int destIndex)
		{
			//Debug.WriteLine("xp_table::DragOver");
			Form1_DragOver(sender, drgevent);
			return false;
		}

		private void xp_table_DragDropExternalTypeEvent(object sender, DragEventArgs drgevent, Row row, int destIndex)
		{
			Debug.WriteLine("xp_table::Drop to row " + destIndex);
			Form1_DragDrop(sender, drgevent);
		}

		private void xp_table_DragLeaveExternalTypeEvent(object sender, EventArgs drgevent)
		{
			Debug.WriteLine("xp_table::DragLeave");
		}

		private DragDropEffects xp_table_DragDropCanMoveRowEvent(Row row, int rowIndex)
		{
			return row.Editable ? DragDropEffects.Move : DragDropEffects.None;
		}
	}
}
