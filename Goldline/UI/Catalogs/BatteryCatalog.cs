using System;
using System.Linq;
using System.Windows.Forms;
using Core.Model.Enums;
using Core.Model.Handlers;
using Core.Model.Products;

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