using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Globalization;
using Microsoft.Phone;
using Microsoft.Phone.Tasks;
using System.Threading;

namespace MortgageCalc
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void buttonMortgageCalculate_Click(object sender, RoutedEventArgs e)
        {
            double principal; // total mortgage loan
            double interestPerc; // percent annual interest
            double interestRate; // monthly interest rate
            double years; // years to pay
            double downpayment; // down payment percent
            double paymentNum; // number of months to pay
            double paymentVal; // value of monthly payment

            principal = double.Parse(this.textBoxMortgageAmount.Text, NumberStyles.Any);
            interestPerc = double.Parse(this.textBoxMortgageInterestRate.Text);
            interestRate = interestPerc / (100 * 12);
            years = double.Parse(this.textBoxMortgageTerm.Text);
            downpayment = double.Parse(this.textBoxMortgageDownPayment.Text);
            paymentNum = years * 12;

            paymentVal = (principal - principal * downpayment / 100) * (interestRate / (1 - Math.Pow((1 + interestRate), (-paymentNum))));

            CultureInfo cult = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cult;
            this.textBoxMortgageMonthlyPayments.Text = paymentVal.ToString("C");
            this.textBoxMortgageAmount.Text = principal.ToString("C");

        }

        private void buttonImage_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask wbt = new WebBrowserTask();
            wbt.URL = "http://www.amerisave.com";
            wbt.Show();
        }

    }
}