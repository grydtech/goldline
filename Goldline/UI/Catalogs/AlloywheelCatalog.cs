using System;
using System.Linq;
using System.Windows.Forms;
using Core.Domain.Enums;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;

namespace Goldline.UI.Catalogs
{
    public partial class AlloywheelCatalog : Form
    {
        public AlloywheelCatalog()
        {
            InitializeComponent();
        }

        private void AlloywheelCatalog_Load(object sender, EventArgs e)
        {
            var alloywheels = new ReportHandler().GetCatalog(ProductType.Alloywheel).Cast<Alloywheel>();
            AlloywheelBindingSource.DataSource = alloywheels;
            reportViewer.RefreshReport();
        }
    }
}