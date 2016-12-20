using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SN_Net.Models;
using SN_Net.MiscClass;
using CC;

namespace SN_Net.Subform
{
    public partial class IstabListDialog : Form
    {
        private Point point_to_screen;
        private string tabtyp;
        private List<istabVM> istabs;
        private BindingSource bs;
        private string initial_typcod; // focus on this typcod for first shown
        public istab selected_istab;

        public IstabListDialog(string tabtyp)
        {
            InitializeComponent();
            this.tabtyp = tabtyp;
        }

        public IstabListDialog(Point point_to_screen, string tabtyp, string initial_typcod = "")
            : this(tabtyp)
        {
            this.point_to_screen = point_to_screen;
            this.initial_typcod = initial_typcod;
        }

        private void IstabListDialog_Load(object sender, EventArgs e)
        {
            this.SetBounds(this.point_to_screen.X, this.point_to_screen.Y, this.Width, this.Height);
            this.bs = new BindingSource();
            this.bs.DataSource = this.istabs;
            this.dgv.DataSource = this.bs;

            using (snEntities db = DBX.DataSet())
            {
                this.istabs = db.istab.Where(i => i.tabtyp == this.tabtyp).ToList().ToViewModel();
                this.bs.ResetBindings(true);
                this.bs.DataSource = this.istabs;
            }

            istabVM istab = this.istabs.Where(i => i.typcod == this.initial_typcod).FirstOrDefault();
            if (istab != null)
                this.dgv.Rows.Cast<DataGridViewRow>().Where(r => (string)r.Cells["col_typcod"].Value == this.initial_typcod).First().Cells["col_typcod"].Selected = true;
        }

        private void dgv_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hinfo = ((XDatagrid)sender).HitTest(e.X, e.Y);
            int row_index = hinfo.RowIndex;
            int col_index = hinfo.ColumnIndex;

            if(row_index == -1 && ((XDatagrid)sender).Columns[col_index].Visible)
            {
                ((XDatagrid)sender).SortByColumn<istabVM>(col_index);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Escape)
            {
                this.btnCancel.PerformClick();
                return true;
            }

            if(keyData == Keys.Enter)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dgv_CurrentCellChanged(object sender, EventArgs e)
        {
            if (((XDatagrid)sender).CurrentCell == null)
                return;

            this.btnOK.Enabled = true;
            this.selected_istab = (istab)((XDatagrid)sender).Rows[((XDatagrid)sender).CurrentCell.RowIndex].Cells["col_istab"].Value;
        }
    }
}
