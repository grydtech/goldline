using System;
using System.Windows;
using Core.Domain.Handlers;
using Core.Domain.Model.Customers;
using Core.Domain.Model.Employees;
using Core.Domain.Model.Suppliers;

namespace Goldline.UI
{
    /// <summary>
    ///     Interaction logic for AddPaymentDialog.xaml
    /// </summary>
    public partial class AddPaymentDialog : Window
    {
        public AddPaymentDialog(Order order, decimal dueAmount)
        {
            ParentId = order.Id.GetValueOrDefault();
            MaxPaymentAmount = dueAmount;
            Type = PaymentType.OrderPayment;
            InitializeComponent();
        }

        public AddPaymentDialog(Purchase purchase, decimal dueAmount)
        {
            ParentId = purchase.Id.GetValueOrDefault();
            MaxPaymentAmount = dueAmount;
            Type = PaymentType.PurchasePayment;
            InitializeComponent();
        }

        public AddPaymentDialog(Employee employee, decimal dueAmount)
        {
            ParentId = employee.Id.GetValueOrDefault();
            MaxPaymentAmount = dueAmount;
            Type = PaymentType.EmployeePayment;
            InitializeComponent();
        }

        private uint ParentId { get; }
        private decimal MaxPaymentAmount { get; }
        private PaymentType Type { get; }


        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            var paymentAmt = Convert.ToInt32(AmountTextBox.Text);
            if (paymentAmt <= 0 || paymentAmt > MaxPaymentAmount)
                MessageBox.Show("Please enter a value greater than 0", "Error", MessageBoxButton.OK);
            else
                try
                {
                    switch (Type)
                    {
                        case PaymentType.OrderPayment:
                            new OrderPaymentHandler().AddPayment(new OrderPayment(ParentId, paymentAmt,
                                NoteTextBox.Text));
                            break;
                        case PaymentType.EmployeePayment:
                            new EmployeePaymentHandler().AddPayment(new EmployeePayment(ParentId, paymentAmt,
                                NoteTextBox.Text));
                            break;
                        case PaymentType.PurchasePayment:
                            //new PurchasePaymentHandler().AddPayment(new PurchasePayment(ParentId, paymentAmt,
                            //    NoteTextBox.Text));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private enum PaymentType
        {
            OrderPayment,
            EmployeePayment,
            PurchasePayment
        }
    }
}