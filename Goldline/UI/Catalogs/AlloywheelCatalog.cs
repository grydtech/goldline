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
            AlloywheelBindingSource.DataSource = new ProductHandler.AlloywheelHandler().GetProducts(productType: ProductType.Alloywheel)
                .Cast<Alloywheel>();
            reportViewer.RefreshReport();
        }
    }
}