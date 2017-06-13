using System;
using System.Linq;
using System.Windows.Forms;
using Core.Model.Enums;
using Core.Model.Handlers;
using Core.Model.Products;

namespace Goldline.UI.Catalogs
{
    public partial class TyreCatalog : Form
    {
        public TyreCatalog()
        {
            InitializeComponent();
        }

        private void TyreCatalog_Load(object sender, EventArgs e)
        {
            var tyres = new ReportHandler().GetCatalog(ProductType.Tyre).Cast<Tyre>();
            TyreBindingSource.DataSource = tyres;
            reportViewer.RefreshReport();
        }
    }
}