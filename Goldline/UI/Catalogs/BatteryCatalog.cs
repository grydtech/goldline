using System;
using System.Linq;
using System.Windows.Forms;
using Core.Domain.Enums;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;

namespace Goldline.UI.Catalogs
{
    public partial class BatteryCatalog : Form
    {
        public BatteryCatalog()
        {
            InitializeComponent();
        }

        private void BatteryCatalog_Load(object sender, EventArgs e)
        {
            var batteries = new ReportHandler().GetCatalog(ProductType.Battery).Cast<Battery>();
            BatteryBindingSource.DataSource = batteries;
            reportViewer.RefreshReport();
        }
    }
}