using System;
using System.Windows.Forms;

namespace EventLogReader
{
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
        }

        private void SetConfig()
        {
            Globals.Config.Directory = txtDir.Text;
            Globals.Config.OffsetSeconds = Globals.GetIntValue(txtOffset.Text);
            Globals.Config.ClearMinutes = Globals.GetIntValue(txtClearMinutesOld.Text);


            Globals.Config.SqldServer = txtDbServer.Text;
            Globals.Config.SqldDb = txtDb.Text;
            Globals.Config.SqlIsTrusted = cbTrusted.Checked ? true : false;
            Globals.Config.SqldUser = txtDbUser.Text;
            Globals.Config.SqldPassword = txtPass.Text;


            Config.SetConfig();
        }

        private void LoadConfig()
        {
            Config.GetConfig();
            txtDir.Text = Globals.Config.Directory;
            txtOffset.Text = Globals.Config.OffsetSeconds.ToString();
            txtClearMinutesOld.Text = Globals.Config.ClearMinutes.ToString();

            txtDbServer.Text = Globals.Config.SqldServer;
            txtDb.Text = Globals.Config.SqldDb;
            cbTrusted.Checked = Globals.Config.SqlIsTrusted;
            txtDbUser.Text = Globals.Config.SqldUser;
            txtPass.Text = Globals.Config.SqldPassword;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SetConfig();
            this.Close();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDir.Text = folderBrowserDialog1.SelectedPath + @"\";     
            }
        }
    }
}
