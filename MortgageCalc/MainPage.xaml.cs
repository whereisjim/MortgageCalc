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
using System.IO;
using System.IO.IsolatedStorage;

namespace MortgageCalc
{
    public partial class MainPage : PhoneApplicationPage
    {
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for Settings
        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            if (settings.Contains("textBoxMortgageAmountSettings"))
            {
                textBoxMortgageAmount.Text = settings["textBoxMortgageAmountSettings"] as string;
            }
            else
            {
               textBoxMortgageAmount.Text = "$150,000.00";
            }

            if (settings.Contains("textBoxMortgageTermSettings"))
            {
                textBoxMortgageTerm.Text = settings["textBoxMortgageTermSettings"] as string;
            }
            else
            {
                textBoxMortgageTerm.Text = "30";
            }

            if (settings.Contains("textBoxMortgageInterestRateSettings"))
            {
                textBoxMortgageInterestRate.Text = settings["textBoxMortgageInterestRateSettings"] as string;
            }
            else
            {
                textBoxMortgageInterestRate.Text = "7";
            }

            if (settings.Contains("textBoxMortgageDownPaymentPercentSettings"))
            {
                textBoxMortgageDownPaymentPercent.Text = settings["textBoxMortgageDownPaymentPercentSettings"] as string;
            }
            else
            {
                textBoxMortgageDownPaymentPercent.Text = "20";
            }
                        
            if (settings.Contains("textBlockMortgageDownPaymentAmountSettings"))
            {
                textBlockMortgageDownPaymentAmount.Text = settings["textBlockMortgageDownPaymentAmountSettings"] as string;
            }
            else
            {
                textBlockMortgageDownPaymentAmount.Text = "$30,000.00";
            }

            if (settings.Contains("textBoxMortgageMonthlyPaymentsSettings"))
            {
                textBoxMortgageMonthlyPayments.Text = settings["textBoxMortgageMonthlyPaymentsSettings"] as string;
            }
            else
            {
                textBoxMortgageMonthlyPayments.Text = "$798.36";
            }
        } 
      

        private void buttonMortgageCalculate_Click(object sender, RoutedEventArgs e)
        {
            double housePrice; // total mortgage loan
            double interestPerc; // percent annual interest
            double interestRate; // monthly interest rate
            double years; // years to pay
            double downpaymentPerc; // down payment percent
            double downpaymentAmount; // down payment amount
            double paymentNum; // number of months to pay
            double paymentVal; // value of monthly payment
            // string tempText;

            try
            {
                housePrice = double.Parse(this.textBoxMortgageAmount.Text, NumberStyles.Any);
                if (housePrice == 0) MessageBox.Show("You entered $0 for house price. Are you sure?");

            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong with the house price. Check the price and try again.");
                housePrice = 0;
                // Jim.ThingsToDo: replace 'housePrice' value from isolatedStorate Setting
                // a. Check if settings contains any value.
                // b. Check if settings variable name and textblock name can be same or should have different name?
                if (settings.Contains("textBoxMortgageAmountSettings"))
                {
                    textBoxMortgageAmount.Text = settings["textBoxMortgageAmountSettings"] as string;
                    housePrice = double.Parse(textBoxMortgageAmount.Text, NumberStyles.Any);
                }
                else
                {
                    housePrice = 150000.00;
                }
            }
            
            try
            {
                interestPerc = double.Parse(this.textBoxMortgageInterestRate.Text);
                if (interestPerc == 0) MessageBox.Show("0% interest? Nice!");
            }
            catch (Exception)
            {
                MessageBox.Show("Something wrong with the interest rate. Check the rate and try again");
                interestPerc = 1;
                if (settings.Contains("textBoxMortgageInterestRateSettings"))
                {
                    textBoxMortgageInterestRate.Text = settings["textBoxMortgageInterestRateSettings"] as string;
                    interestPerc = double.Parse(textBoxMortgageInterestRate.Text);
                }
            }
           
            interestRate = interestPerc / (100 * 12);

            try
            {
                years = double.Parse(this.textBoxMortgageTerm.Text);
            }
            catch (Exception)
            {
                years = -1;
                MessageBox.Show("Mortgage term is invalid. Please enter your correct one");
            }

            try
            {
                downpaymentPerc = double.Parse(this.textBoxMortgageDownPaymentPercent.Text);
            }
            catch (Exception)
            {
                downpaymentPerc = -1;
                MessageBox.Show("Mortgage term is invalid. Please enter your correct one");
            }
            
            downpaymentAmount = housePrice * (downpaymentPerc / 100);
            paymentNum = years * 12;

            paymentVal = (housePrice - housePrice * downpaymentPerc / 100) * (interestRate / (1 - Math.Pow((1 + interestRate), (-paymentNum))));

            CultureInfo cult = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cult;
            
            // Add or update the settings
            if (settings.Contains("textBoxMortgageAmountSettings"))
            {
                settings["textBoxMortgageAmountSettings"] = housePrice.ToString("C");
            }
            else
            {
                settings.Add("textBoxMortgageAmountSettings", housePrice.ToString("C"));
            }
            this.textBoxMortgageAmount.Text = housePrice.ToString("C");

            if (settings.Contains("textBoxMortgageTermSettings"))
            {
                settings["textBoxMortgageTermSettings"] = years.ToString();
            }
            else
            {
                settings.Add("textBoxMortgageTermSettings", years.ToString());
            }

            if (settings.Contains("textBoxMortgageInterestRateSettings"))
            {
                settings["textBoxMortgageInterestRateSettings"] = interestPerc.ToString();
            }
            else
            {
                settings.Add("textBoxMortgageInterestRateSettings", years.ToString());
            }

            if (settings.Contains("textBoxMortgageDownPaymentPercentSettings"))
            {
                settings["textBoxMortgageDownPaymentPercentSettings"] = downpaymentPerc.ToString();
            }
            else
            {
                settings.Add("textBoxMortgageDownPaymentPercentSettings", downpaymentPerc.ToString());
            }

            if (settings.Contains("textBlockMortgageDownPaymentAmountSettings"))
            {
                settings["textBlockMortgageDownPaymentAmountSettings"] = downpaymentAmount.ToString("C");
            }
            else
            {
                settings.Add("textBlockMortgageDownPaymentAmountSettings", downpaymentAmount.ToString("C"));
            }
            this.textBlockMortgageDownPaymentAmount.Text = downpaymentAmount.ToString("C");

            if (settings.Contains("textBoxMortgageMonthlyPaymentsSettings"))
            {
                settings["textBoxMortgageMonthlyPaymentsSettings"] = paymentVal.ToString("C");
            }
            else
            {
                settings.Add("textBoxMortgageMonthlyPaymentsSettings", paymentVal.ToString("C"));
            }
            this.textBoxMortgageMonthlyPayments.Text = paymentVal.ToString("C");
        }

        private void buttonImage_Click(object sender, RoutedEventArgs e)
        {
            WebBrowserTask wbt = new WebBrowserTask();
            wbt.URL = "http://www.whereisjim.com/home/FinCalculatorMainBottomImageClick.htm";
            wbt.Show();
        }

        private void buttonTipCalculate_Click(object sender, RoutedEventArgs e)
        {
            double billAmount;
            double tipPercent;
            double numberOfPeople;
            double tipAmount;
            double totalPerPersonAmount;

            try
            {
                billAmount = double.Parse(this.textBoxBillAmount.Text);
                if (billAmount == 0) MessageBox.Show("Please check your purchase price and enter your correct purchase price");

            }
            catch (Exception)
            {
                billAmount = -1;
                MessageBox.Show("Bill Amount is invalid. Please enter your correct one");
            }

            try
            {
                tipPercent = double.Parse(this.textBoxTipPercent.Text);
            }
            catch (Exception)
            {
                tipPercent = -1;
                MessageBox.Show("Percentage of Tip is invalid. Please enter your correct one");
            }

            try
            {
                numberOfPeople = double.Parse(this.textBoxTipNumberOfPeople.Text);
            }
            catch (Exception)
            {
                numberOfPeople = -1;
                MessageBox.Show("Number of People is invalid. Please enter your correct one");
            }
            

            tipAmount = billAmount * (tipPercent / 100);
            totalPerPersonAmount = (billAmount + tipAmount) / numberOfPeople;

            this.textBoxTipAmount.Text = tipAmount.ToString();
            this.textBoxTotalPerPerson.Text = totalPerPersonAmount.ToString();
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

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
           

        }


    }
}