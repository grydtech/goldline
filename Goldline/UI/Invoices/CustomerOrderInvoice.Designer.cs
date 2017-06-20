using Core.Domain.Model.Customers;

namespace Goldline.UI.Invoices
{
    partial class CustomerOrderInvoice
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.CustomerOrderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            this.orderEntriesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.CustomerOrderBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderEntriesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // CustomerOrderBindingSource
            // 
            this.CustomerOrderBindingSource.DataSource = typeof(Order);
            // 
            // reportViewer
            // 
            reportDataSource1.Name = "CustomerOrderDataset";
            reportDataSource1.Value = this.CustomerOrderBindingSource;
            reportDataSource2.Name = "CustomerOrderEntries";
            reportDataSource2.Value = this.orderEntriesBindingSource;
            this.reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer.LocalReport.ReportEmbeddedResource = "Goldline.Reporting.Invoice.Invoice_CustomerOrder.rdlc";
            this.reportViewer.Location = new System.Drawing.Point(12, 12);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.Size = new System.Drawing.Size(760, 537);
            this.reportViewer.TabIndex = 0;
            // 
            // orderEntriesBindingSource
            // 
            this.orderEntriesBindingSource.DataMember = "OrderItems";
            this.orderEntriesBindingSource.DataSource = this.CustomerOrderBindingSource;
            // 
            // CustomerOrderInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.reportViewer);
            this.Name = "CustomerOrderInvoice";
            this.Text = "CustomerOrderInvoice";
            this.Load += new System.EventHandler(this.CustomerOrderInvoice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CustomerOrderBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderEntriesBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer;
        private System.Windows.Forms.BindingSource CustomerOrderBindingSource;
        private System.Windows.Forms.BindingSource orderEntriesBindingSource;
    }
}