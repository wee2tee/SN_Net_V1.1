using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using SN_Net.MiscClass;
using SN_Net.DataModels;
using WebAPI;
using WebAPI.ApiResult;
using Newtonsoft.Json;
using SN_Net.Models;

namespace SN_Net.Subform
{
    public partial class SnWindow : Form
    {
        //private snEntities db;
        public MainForm main_form;
        public GlobalVar G;
        public List<Serial> serial_id_list;
        //public int macloud_exp_prd = 0;


        #region declare Data Model
        public Serial serial;
        public Istab busityp;
        public Istab area;
        public Istab howknown;
        public Istab verext;
        public Dealer dealer;
        public List<Problem> problem;
        public List<Problem> problem_im_only;
        public List<Ma> ma;
        public List<CloudSrv> cloudsrv;

        //private List<Problem> problem_not_found = new List<Problem>();
        #endregion declare Data Model

        #region declare general variable
        private enum FORM_MODE
        {
            PROCESSING,
            SAVING,
            READ,
            ADD,
            EDIT,
            READ_ITEM,
            ADD_ITEM,
            EDIT_ITEM
        }

        private FORM_MODE form_mode;
        //public int id;
        public const string SORT_ID = "id";
        public const string SORT_SN = "sernum";
        public const string SORT_CONTACT = "contact";
        public const string SORT_COMPANY = "compnam";
        public const string SORT_DEALER = "dealer_dealer";
        public const string SORT_OLDNUM = "oldnum";
        public const string SORT_BUSITYP = "busityp";
        public const string SORT_AREA = "area";

        public string sortMode;
        private int find_id;
        private string find_sernum = "";
        private string find_contact = "";
        private string find_company = "";
        private string find_dealer = "";
        private string find_oldnum = "";
        private string find_busityp = "";
        private string find_area = "";
        private FIND_TYPE find_type;
        //private CultureInfo cinfo_us = new CultureInfo("en-US");
        //private CultureInfo cinfo_th = new CultureInfo("th-TH");
        //private List<Label> labels = new List<Label>();
        //private List<Control> list_edit_control;
        public Control current_focused_control;
        private bool is_problem_im_only = false;

        public serial current_serial;

        private enum FIND_TYPE
        {
            SERNUM,
            CONTACT,
            COMPANY,
            DEALER,
            OLDNUM,
            BUSITYP,
            AREA
        }

        #endregion declare general variable

        public SnWindow(MainForm main_form)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");
            InitializeComponent();
            this.main_form = main_form;
        }

        private void SnWindow_Load(object sender, EventArgs e)
        {
            this.form_mode = FORM_MODE.READ;
            this.btnSupportViewNote.Visible = false;
            this.btnSupportNote.Visible = false;

            if (this.main_form.loged_in_user.level < (int)USER_LEVEL.ADMIN)
            {
                this.btnCD.Visible = false;
                this.btnUP.Visible = false;
                this.btnUPNewRwt.Visible = false;
                this.btnUPNewRwtJob.Visible = false;
                this.btnSupportViewNote.Visible = true;
                this.btnSupportNote.Visible = true;
            }
            if (this.main_form.loged_in_user.level == (int)USER_LEVEL.SUPPORT || this.main_form.loged_in_user.level == (int)USER_LEVEL.ACCOUNT)
            {
                this.toolStripImport.Visible = false;
                this.toolStripGenSN.Visible = false;
            }
            //this.main_form = this.MdiParent as MainForm;

            //this.sortMode = SORT_SN;

            /************************************/
            this.current_serial = this.main_form.db.serial.Include("problem_Serial_id").LastOrDefault();
            if (current_serial != null)
                this.FillForm();

            this.ResetControlState();
        }

        public void FillForm()
        {
            this.txtSernum.Texts = this.current_serial.sernum;
        }

        private void ResetControlState()
        {
            /* Tool Strip */
            this.ValidateControlState(this.toolStripAdd, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripEdit, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripDelete, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripStop, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT, FORM_MODE.READ_ITEM, FORM_MODE.ADD_ITEM, FORM_MODE.EDIT_ITEM });
            this.ValidateControlState(this.toolStripSave, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT, FORM_MODE.ADD_ITEM, FORM_MODE.EDIT_ITEM });
            this.ValidateControlState(this.toolStripFirst, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripPrevious, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripNext, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripLast, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripItem, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripImport, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripGenSN, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripUpgrade, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripBook, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripSet2, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripSearch, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripReload, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripInquiryAll, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripInquiryCloud, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripInquiryMA, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripInquiryRest, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripSearchArea, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripSearchBusityp, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripSearchCompany, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripSearchContact, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripSearchDealer, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripSearchOldnum, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.toolStripSearchSN, new FORM_MODE[] { FORM_MODE.READ });

            /* Header */
            this.ValidateControlState(this.txtSernum, new FORM_MODE[] { FORM_MODE.ADD });
            this.ValidateControlState(this.txtVersion, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtArea, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.btnBrowseArea, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtRefnum, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtPrenam, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtCompnam, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.chkIMOnly, new FORM_MODE[] { FORM_MODE.READ, FORM_MODE.READ_ITEM });
            this.ValidateControlState(this.btnLostRenew, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.btnCD, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.btnUP, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.btnUPNewRwt, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.btnUPNewRwtJob, new FORM_MODE[] { FORM_MODE.READ });

            /* Tab Description */
            this.ValidateControlState(this.txtAddr01, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtAddr02, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtAddr03, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtTelnum, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtFaxnum, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtContact, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtPosition, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtOldnum, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.btnPasswordAdd, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.btnPasswordRemove, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.txtRemark, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtBusides, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtBusityp, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtDealer, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtHowknown, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.txtUpfree, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.dtExpdat, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.dtManual, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.dtPurdat, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.dtVerextdat, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.cbVerext, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.btnBrowseBusityp, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.btnBrowseDealer, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.btnBrowseHowknown, new FORM_MODE[] { FORM_MODE.ADD, FORM_MODE.EDIT });
            this.ValidateControlState(this.btnEditMA, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.btnDeleteMA, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.btnEditCloud, new FORM_MODE[] { FORM_MODE.READ });
            this.ValidateControlState(this.btnDeleteCloud, new FORM_MODE[] { FORM_MODE.READ });
        }

        private void ValidateControlState(Component cpn, FORM_MODE[] form_mode_to_enable)
        {
            if(form_mode_to_enable.ToList().Where(fm => fm == this.form_mode).Count() > 0)
            {
                if(cpn is CustomTextBox)
                {
                    ((CustomTextBox)cpn).Read_Only = false; return;
                }
                if (cpn is CustomMaskedTextBox)
                {
                    ((CustomMaskedTextBox)cpn).Read_Only = false; return;
                }
                if (cpn is CustomDateTimePicker)
                {
                    ((CustomDateTimePicker)cpn).Read_Only = false; return;
                }
                if (cpn is CustomComboBox)
                {
                    ((CustomComboBox)cpn).Read_Only = false; return;
                }
                if (cpn is ToolStripButton)
                {
                    ((ToolStripButton)cpn).Enabled = true; return;
                }
                if (cpn is Control)
                {
                    ((Control)cpn).Enabled = true; return;
                }
            }
            else
            {
                if (cpn is CustomTextBox)
                {
                    ((CustomTextBox)cpn).Read_Only = true; return;
                }
                if (cpn is CustomMaskedTextBox)
                {
                    ((CustomMaskedTextBox)cpn).Read_Only = true; return;
                }
                if (cpn is CustomDateTimePicker)
                {
                    ((CustomDateTimePicker)cpn).Read_Only = true; return;
                }
                if (cpn is CustomComboBox)
                {
                    ((CustomComboBox)cpn).Read_Only = true; return;
                }
                if (cpn is ToolStripButton)
                {
                    ((ToolStripButton)cpn).Enabled = false; return;
                }
                if (cpn is Control)
                {
                    ((Control)cpn).Enabled = false; return;
                }
            }
        }

        private void toolStripAdd_Click(object sender, EventArgs e)
        {
            this.form_mode = FORM_MODE.ADD;
            this.ResetControlState();
        }

        private void toolStripEdit_Click(object sender, EventArgs e)
        {

        }

        private void toolStripDelete_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStop_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSave_Click(object sender, EventArgs e)
        {

        }

        private void toolStripFirst_Click(object sender, EventArgs e)
        {

        }

        private void toolStripPrevious_Click(object sender, EventArgs e)
        {

        }

        private void toolStripNext_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLast_Click(object sender, EventArgs e)
        {

        }

        private void toolStripItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripImport_Click(object sender, EventArgs e)
        {

        }

        private void toolStripGenSN_Click(object sender, EventArgs e)
        {

        }

        private void toolStripUpgrade_Click(object sender, EventArgs e)
        {

        }

        private void toolStripBook_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSet2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSearch_ButtonClick(object sender, EventArgs e)
        {

        }

        private void toolStripInquiryAll_Click(object sender, EventArgs e)
        {

        }

        private void toolStripInquiryRest_Click(object sender, EventArgs e)
        {

        }

        private void toolStripInquiryMA_Click(object sender, EventArgs e)
        {

        }

        private void toolStripInquiryCloud_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSearchSN_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSearchContact_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSearchCompany_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSearchDealer_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSearchOldnum_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSearchBusityp_Click(object sender, EventArgs e)
        {

        }

        private void toolStripSearchArea_Click(object sender, EventArgs e)
        {

        }

        private void toolStripReload_Click(object sender, EventArgs e)
        {

        }

        private string GetMachineCode(string probDesc) // Get Machine Code (string) in probdesc
        {
            int dash_1st = 0;
            int dash_2nd = 0;
            int dash_3rd = 0;
            int start = 0;
            int end = probDesc.Length;
            int count = 0;
            int at = 0;
            int loop_cnt = 0;
            while ((start <= end) && (at > -1))
            {
                loop_cnt++;
                if (loop_cnt <= 3)
                {
                    count = end - start;
                    at = probDesc.IndexOf("-", start, count);
                    if (at == -1) break;
                    if (loop_cnt == 1)
                    {
                        dash_1st = at;
                    }
                    else if (loop_cnt == 2)
                    {
                        dash_2nd = at;
                    }
                    else if (loop_cnt == 3)
                    {
                        dash_3rd = at;
                    }

                    start = at + 1;
                }
                else
                {
                    break;
                }
            }
            if (dash_1st == 8)
            {
                if (dash_2nd > 0)
                {
                    // tryparse to int for stage 1 (2 digit after the second dash)
                    int parse1;
                    int parse1_len = (probDesc.Length >= dash_2nd + 3 ? 2 : (probDesc.Length == dash_2nd + 2 ? 1 : 0));
                    bool parse1_result = Int32.TryParse(probDesc.Substring(dash_2nd + 1, parse1_len), out parse1);

                    if (parse1_result == true)
                    {
                        return probDesc.Substring(0, dash_2nd + parse1_len + 1);
                    }
                    else
                    {
                        if (dash_3rd > 0)
                        {
                            // tryparse to int for stage 2 (2 digit after the third dash)
                            int parse2;
                            int parse2_len = (probDesc.Length >= dash_3rd + 3 ? 2 : (probDesc.Length == dash_3rd + 2 ? 1 : 0));
                            bool parse2_result = Int32.TryParse(probDesc.Substring(dash_3rd + 1, parse2_len), out parse2);
                            if (parse2_result == true)
                            {
                                return probDesc.Substring(0, dash_3rd + parse2_len + 1);
                            }
                        }
                    }
                }
            }

            return "";
        }

        private void toolStripAdd_EnabledChanged(object sender, EventArgs e)
        {
            if (((ToolStripButton)sender).Enabled)
            {
                this.btnLostRenew.Enabled = true;
                this.btnCD.Enabled = true;
                this.btnUP.Enabled = true;
                this.btnUPNewRwt.Enabled = true;
                this.btnUPNewRwtJob.Enabled = true;
            }
            else
            {
                this.btnLostRenew.Enabled = false;
                this.btnCD.Enabled = false;
                this.btnUP.Enabled = false;
                this.btnUPNewRwt.Enabled = false;
                this.btnUPNewRwtJob.Enabled = false;
            }
        }

        #region not use
        //private void chkIMOnly_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (((CheckBox)sender).Checked)
        //    {
        //        this.is_problem_im_only = true;
        //        this.fillInDatagrid();
        //    }
        //    else
        //    {
        //        this.is_problem_im_only = false;
        //        this.fillInDatagrid();
        //    }
        //}

        //private void btnLostRenew_Click(object sender, EventArgs e)
        //{
        //    LostRenewForm wind = new LostRenewForm(this);
        //    if (wind.ShowDialog() == DialogResult.OK)
        //    {
        //        this.FormProcessing();
        //        BackgroundWorker workerAfterLostRenew = new BackgroundWorker();
        //        workerAfterLostRenew.DoWork += delegate
        //        {
        //            this.getSerialIDList();
        //            this.getSerial(this.serial.id);
        //        };
        //        workerAfterLostRenew.RunWorkerCompleted += delegate
        //        {
        //            this.fillSerialInForm();
        //            this.FormRead();
        //        };
        //        workerAfterLostRenew.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        this.FormRead();
        //    }
        //}

        //private void toolStripGenSN_Click(object sender, EventArgs e)
        //{
        //    GenerateSNForm wind = new GenerateSNForm(this);
        //    if (wind.ShowDialog() == DialogResult.OK)
        //    {
        //        this.FormProcessing();
        //        BackgroundWorker worker = new BackgroundWorker();
        //        worker.DoWork += delegate
        //        {
        //            this.getSerialIDList();
        //        };
        //        worker.RunWorkerCompleted += delegate
        //        {
        //            this.fillSerialInForm();
        //            this.FormRead();
        //        };
        //        worker.RunWorkerAsync();
        //    }
        //}

        //private void btnCD_Click(object sender, EventArgs e)
        //{
        //    if (MessageAlert.Show("Generate \"CD training date\"", "", MessageAlertButtons.OK_CANCEL, MessageAlertIcons.QUESTION) == DialogResult.OK)
        //    {
        //        this.FormProcessing();
        //        bool post_success = false;

        //        BackgroundWorker workerCD = new BackgroundWorker();
        //        workerCD.DoWork += delegate
        //        {
        //            string json_data = "{\"id\":" + this.serial.id.ToString() + ",";
        //            json_data += "\"users_name\":\"" + this.main_form.G.loged_in_user_name + "\"}";
        //            Console.WriteLine(json_data);
        //            CRUDResult post = ApiActions.POST(PreferenceForm.API_MAIN_URL() + "serial/gen_cd_training_date", json_data);
        //            Console.WriteLine("post.data = " + post.data);
        //            ServerResult sr = JsonConvert.DeserializeObject<ServerResult>(post.data);

        //            if (sr.result == ServerResult.SERVER_RESULT_SUCCESS)
        //            {
        //                this.serial = sr.serial[0];
        //                post_success = true;
        //            }
        //            else
        //            {
        //                MessageAlert.Show(sr.message, "Error", MessageAlertButtons.OK, MessageAlertIcons.ERROR);
        //                post_success = false;
        //            }
        //        };

        //        workerCD.RunWorkerCompleted += delegate
        //        {
        //            if (post_success)
        //            {
        //                this.dtExpdat.TextsMysql = this.serial.expdat;
        //                this.lblExpdat2.pickedDate(this.serial.expdat);
        //                this.FormRead();
        //            }
        //            else
        //            {
        //                this.FormRead();
        //            }
        //        };

        //        workerCD.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        this.FormRead();
        //    }
        //}

        //private void btnUPNewRwt_Click(object sender, EventArgs e)
        //{
        //    UpNewRwtLineForm wind = new UpNewRwtLineForm(this, UpNewRwtLineForm.DIALOG_TYPE.UP_NEWRWT);
        //    if (wind.ShowDialog() == DialogResult.OK)
        //    {
        //        this.fillSerialInForm();
        //        this.FormRead();
        //    }
        //    else
        //    {
        //        this.FormRead();
        //    }
        //}

        //private void btnUPNewRwtJob_Click(object sender, EventArgs e)
        //{
        //    UpNewRwtLineForm wind = new UpNewRwtLineForm(this, UpNewRwtLineForm.DIALOG_TYPE.UP_NEWRWT_JOB);
        //    if (wind.ShowDialog() == DialogResult.OK)
        //    {
        //        this.fillSerialInForm();
        //        this.FormRead();
        //    }
        //    else
        //    {
        //        this.FormRead();
        //    }
        //}

        //private void btnUP_Click(object sender, EventArgs e)
        //{
        //    if (MessageAlert.Show("Generate \"Update\" line", "", MessageAlertButtons.OK_CANCEL, MessageAlertIcons.QUESTION) == DialogResult.OK)
        //    {
        //        bool post_success = false;
        //        this.FormProcessing();

        //        BackgroundWorker workerUp = new BackgroundWorker();
        //        workerUp.DoWork += delegate
        //        {
        //            string json_data = "{\"id\":" + this.serial.id.ToString() + ",";
        //            json_data += "\"users_name\":\"" + this.main_form.G.loged_in_user_name + "\"}";

        //            CRUDResult post = ApiActions.POST(PreferenceForm.API_MAIN_URL() + "problem/gen_up_line", json_data);
        //            ServerResult sr = JsonConvert.DeserializeObject<ServerResult>(post.data);

        //            if (sr.result == ServerResult.SERVER_RESULT_SUCCESS)
        //            {
        //                post_success = true;
        //                this.problem = sr.problem;
        //                this.problem_im_only = (sr.problem.Count > 0 ? sr.problem.Where<Problem>(t => t.probcod == "IM").ToList<Problem>() : new List<Problem>());
        //            }
        //            else
        //            {
        //                MessageAlert.Show(sr.message, "Error", MessageAlertButtons.OK, MessageAlertIcons.ERROR);
        //                post_success = false;
        //            }
        //        };

        //        workerUp.RunWorkerCompleted += delegate
        //        {
        //            if (post_success)
        //            {
        //                this.fillInDatagrid();
        //                this.FormRead();
        //            }
        //            else
        //            {
        //                this.FormRead();
        //            }
        //        };

        //        workerUp.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        this.FormRead();
        //    }
        //}

        //private void toolStripUpgrade_Click(object sender, EventArgs e)
        //{
        //    UpgradeProgramForm wind = new UpgradeProgramForm(this);
        //    if (wind.ShowDialog() == DialogResult.OK)
        //    {
        //        this.txtSernum.Texts = this.serial.sernum;
        //        this.txtVersion.Texts = this.serial.version;
        //        this.dtExpdat.TextsMysql = this.serial.expdat;
        //        this.lblExpdat2.pickedDate(this.serial.expdat);
        //        this.dtVerextdat.TextsMysql = this.serial.verextdat;
        //        this.fillInDatagrid();
        //        this.FormRead();
        //    }
        //    else
        //    {
        //        this.FormRead();
        //    }
        //}

        //private void toolStripBook_Click(object sender, EventArgs e)
        //{
        //    SellBookForm wind = new SellBookForm(this);
        //    if (wind.ShowDialog() == DialogResult.OK)
        //    {
        //        this.fillInDatagrid();
        //    }
        //}

        //private void toolStripSet2_Click(object sender, EventArgs e)
        //{
        //    SellProgram2nd wind = new SellProgram2nd(this);
        //    if (wind.ShowDialog() == DialogResult.OK)
        //    {
        //        this.FormProcessing();
        //        BackgroundWorker worker = new BackgroundWorker();
        //        worker.DoWork += delegate
        //        {
        //            this.getSerialIDList();
        //            this.getSerial(this.serial.id);
        //        };
        //        worker.RunWorkerCompleted += delegate
        //        {
        //            this.fillSerialInForm();
        //            this.FormRead();
        //        };
        //        worker.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        this.FormRead();
        //    }
        //}

        //private void toolStripImport_Click(object sender, EventArgs e)
        //{
        //    ImportListForm wind = new ImportListForm(this);
        //    wind.ShowDialog();
        //}

        //private void btnSupportNote_Click(object sender, EventArgs e)
        //{
        //    this.btnSupportNote.Enabled = false;
        //    if (this.main_form.supportnote_wind == null)
        //    {
        //        SupportNoteWindow wind = new SupportNoteWindow(this, this.serial, GetPasswordList(this.serial.sernum));
        //        wind.Text += " (" + this.main_form.loged_in_user.level.ToUserLevelString() + " : " + this.main_form.G.loged_in_user_name + " - " + this.main_form.G.loged_in_user_realname + ")";
        //        wind.MdiParent = this.main_form;
        //        this.main_form.supportnote_wind = wind;
        //        wind.Show();
        //    }
        //    else
        //    {
        //        this.main_form.supportnote_wind.CrossingCall(this.serial, GetPasswordList(this.serial.sernum));
        //    }
        //}

        //private void btnSupportPause_Click(object sender, EventArgs e)
        //{
        //    this.splitContainer2.SplitterDistance = 143;
        //}

        //private void btnSupportHistory_Click(object sender, EventArgs e)
        //{
        //    if (this.main_form.supportnote_wind == null)
        //    {
        //        SupportNoteWindow wind = new SupportNoteWindow(this);
        //        wind.Text += " (" + this.main_form.loged_in_user.level.ToUserLevelString() + " : " + this.main_form.G.loged_in_user_name + " - " + this.main_form.G.loged_in_user_realname + ")";
        //        wind.MdiParent = this.main_form;
        //        this.main_form.supportnote_wind = wind;
        //        wind.Show();
        //    }
        //    else
        //    {
        //        this.main_form.supportnote_wind.Activate();
        //    }
        //}

        //private void btnPasswordAdd_Click(object sender, EventArgs e)
        //{
        //    SerialPasswordDialog wind = new SerialPasswordDialog(this.main_form, this);
        //    if (wind.ShowDialog() == DialogResult.OK)
        //    {
        //        this.FillDgvPassword(GetPasswordList(this.serial.sernum));
        //        if (this.dgvPassword.Rows.Cast<DataGridViewRow>().Where(r => r.Tag is SerialPassword).Where(r => ((SerialPassword)r.Tag).id == wind.inserted_id).Count<DataGridViewRow>() > 0)
        //        {
        //            this.dgvPassword.CurrentCell = this.dgvPassword.Rows.Cast<DataGridViewRow>().Where(r => r.Tag is SerialPassword).Where(r => ((SerialPassword)r.Tag).id == wind.inserted_id).First<DataGridViewRow>().Cells[1];
        //        }
        //    }
        //}

        //private void btnPasswordRemove_Click(object sender, EventArgs e)
        //{
        //    if (this.dgvPassword.CurrentCell == null)
        //        return;

        //    if (!(this.dgvPassword.Rows[this.dgvPassword.CurrentCell.RowIndex].Tag is SerialPassword))
        //        return;

        //    SerialPassword pwd_to_del = (SerialPassword)this.dgvPassword.Rows[this.dgvPassword.CurrentCell.RowIndex].Tag;

        //    if (MessageAlert.Show("ลบรหัสผ่าน \"" + pwd_to_del.pass_word + "\" นี้ใช่หรือไม่?", "", MessageAlertButtons.OK_CANCEL, MessageAlertIcons.QUESTION) == DialogResult.OK)
        //    {

        //        CRUDResult delete = ApiActions.DELETE(PreferenceForm.API_MAIN_URL() + "serialpassword/delete&id=" + pwd_to_del.id.ToString());
        //        ServerResult sr = JsonConvert.DeserializeObject<ServerResult>(delete.data);

        //        this.FillDgvPassword(GetPasswordList(this.serial.sernum));
        //    }
        //}

        //public static List<SerialPassword> GetPasswordList(string sernum)
        //{
        //    CRUDResult get = ApiActions.GET(PreferenceForm.API_MAIN_URL() + "serialpassword/get_password&sernum=" + sernum);
        //    ServerResult sr = JsonConvert.DeserializeObject<ServerResult>(get.data);

        //    if (sr.result == ServerResult.SERVER_RESULT_SUCCESS)
        //    {
        //        return sr.serial_password;
        //    }
        //    else
        //    {
        //        return new List<SerialPassword>();
        //    }
        //}

        //private void FillDgvPassword(List<SerialPassword> list_password)
        //{
        //    this.dgvPassword.Rows.Clear();
        //    this.dgvPassword.Columns.Clear();
        //    this.dgvPassword.Tag = HelperClass.DGV_TAG.READ;

        //    this.dgvPassword.Columns.Add(new DataGridViewTextBoxColumn()
        //    {
        //        Visible = false
        //    });
        //    this.dgvPassword.Columns.Add(new DataGridViewTextBoxColumn()
        //    {
        //        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        //    });

        //    foreach (SerialPassword sp in list_password)
        //    {
        //        int r = this.dgvPassword.Rows.Add();
        //        this.dgvPassword.Rows[r].Tag = sp;

        //        this.dgvPassword.Rows[r].Cells[0].ValueType = typeof(int);
        //        this.dgvPassword.Rows[r].Cells[0].Value = sp.id;

        //        this.dgvPassword.Rows[r].Cells[1].ValueType = typeof(string);
        //        this.dgvPassword.Rows[r].Cells[1].Value = sp.pass_word;
        //    }
        //}

        //private void btnEditMA_Click(object sender, EventArgs e)
        //{
        //    MAFormDialog ma = new MAFormDialog(this);
        //    if (ma.ShowDialog() == DialogResult.OK)
        //    {
        //        BackgroundWorker worker = new BackgroundWorker();
        //        worker.DoWork += delegate
        //        {
        //            this.getSerial(this.serial.id);
        //        };
        //        worker.RunWorkerCompleted += delegate
        //        {
        //            this.fillSerialInForm();
        //        };
        //        worker.RunWorkerAsync();
        //    }
        //}

        //private void btnDeleteMA_Click(object sender, EventArgs e)
        //{
        //    this.DeleteMA();
        //}

        //private void DeleteMA()
        //{
        //    if (this.ma.Count == 0)
        //        return;

        //    if (MessageAlert.Show("ลบข้อมูลบริการ MA., ทำต่อ?", "", MessageAlertButtons.OK_CANCEL, MessageAlertIcons.QUESTION) == DialogResult.Cancel)
        //        return;


        //    bool delete_success = false;
        //    string err_msg = "";

        //    this.FormProcessing();
        //    BackgroundWorker worker = new BackgroundWorker();
        //    worker.DoWork += delegate
        //    {
        //        CRUDResult delete = ApiActions.DELETE(PreferenceForm.API_MAIN_URL() + "ma/delete&id=" + this.ma[0].id.ToString());
        //        ServerResult sr = JsonConvert.DeserializeObject<ServerResult>(delete.data);

        //        if (sr.result == ServerResult.SERVER_RESULT_SUCCESS)
        //        {
        //            delete_success = true;
        //        }
        //        else
        //        {
        //            delete_success = false;
        //            err_msg = sr.message;
        //        }
        //    };
        //    worker.RunWorkerCompleted += delegate
        //    {
        //        if (delete_success)
        //        {
        //            this.ma.RemoveAll(t => t.id > -1);
        //            this.fillSerialInForm();
        //            this.FormRead();
        //        }
        //        else
        //        {
        //            if (MessageAlert.Show(err_msg, "Error", MessageAlertButtons.RETRY_CANCEL, MessageAlertIcons.ERROR) == DialogResult.Retry)
        //            {
        //                this.DeleteMA();
        //            }
        //            this.FormRead();
        //        }
        //    };
        //    worker.RunWorkerAsync();
        //}

        //private void btnEditCloud_Click(object sender, EventArgs e)
        //{
        //    CloudSrv cs = this.cloudsrv.Count > 0 ? this.cloudsrv.First() : null;

        //    CloudsrvFormDialog csf = new CloudsrvFormDialog(this, cs);
        //    if (csf.ShowDialog() == DialogResult.OK)
        //    {
        //        BackgroundWorker worker = new BackgroundWorker();
        //        worker.DoWork += delegate
        //        {
        //            this.getSerial(this.serial.id);
        //        };
        //        worker.RunWorkerCompleted += delegate
        //        {
        //            this.fillSerialInForm();
        //        };
        //        worker.RunWorkerAsync();
        //    }
        //}

        //private void btnDeleteCloud_Click(object sender, EventArgs e)
        //{
        //    this.DeleteCloudSrv();
        //}

        //private void DeleteCloudSrv()
        //{
        //    if (this.cloudsrv.Count == 0)
        //        return;

        //    if (MessageAlert.Show("ลบข้อมูลบริการ Express on Cloud, ทำต่อ?", "", MessageAlertButtons.OK_CANCEL, MessageAlertIcons.QUESTION) == DialogResult.Cancel)
        //        return;


        //    bool delete_success = false;
        //    string err_msg = "";

        //    this.FormProcessing();
        //    BackgroundWorker worker = new BackgroundWorker();
        //    worker.DoWork += delegate
        //    {
        //        CRUDResult delete = ApiActions.DELETE(PreferenceForm.API_MAIN_URL() + "cloudsrv/delete&id=" + this.cloudsrv[0].id.ToString());
        //        ServerResult sr = JsonConvert.DeserializeObject<ServerResult>(delete.data);

        //        if (sr.result == ServerResult.SERVER_RESULT_SUCCESS)
        //        {
        //            delete_success = true;
        //        }
        //        else
        //        {
        //            delete_success = false;
        //            err_msg = sr.message;
        //        }
        //    };
        //    worker.RunWorkerCompleted += delegate
        //    {
        //        if (delete_success)
        //        {
        //            this.cloudsrv.RemoveAll(t => t.id > -1);
        //            this.fillSerialInForm();
        //            this.FormRead();
        //        }
        //        else
        //        {
        //            if (MessageAlert.Show(err_msg, "Error", MessageAlertButtons.RETRY_CANCEL, MessageAlertIcons.ERROR) == DialogResult.Retry)
        //            {
        //                this.DeleteCloudSrv();
        //            }
        //            this.FormRead();
        //        }
        //    };
        //    worker.RunWorkerAsync();
        //}

        //private void toolStripInquiryMA_Click(object sender, EventArgs e)
        //{
        //    InquiryMaAndCloud im = new InquiryMaAndCloud(this, INQUIRY_SERVICE_TYPE.MA);
        //    if (im.ShowDialog() == DialogResult.OK)
        //    {
        //        this.getSerial(im.selected_id);
        //        this.fillSerialInForm();
        //    }
        //}

        //private void toolStripInquiryCloud_Click(object sender, EventArgs e)
        //{
        //    InquiryMaAndCloud im = new InquiryMaAndCloud(this, INQUIRY_SERVICE_TYPE.CLOUD);
        //    if (im.ShowDialog() == DialogResult.OK)
        //    {
        //        this.getSerial(im.selected_id);
        //        this.fillSerialInForm();
        //    }
        //}

        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    if (keyData == Keys.Tab && this.form_mode == FORM_MODE.READ)
        //    {
        //        DataInfo data_info = new DataInfo();

        //        int max_id = 0;
        //        foreach (Serial s in this.serial_id_list)
        //        {
        //            if (s.id > max_id)
        //            {
        //                max_id = s.id;
        //            }
        //        }

        //        data_info.lblDataTable.Text = "Serial";
        //        data_info.lblExpression.Text = (this.sortMode == SORT_SN ? this.sortMode : this.sortMode + "+sernum");
        //        data_info.lblRecBy.Text = this.serial.users_name;
        //        data_info.lblRecDate.pickedDate(this.serial.chgdat);
        //        data_info.lblRecNo.Text = this.serial.id.ToString();
        //        data_info.lblTotalRec.Text = max_id.ToString();
        //        data_info.lblTime.ForeColor = Color.DarkGray;
        //        data_info.lblRecTime.BackColor = Color.WhiteSmoke;
        //        data_info.ShowDialog();
        //        return true;
        //    }
        //    if (keyData == Keys.Tab && this.form_mode == FORM_MODE.READ_ITEM)
        //    {
        //        if (this.dgvProblem.Rows[this.dgvProblem.CurrentCell.RowIndex].Tag is Problem)
        //        {
        //            CRUDResult get = ApiActions.GET(PreferenceForm.API_MAIN_URL() + "problem/get_info&id=" + ((Problem)this.dgvProblem.Rows[this.dgvProblem.CurrentCell.RowIndex].Tag).id.ToString());
        //            ServerResult sr = JsonConvert.DeserializeObject<ServerResult>(get.data);

        //            if (sr.result == ServerResult.SERVER_RESULT_SUCCESS)
        //            {
        //                if (sr.problem.Count > 0)
        //                {
        //                    DataInfo data_info = new DataInfo();
        //                    data_info.lblDataTable.Text = "Problem";
        //                    data_info.lblExpression.Text = "sernum+date";
        //                    data_info.lblRecBy.Text = sr.problem.First<Problem>().users_name;
        //                    data_info.lblRecDate.pickedDate(sr.problem.First<Problem>().date);
        //                    data_info.lblRecTime.Text = sr.problem.First<Problem>().time;
        //                    data_info.lblRecNo.Text = sr.problem.First<Problem>().id.ToString();
        //                    data_info.lblTotalRec.Text = sr.message;
        //                    data_info.ShowDialog();
        //                }
        //            }

        //        }
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.A))
        //    {
        //        if (this.form_mode == FORM_MODE.READ_ITEM)
        //        {
        //            Problem pattern = (this.dgvProblem.CurrentCell != null && this.dgvProblem.Rows[this.dgvProblem.CurrentCell.RowIndex].Tag is Problem ? (Problem)this.dgvProblem.Rows[this.dgvProblem.CurrentCell.RowIndex].Tag : null);
        //            int problem_count = (this.is_problem_im_only ? this.problem_im_only.Count : this.problem.Count);
        //            this.dgvProblem.Rows[problem_count].Cells[1].Selected = true;
        //            //this.showInlineProblemForm(this.dgvProblem.Rows[problem_count], 1, pattern);
        //            return true;
        //        }
        //        else if (this.form_mode == FORM_MODE.READ)
        //        {
        //            this.toolStripAdd.PerformClick();
        //            return true;
        //        }
        //    }
        //    if (keyData == (Keys.Alt | Keys.E))
        //    {
        //        if (this.form_mode == FORM_MODE.READ_ITEM)
        //        {
        //            if (this.dgvProblem.Rows[this.dgvProblem.CurrentCell.RowIndex].Tag is Problem)
        //            {
        //                //this.showInlineProblemForm(this.dgvProblem.Rows[this.dgvProblem.CurrentCell.RowIndex]);
        //                return true;
        //            }
        //        }
        //        else
        //        {
        //            this.toolStripEdit.PerformClick();
        //            return true;
        //        }
        //    }
        //    if (keyData == Keys.Enter && (this.form_mode == FORM_MODE.ADD || this.form_mode == FORM_MODE.EDIT || this.form_mode == FORM_MODE.ADD_ITEM || this.form_mode == FORM_MODE.EDIT_ITEM))
        //    {
        //        if (this.current_focused_control == this.dtVerextdat || this.current_focused_control.Name == "inline_problem_probdesc")
        //        {
        //            this.toolStripSave.PerformClick();
        //            return true;
        //        }

        //        SendKeys.Send("{TAB}");
        //        return true;
        //    }
        //    if (keyData == Keys.Escape)
        //    {
        //        if (this.cbVerext.comboBox1.Focused && this.cbVerext.comboBox1.DroppedDown)
        //        {
        //            SendKeys.Send("{F4}");
        //            return true;
        //        }
        //        this.toolStripStop.PerformClick();
        //        return true;
        //    }
        //    if (keyData == Keys.F6)
        //    {
        //        if (this.current_focused_control != null)
        //        {
        //            if (this.current_focused_control == this.txtArea)
        //            {
        //                this.btnBrowseArea.PerformClick();
        //                return true;
        //            }
        //            else if (this.current_focused_control == this.txtBusityp)
        //            {
        //                this.btnBrowseBusityp.PerformClick();
        //                return true;
        //            }
        //            else if (this.current_focused_control == this.txtDealer)
        //            {
        //                this.btnBrowseDealer.PerformClick();
        //                return true;
        //            }
        //            else if (this.current_focused_control == this.txtHowknown)
        //            {
        //                this.btnBrowseHowknown.PerformClick();
        //                return true;
        //            }
        //            else if (this.current_focused_control == this.dtPurdat)
        //            {
        //                this.dtPurdat.dateTimePicker1.Focus();
        //                SendKeys.Send("{F4}");
        //                return true;
        //            }
        //            else if (this.current_focused_control == this.dtExpdat)
        //            {
        //                this.dtExpdat.dateTimePicker1.Focus();
        //                SendKeys.Send("{F4}");
        //                return true;
        //            }
        //            else if (this.current_focused_control == this.dtManual)
        //            {
        //                this.dtManual.dateTimePicker1.Focus();
        //                SendKeys.Send("{F4}");
        //                return true;
        //            }
        //            else if (this.current_focused_control == this.dtVerextdat)
        //            {
        //                this.dtVerextdat.dateTimePicker1.Focus();
        //                SendKeys.Send("{F4}");
        //                return true;
        //            }
        //            else if (this.current_focused_control == this.cbVerext)
        //            {
        //                SendKeys.Send("{F4}");
        //                return true;
        //            }
        //            else if (this.current_focused_control.Name == "inline_problem_date" && (this.form_mode == FORM_MODE.ADD_ITEM || this.form_mode == FORM_MODE.EDIT_ITEM))
        //            {
        //                Control[] ct = this.dgvProblem.Parent.Controls.Find("inline_problem_date", true);
        //                if (ct.Length > 0)
        //                {
        //                    CustomDateTimePicker dt = (CustomDateTimePicker)ct[0];
        //                    dt.dateTimePicker1.Focus();
        //                    SendKeys.Send("{F4}");
        //                    return true;
        //                }
        //            }
        //            else if (this.current_focused_control.Name == "inline_problem_probcod" && (this.form_mode == FORM_MODE.ADD_ITEM || this.form_mode == FORM_MODE.EDIT_ITEM))
        //            {
        //                Control[] ct = this.dgvProblem.Parent.Controls.Find("inline_problem_probcod", true);
        //                if (ct.Length > 0)
        //                {
        //                    CustomTextBox probcod = (CustomTextBox)ct[0];
        //                    probcod.Focus();
        //                    IstabList wind = new IstabList(this.main_form, probcod.Texts, Istab.TABTYP.PROBLEM_CODE);
        //                    if (wind.ShowDialog() == DialogResult.OK)
        //                    {
        //                        probcod.Texts = wind.istab.typcod;
        //                        SendKeys.Send("{TAB}");
        //                    }
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    if (keyData == Keys.PageUp)
        //    {
        //        if (this.form_mode == FORM_MODE.READ)
        //        {
        //            this.toolStripPrevious.PerformClick();
        //            return true;
        //        }
        //    }
        //    if (keyData == Keys.PageDown)
        //    {
        //        if (this.form_mode == FORM_MODE.READ)
        //        {
        //            this.toolStripNext.PerformClick();
        //            return true;
        //        }
        //    }
        //    if (keyData == (Keys.Control | Keys.Home))
        //    {
        //        this.toolStripFirst.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Control | Keys.End))
        //    {
        //        this.toolStripLast.PerformClick();
        //        return true;
        //    }
        //    if (keyData == Keys.F3)
        //    {
        //        if (this.form_mode == FORM_MODE.READ)
        //        {
        //            this.tabControl1.SelectedTab = this.tabPage1;
        //            return true;
        //        }
        //    }
        //    if (keyData == Keys.F4)
        //    {
        //        if (this.form_mode == FORM_MODE.READ)
        //        {
        //            this.tabControl1.SelectedTab = this.tabPage2;
        //            return true;
        //        }
        //    }
        //    if (keyData == Keys.F5)
        //    {
        //        this.toolStripReload.PerformClick();
        //        return true;
        //    }
        //    if (keyData == Keys.F8)
        //    {
        //        this.toolStripItem.PerformClick();
        //        return true;
        //    }
        //    if (keyData == Keys.F9)
        //    {
        //        this.toolStripSave.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.D))
        //    {
        //        if (this.form_mode == FORM_MODE.READ_ITEM)
        //        {
        //            //this.deleteProblemData();
        //            return true;
        //        }
        //        else
        //        {
        //            this.toolStripDelete.PerformClick();
        //            return true;
        //        }
        //    }
        //    if (keyData == (Keys.Control | Keys.L))
        //    {
        //        this.toolStripInquiryAll.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.L))
        //    {
        //        this.toolStripInquiryRest.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.S))
        //    {
        //        this.toolStripSearchSN.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.D2))
        //    {
        //        this.toolStripSearchContact.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.D3))
        //    {
        //        this.toolStripSearchCompany.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.D4))
        //    {
        //        this.toolStripSearchDealer.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.D5))
        //    {
        //        this.toolStripSearchOldnum.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.D6))
        //    {
        //        this.toolStripSearchBusityp.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Alt | Keys.D7))
        //    {
        //        this.toolStripSearchArea.PerformClick();
        //        return true;
        //    }
        //    if (keyData == Keys.F2)
        //    {
        //        this.btnSupportNote.PerformClick();
        //        return true;
        //    }
        //    if (keyData == (Keys.Control | Keys.Alt | Keys.M))
        //    {
        //        this.toolStripInquiryMA.PerformClick();
        //    }
        //    if (keyData == (Keys.Control | Keys.Alt | Keys.C))
        //    {
        //        this.toolStripInquiryCloud.PerformClick();
        //    }

        //    return base.ProcessCmdKey(ref msg, keyData);
        //}
        #endregion not use

        private void SnWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.MdiFormClosing && (this.form_mode != FORM_MODE.READ && this.form_mode != FORM_MODE.READ_ITEM))
            {
                this.Activate();
                if (MessageAlert.Show(StringResource.CONFIRM_CLOSE_WINDOW, "SN_Net", MessageAlertButtons.OK_CANCEL, MessageAlertIcons.WARNING) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (e.CloseReason == CloseReason.UserClosing && (this.form_mode != FORM_MODE.READ && this.form_mode != FORM_MODE.READ_ITEM))
            {
                this.Activate();
                if (MessageAlert.Show(StringResource.CONFIRM_CLOSE_WINDOW, "SN_Net", MessageAlertButtons.OK_CANCEL, MessageAlertIcons.WARNING) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }

            this.main_form.sn_wind = null;
            if (this.main_form.supportnote_wind != null)
            {
                this.main_form.supportnote_wind.Close();
            }
        }
    }

    //public class CompareStrings : IComparer<string>
    //{
    //    // Because the class implements IComparer, it must define a 
    //    // Compare method. The method returns a signed integer that indicates 
    //    // whether s1 > s2 (return is greater than 0), s1 < s2 (return is negative),
    //    // or s1 equals s2 (return value is 0). This Compare method compares strings. 
    //    public int Compare(string s1, string s2)
    //    {
    //        return string.CompareOrdinal(s1, s2);
    //    }
    //}

}
