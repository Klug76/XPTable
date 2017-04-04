using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using XPTable.Editors;
using XPTable.Events;
using XPTable.Models;
using XPTable.Renderers;
using XPTable.Sorting;
using XPTable.Themes;
using XPTable.Win32;

/*TODO

. fix scroll
+. fix header resize
. fix focus out

*/

namespace XPTable.Models
{

	class DragDropHelper2
	{
		Table table_;
		IDragDropRenderer renderer_;
		private bool drag_drop_mode_ = false;
		private MouseButtons start_mouse_button_ = MouseButtons.None;
		private Row drag_row_ = null;
		private Row last_hover_ = null;
		private DragDropEffects allowed_effects_ = DragDropEffects.None;
		private Rectangle drag_threshold_bounds_;
		private Form form_;
		private string drag_type_;
		private int last_timer_ = 0;
		private int initial_scroll_delay_ = 300;
		private int min_scroll_delay_ = 100;
		private int scroll_delay_;

		public DragDropHelper2(Table table)
		{
			table_ = table;
			table_.DragEnter += new DragEventHandler(table_DragEnter);
			table_.DragOver += new DragEventHandler(table_DragOver);
			table_.DragDrop += new DragEventHandler(table_DragDrop);
			table_.DragLeave += new EventHandler(table_DragLeave);
			table_.GiveFeedback += new GiveFeedbackEventHandler(table_GiveFeedback);
			drag_type_ = typeof(DragItemData).ToString();
		}

		/// <summary>
		/// Gets or sets the renderer that draws the drag drop hover indicator.
		/// </summary>
		public IDragDropRenderer DragDropRenderer
		{
			get { return renderer_; }
			set { renderer_ = value; }
		}


		#region Drag drop helpers
		private DragItemData GetDataForDragDrop()
		{
			DragItemData data = new DragItemData(table_, drag_row_);
			return data;
		}

		private class DragItemData
		{
			public Table table_;
			public Row row_;

			public DragItemData(Table table, Row row)
			{
				table_ = table;
				row_ = row;
			}
		}
		#endregion

		public void AllowBeginDrag(MouseEventArgs e, int row_idx)
		{
			if (drag_drop_mode_)
				return;
			drag_row_ = null;
			if (row_idx < 0)
				return;
			if (e.Clicks > 1)
				return;
			start_mouse_button_ = e.Button;
			if (start_mouse_button_ == MouseButtons.None)
				return;
			Row row = table_.TableModel.Rows[row_idx];
			allowed_effects_ = table_.DragDropRowGetAllowedDropEffects(row, row_idx);
			if (DragDropEffects.None == allowed_effects_)
				return;
			drag_row_ = row;
			//Debug.WriteLine("*** prepare drag: " + row_idx);
			Size drag_size = SystemInformation.DragSize;
			drag_threshold_bounds_ = new Rectangle(new Point(e.X - drag_size.Width / 2, e.Y - drag_size.Height / 2), drag_size);
			Form parent = table_.FindForm();
			if (parent != null)
			{
				form_ = parent;
				parent.Deactivate += on_Deactivate;
			}
		}

		public void DisallowBeginDrag()
		{
			if (drag_drop_mode_)
				return;
			reset_DragDrop();
			remove_Deactivate_Listener();
		}

		private bool CanBeginDrag(MouseEventArgs e)
		{
			if (null == drag_row_)
				return false;
			if (start_mouse_button_ == MouseButtons.None)
				return false;
			if ((Control.MouseButtons & start_mouse_button_) != start_mouse_button_)
				return false;
			return !drag_threshold_bounds_.Contains(e.X, e.Y);
		}

		public bool DoDrag(MouseEventArgs e)
		{
			if (drag_drop_mode_)
				return true;//:paranoia!?
			if (!CanBeginDrag(e))
				return false;
			prepare_DragDrop();
			table_.DoDragDrop(GetDataForDragDrop(), allowed_effects_);//:modal loop
			finish_DragDrop();
			return true;
		}

		private void prepare_DragDrop()
		{
			//Debug.WriteLine("*** begin drag: " + drag_row_.Index);
			drag_drop_mode_ = true;
			scroll_delay_ = initial_scroll_delay_;
		}

		private void finish_DragDrop()
		{
			//Debug.WriteLine("*** end drag: " + drag_row_.Index);
			reset_DragDrop();
			remove_Deactivate_Listener();
		}

		private void reset_DragDrop()
		{
			drag_drop_mode_ = false;
			start_mouse_button_ = MouseButtons.None;
			drag_row_ = null;
			last_hover_ = null;
		}

		private void remove_Deactivate_Listener()
		{
			if (form_ != null)
			{
				Form parent = form_;
				form_ = null;
				parent.Deactivate -= on_Deactivate;
			}
		}

		private void on_Deactivate(object sender, EventArgs e)
		{
			DisallowBeginDrag();
		}

		void table_DragEnter(object sender, DragEventArgs drgevent)
		{
			if (drgevent.Data.GetDataPresent(drag_type_, false))
			{
				DragDropEffects effect = DragDropEffects.None;
				DragItemData data = (DragItemData)drgevent.Data.GetData(drag_type_, false);
				if ((data != null) && (data.table_ == table_))
				{
					//Debug.WriteLine("*** drag ENTER");
					effect = DragDropEffects.Move;
				}
				drgevent.Effect = effect;
				return;
			}
			table_.DragEnterExternalType(sender, drgevent);
		}

		void table_DragOver(object sender, DragEventArgs drgevent)
		{
			last_timer_ = Environment.TickCount;
			scroll_delay_ = initial_scroll_delay_;
			if (drgevent.Data.GetDataPresent(drag_type_, false))
			{
				DragItemData data = (DragItemData)drgevent.Data.GetData(drag_type_, false);
				if ((data != null) && (data.table_ == table_))
				{
					int hover_idx = find_Row(drgevent);
					//Debug.WriteLine("*** hover idx: " + hover_idx);
					if ((hover_idx >= 0) && (hover_idx < table_.TableModel.Rows.Count))
					{
						table_.EnsureVisible(hover_idx, -1);//:may scroll
						Row hover_row = table_.TableModel.Rows[hover_idx];
						if (hover_row != drag_row_)
						{
							drgevent.Effect = DragDropEffects.Move;
							paint_Drop_Hover(hover_idx, hover_row);
							return;
						}
					}
				}
				clear_Drop_Hover();
				drgevent.Effect = DragDropEffects.None;
				return;
			}
			table_.DragOverExternalType(sender, drgevent);
		}

		private void clear_Drop_Hover()
		{
			if (last_hover_ != null)
			{
				last_hover_ = null;
				table_.Invalidate();
			}
		}

		private void paint_Drop_Hover(int hover_idx, Row hover_row)
		{
			if (null == renderer_)
				return;
			if ((last_hover_ != null) && (hover_row != last_hover_))
				table_.Invalidate();
			last_hover_ = hover_row;
			using (Graphics gr = table_.CreateGraphics())
			{
				renderer_.PaintDragDrop(gr, hover_row, table_.RowRect(hover_idx));
			}
		}

		private int find_Row(DragEventArgs drgevent)
		{
			Point pt = table_.PointToClient(new Point(drgevent.X, drgevent.Y));
			int row = -1;
			if (pt.Y <= table_.HeaderHeight)
			{
				row = table_.TopIndex - 1;
				if (row < 0)
					row = 0;
			}
			else
			{
				row = table_.NearestRowIndexAt(pt.X, pt.Y);
			}
			return row;
		}

		void table_DragDrop(object sender, DragEventArgs drgevent)
		{
			if (drgevent.Data.GetDataPresent(drag_type_, false))
			{
				//Debug.WriteLine("*** drag DROP");
				DragItemData data = (DragItemData)drgevent.Data.GetData(drag_type_, false);
				if ((data != null) && (data.table_ == table_))
				{
					int hover_idx = find_Row(drgevent);
					//Debug.WriteLine("*** drop idx: " + hover_idx);
					if ((hover_idx >= 0) && (hover_idx < table_.TableModel.Rows.Count))
					{
						Row hover_row = table_.TableModel.Rows[hover_idx];
						if (hover_row != drag_row_)
						{
							int src_idx = table_.TableModel.Rows.IndexOf(drag_row_);
							if ((src_idx >= 0) && (src_idx != hover_idx))
							{
								int col = table_.FocusedCell.Column;
								if (col < 0)
									col = 0;
								table_.TableModel.Rows.RemoveAt(src_idx);
								table_.TableModel.Rows.Insert(hover_idx, drag_row_);
								table_.TableModel.Selections.Clear();
								table_.FocusedCell = new CellPos(hover_idx, col);
								table_.TableModel.Selections.SelectCell(table_.FocusedCell);
								table_.DragDropRowMoved(drag_row_, src_idx, hover_idx);
							}
						}
					}
				}
				clear_Drop_Hover();
				return;
			}
			table_.DragDropExternalType(sender, drgevent);
		}

		void table_DragLeave(object sender, EventArgs drgevent)
		{
			if (drag_drop_mode_)
			{
				//Debug.WriteLine("*** drag LEAVE");
				clear_Drop_Hover();
				return;
			}
			table_.DragLeaveExternalType(sender, drgevent);
		}

		private void table_GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			int timer = Environment.TickCount;
			if (timer - last_timer_ < scroll_delay_)
				return;
			last_timer_ = timer;
			if (!table_.Scrollable || !table_.VScroll)
				return;
			scroll_delay_ = (scroll_delay_ * 2) / 3;
			if (scroll_delay_ < min_scroll_delay_)
				scroll_delay_ = min_scroll_delay_;
			Point pt = table_.PointToClient(Control.MousePosition);
			//Debug.WriteLine("table::GiveFeedback, y=" + pt.Y);
			if (pt.Y < 0)
			{
				int idx = table_.TopIndex - 1;
				if (idx < 0)
					idx = 0;
				table_.EnsureVisible(idx, -1);
				return;
			}
			if (pt.Y > table_.Height)
			{
				int idx = table_.TopIndex + table_.GetVisibleRowCount();
				if (idx >= table_.TableModel.Rows.Count)
					idx = table_.TableModel.Rows.Count - 1;
				table_.EnsureVisible(idx, -1);
			}
			/*
			VScrollBar vbar = table_.VerticalScrollBar;
			if (null == vbar)
				return;
			int v = vbar.Value;
			if (pt.Y < 0)
			{
				--v;
				if (v >= vbar.Minimum)
					vbar.Value = v;
				return;
			}
			if (pt.Y > table_.Height)
			{BUG: scroll too high then reset to 0
				++v;
				if (v <= vbar.Maximum)
					vbar.Value = v;
			}
			*/
		}
	}
	#region Delegates

	/// <summary>
	/// Represents the method that will handle DragEnter functionality when the data is an external type.
	/// </summary>
	public delegate void DragEnterExternalTypeEventHandler(object sender, DragEventArgs drgevent);

	/// <summary>
	/// Represents the method that will handle DragOver functionality when the data is an external type.
	/// </summary>
	public delegate void DragOverExternalTypeEventHandler(object sender, DragEventArgs drgevent);

	/// <summary>
	/// Represents the method that will handle Drop functionality when the data is an external type.
	/// </summary>
	public delegate void DragDropExternalTypeEventHandler(object sender, DragEventArgs drgevent);

	/// <summary>
	/// Represents the method that will handle DragLeave functionality when the data is an external type.
	/// </summary>
	public delegate void DragLeaveExternalTypeEventHandler(object sender, EventArgs drgevent);


	/// <summary>
	/// Represents the method that will supply the index of the new row following a
	/// successful DragDrop operation.
	/// </summary>
	public delegate void DragDropRowInsertedAtEventHandler(Row row, int destIndex);

	/// <summary>
	/// Represents the method that will supply the source and destination index 
	/// when a row is moved following a successful DragDrop operation.
	/// </summary>
	public delegate void DragDropRowMovedEventHandler(Row row, int srcIndex, int destIndex);

	/// <summary>
	/// Represents the method that will handle DragLeave functionality when the data is an external type.
	/// </summary>
	public delegate DragDropEffects DragDropRowGetAllowedDropEffectsHandler(Row row, int rowIndex);

	#endregion
}