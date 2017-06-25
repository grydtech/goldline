using System;
using System.Linq;
using System.Windows.Forms;
using Core.Domain.Enums;
using Core.Domain.Handlers;
using Core.Domain.Model.Inventory;

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
            TyreBindingSource.DataSource = new ProductHandler.TyreHandler().GetProducts(productType: ProductType.Tyre)
                .Cast<Tyre>();
            reportViewer.RefreshReport();
        }
    }
}