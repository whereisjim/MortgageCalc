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
            double propertyPrice; // total mortgage loan
            double interestPerc; // percent annual interest
            double interestRate; // monthly interest rate
            double years; // years to pay
            double downpaymentPerc; // down payment percent
            double downpaymentAmount; // down payment amount
            double paymentNum; // number of months to pay
            double paymentVal; // value of monthly payment

            propertyPrice = double.Parse(this.textBoxMortgageAmount.Text, NumberStyles.Any);
            interestPerc = double.Parse(this.textBoxMortgageInterestRate.Text);
            interestRate = interestPerc / (100 * 12);
            years = double.Parse(this.textBoxMortgageTerm.Text);
            downpaymentPerc = double.Parse(this.textBoxMortgageDownPaymentPercent.Text);
            downpaymentAmount = propertyPrice * (downpaymentPerc / 100);
            paymentNum = years * 12;

            paymentVal = (propertyPrice - propertyPrice * downpaymentPerc / 100) * (interestRate / (1 - Math.Pow((1 + interestRate), (-paymentNum))));

            CultureInfo cult = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cult;
            this.textBoxMortgageMonthlyPayments.Text = paymentVal.ToString("C");
            this.textBoxMortgageAmount.Text = propertyPrice.ToString("C");
            this.textBlockMortgageDownPaymentAmount.Text = downpaymentAmount.ToString("C");

        }

        private void buttonImage_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask wbt = new WebBrowserTask();
            wbt.URL = "http://www.amerisave.com";
            wbt.Show();
        }

        private void buttonTipCalculate_Click(object sender, RoutedEventArgs e)
        {
            double billAmount;
            double tipPercent;
            double numberOfPeople;
            double tipAmount;
            double totalTipPerPersonAmount;

            billAmount = double.Parse(this.textBoxBillAmount.Text);
            tipPercent = double.Parse(this.textBoxTipPercent.Text);
            numberOfPeople = double.Parse(this.textBoxTipNumberOfPeople.Text);

            tipAmount = billAmount * (tipPercent / 100);
            totalTipPerPersonAmount = tipAmount / numberOfPeople;

            this.textBoxTipAmount.Text = tipAmount.ToString();
            this.textBoxTipTotalPerPerson.Text = totalTipPerPersonAmount.ToString();

        }

        private void buttonMortgageDetail_Click(object sender, RoutedEventArgs e)
        {

        }


        private void buttonLoanCalculate_Click(object sender, RoutedEventArgs e)
        {
            double carPrice; // Purchase price of car
            double carinterestPerc; // percent annual interest
            double carinterestRate; // monthly interest rate
            double carmonths; // months to pay
            double cardownpaymentPerc; // down payment percent
            double cardownpaymentAmount; // down payment amount
            double carpaymentVal; // value of monthly payment

            try
            {
               carPrice = double.Parse(this.textBoxCarAmount.Text, NumberStyles.Any);
               if (carPrice == 0) MessageBox.Show("Please check your purchase price and enter your correct purchase price");
                
            }
            catch(Exception)
            {
                carPrice = -1;
                MessageBox.Show("Purchase price is invalid. Please enter your correct one");
            }

            try
            {
                carinterestPerc = double.Parse(this.textBoxLoanInterestRate.Text);
            }
            catch (Exception)
            {
                carinterestPerc = -1;
                MessageBox.Show("Interest rate is invalid. Please enter your correct one");
            }
           
            carinterestRate = carinterestPerc / (100 * 12);
            try
            {
                carmonths = double.Parse(this.textBoxLoanTerm.Text);
                if (carmonths == 0) MessageBox.Show("Please check your loan term and enter your correct purchase price");
                

            }
            catch (Exception)
            {
                carmonths = -1;
                MessageBox.Show("Loan term is invalid. Please enter your correct one");
            }

            try
            {
                cardownpaymentPerc = double.Parse(this.textBoxLoanDownPaymentPercent.Text);
            }
            catch (Exception)
            {
                cardownpaymentPerc = -1;
                MessageBox.Show("Downpayment is invalid. Please enter your correct one");
            }
            cardownpaymentAmount = carPrice * (cardownpaymentPerc / 100);

            carpaymentVal = (carPrice - carPrice * cardownpaymentPerc / 100) * (carinterestRate / (1 - Math.Pow((1 + carinterestRate), (-carmonths))));

            CultureInfo cult = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cult;
            this.textBoxLoanMonthlyPayments.Text = carpaymentVal.ToString("C");
            this.textBoxCarAmount.Text = carPrice.ToString("C");
            this.textBlockLoanDownPaymentAmount.Text = cardownpaymentAmount.ToString("C");

        }


    }
}