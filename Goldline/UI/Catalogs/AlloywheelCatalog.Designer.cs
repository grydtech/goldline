namespace Goldline.UI.Catalogs
{
    partial class AlloywheelCatalog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.AlloywheelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.AlloywheelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // AlloywheelBindingSource
            // 
            this.AlloywheelBindingSource.DataSource = typeof(Core.Model.Products.Alloywheel);
            // 
            // reportViewer
            // 
            reportDataSource1.Name = "AlloywheelDataset";
            reportDataSource1.Value = this.AlloywheelBindingSource;
            this.reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer.LocalReport.ReportEmbeddedResource = "Goldline.Reporting.Catalogs.Catalog_Alloywheel.rdlc";
            this.reportViewer.Location = new System.Drawing.Point(12, 12);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.Size = new System.Drawing.Size(920, 657);
            this.reportViewer.TabIndex = 1;
            // 
            // AlloywheelCatalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 681);
            this.Controls.Add(this.reportViewer);
            this.Name = "AlloywheelCatalog";
            this.Text = "AlloywheelCatalog";
            this.Load += new System.EventHandler(this.AlloywheelCatalog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AlloywheelBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer;
        private System.Windows.Forms.BindingSource AlloywheelBindingSource;
    }
}