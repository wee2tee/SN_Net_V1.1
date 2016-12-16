using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CC
{
    public partial class XDatagrid : DataGridView
    {
        private bool row_border_redline;
        public bool FocusedRowBorderRedLine {
            get {
                return this.row_border_redline;
            }
            set
            {
                this.row_border_redline = value;
            }
        }

        private bool allow_sort_by_column_header_clicked;
        public bool AllowSortByColumnHeaderClicked
        {
            get
            {
                return this.allow_sort_by_column_header_clicked;
            }
            set
            {
                this.allow_sort_by_column_header_clicked = value;
            }
        }

        private bool fill_empty_row = false;
        public bool FillEmptyRow
        {
            get
            {
                return this.fill_empty_row;
            }
            set
            {
                this.fill_empty_row = value;
            }
        }

        private int sort_column = -1;
        public int SortColumn
        {
            get
            {
                return this.sort_column;
            }
        }

        private bool sort_asc = true;
        public bool SortASC
        {
            get
            {
                return this.sort_asc;
            }
        }

        public XDatagrid()
        {
            InitializeComponent();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToOrderColumns = false;
            this.AllowUserToResizeColumns = false;
            this.AllowUserToResizeRows = false;

            this.ColumnHeadersDefaultCellStyle.BackColor = Color.PeachPuff;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ColumnHeadersHeight = 28;

            this.RowTemplate.Height = 25;

            if (this.row_border_redline)
            {
                this.DefaultCellStyle.BackColor = Color.White;
                this.DefaultCellStyle.ForeColor = Color.Black;
                this.DefaultCellStyle.SelectionBackColor = Color.White;
                this.DefaultCellStyle.SelectionForeColor = Color.Black;
            }
            using (Font tahoma = new Font("tahoma", 9.75f))
            {
                this.DefaultCellStyle.Font = tahoma;
            }

            this.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.EnableHeadersVisualStyles = false;

            this.MultiSelect = false;
            this.ReadOnly = true;
            this.RowHeadersVisible = false;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.StandardTab = true;

            this.AttachEventHandler();
        }

        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            base.OnDataBindingComplete(e);
            //DataGridViewRow row = new DataGridViewRow();
            //foreach (DataGridViewColumn col in this.Columns)
            //{
            //    row.Cells[col.Index].Value = null;
            //}

            //this.Rows.Add(row);
        }

        private void AttachEventHandler()
        {
            //if (this.row_border_redline)
            //{
                this.Paint += new PaintEventHandler(this.DgvPainted);
            //}
            if (this.allow_sort_by_column_header_clicked)
            {
                this.CellPainting += new DataGridViewCellPaintingEventHandler(this.DrawColumnHeaderSortSign);
            }
        }

        private void DrawColumnHeaderSortSign(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
            
            if (e.RowIndex == -1 && e.ColumnIndex == this.SortColumn)
            {
                using (SolidBrush brush = new SolidBrush(Color.Sienna))
                {
                    if (!this.sort_asc)
                    {
                        /* แสดงแบบ Z->A */
                        using (Font font = new Font("tahoma", 5f))
                        {
                            e.Graphics.DrawString("Z", font, brush, new Point(e.CellBounds.X + e.CellBounds.Width - 16, e.CellBounds.Y + 6));
                            e.Graphics.DrawString("A", font, brush, new Point(e.CellBounds.X + e.CellBounds.Width - 16, e.CellBounds.Y + 13));
                        }
                        using (Pen pen = new Pen(Color.Sienna))
                        {
                            e.Graphics.DrawLine(pen, new Point(e.CellBounds.X + e.CellBounds.Width - 7, e.CellBounds.Y + 9), new Point(e.CellBounds.X + e.CellBounds.Width - 7, e.CellBounds.Y + 19));
                        }
                        e.Graphics.FillPolygon(brush, new Point[] { new Point(e.CellBounds.X + e.CellBounds.Width - 9, e.CellBounds.Y + 17), new Point(e.CellBounds.X + e.CellBounds.Width - 4, e.CellBounds.Y + 17), new Point(e.CellBounds.X + e.CellBounds.Width - 7, e.CellBounds.Y + 19) });
                        
                        /* แสดงเป็นรูปสามเหลี่ยม */
                        //e.Graphics.FillPolygon(brush, new Point[] { new Point(e.CellBounds.X + e.CellBounds.Width - 5, e.CellBounds.Y + 10), new Point(e.CellBounds.X + e.CellBounds.Width - 12, e.CellBounds.Y + 10), new Point(e.CellBounds.X + e.CellBounds.Width - 9, e.CellBounds.Y + 16) });
                    }
                    else
                    {
                        /* แสดงแบบ A->Z */
                        using (Font font = new Font("tahoma", 5f))
                        {
                            e.Graphics.DrawString("A", font, brush, new Point(e.CellBounds.X + e.CellBounds.Width - 16, e.CellBounds.Y + 6));
                            e.Graphics.DrawString("Z", font, brush, new Point(e.CellBounds.X + e.CellBounds.Width - 16, e.CellBounds.Y + 13));
                        }
                        using (Pen pen = new Pen(Color.Sienna))
                        {
                            e.Graphics.DrawLine(pen, new Point(e.CellBounds.X + e.CellBounds.Width - 7, e.CellBounds.Y + 9), new Point(e.CellBounds.X + e.CellBounds.Width - 7, e.CellBounds.Y + 19));
                        }
                        e.Graphics.FillPolygon(brush, new Point[] { new Point(e.CellBounds.X + e.CellBounds.Width - 9, e.CellBounds.Y + 17), new Point(e.CellBounds.X + e.CellBounds.Width - 4, e.CellBounds.Y + 17), new Point(e.CellBounds.X + e.CellBounds.Width - 7, e.CellBounds.Y + 19) });

                        /* แสดงเป็นรูปสามเหลี่ยม */
                        //e.Graphics.FillPolygon(brush, new Point[] { new Point(e.CellBounds.X + e.CellBounds.Width - 5, e.CellBounds.Y + 16), new Point(e.CellBounds.X + e.CellBounds.Width - 12, e.CellBounds.Y + 16), new Point(e.CellBounds.X + e.CellBounds.Width - 9, e.CellBounds.Y + 10) });
                    }
                }
                e.Handled = true;
            }
        }

        private void DgvPainted(object sender, PaintEventArgs e)
        {
            if (!this.row_border_redline)
                return;

            if (this.CurrentCell == null)
                return;

            Rectangle rect = this.GetRowDisplayRectangle(this.CurrentCell.RowIndex, true);

            using (Pen p = new Pen(Color.Red))
            {
                e.Graphics.DrawLine(p, new Point(rect.X, rect.Y), new Point(rect.X + rect.Width, rect.Y));
                e.Graphics.DrawLine(p, new Point(rect.X, rect.Y + rect.Height - 2), new Point(rect.X + rect.Width, rect.Y + rect.Height - 2));
            }
        }

        /* 
         * Sort DataGridViewRow by column header clicked (Specify an object type that implement in bindingsource)
         */
        public void SortByColumn<T>(int column_index)
        {
            if (this.allow_sort_by_column_header_clicked && this.Columns[column_index].SortMode != DataGridViewColumnSortMode.NotSortable)
            {
                this.SetSortColumnInfo(column_index);

                string field_name = this.Columns[column_index].DataPropertyName;

                List<T> data = ((BindingSource)this.DataSource).DataSource as List<T>;
                if (this.SortASC)
                {
                    data = data.OrderBy(d => d.GetType().GetProperty(field_name).GetValue(d, null)).ToList();
                }
                else
                {
                    data = data.OrderByDescending(d => d.GetType().GetProperty(field_name).GetValue(d, null)).ToList();
                }
                ((BindingSource)this.DataSource).ResetBindings(true);
                ((BindingSource)this.DataSource).DataSource = data;
            }
        }

        public void SetSortColumnInfo(int sort_column)
        {
            if(this.SortColumn == sort_column)
            {
                this.sort_asc = !this.SortASC;
            }
            else
            {
                this.sort_column = sort_column;
                this.sort_asc = true;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
