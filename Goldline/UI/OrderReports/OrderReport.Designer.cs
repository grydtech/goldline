using Core.Domain.Model.Customers;

namespace Goldline.UI.OrderReports
{
    partial class OrderReport
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
            this.CustomerOrderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.reportViewer = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.CustomerOrderBindingSource)).BeginInit();
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
            this.reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer.LocalReport.ReportEmbeddedResource = "Goldline.Reporting.Orders.Report_CustomerOrder.rdlc";
            this.reportViewer.Location = new System.Drawing.Point(12, 12);
            this.reportViewer.Name = "reportViewer";
            this.reportViewer.Size = new System.Drawing.Size(920, 657);
            this.reportViewer.TabIndex = 1;
            // 
            // CustomerCashOrderReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 681);
            this.Controls.Add(this.reportViewer);
            this.Name = "CustomerCashOrderReport";
            this.Text = "CustomerCashOrderReport";
            this.Load += new System.EventHandler(this.CustomerCashOrderReport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CustomerOrderBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer;
        private System.Windows.Forms.BindingSource CustomerOrderBindingSource;
    }
}