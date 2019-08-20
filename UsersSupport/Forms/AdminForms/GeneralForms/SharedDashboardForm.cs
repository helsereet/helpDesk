using System.Windows.Forms;
using UsersSupport.Forms.GeneralForms;

namespace UsersSupport.Forms.AdminForms.GeneralForms
{

    public partial class SharedDashboardForm : Form
    {
        protected internal MainForm MainFrm { get; set; }

        protected internal DataGridView DataGridView { get; set; }

        protected internal bool ExceptionOccured { get; set; } = false;

        public SharedDashboardForm()
        {
            InitializeComponent();
            AcceptButton = leftBtn;
            CancelButton = cancelBtn;
        }

        public SharedDashboardForm(MainForm mainForm, DataGridView dataGridView) : this(mainForm)
        {
            DataGridView = dataGridView ?? null;
        }

        public SharedDashboardForm(MainForm mainForm) : this()
        {
            MainFrm = mainForm ?? null;
        }

    }
}
