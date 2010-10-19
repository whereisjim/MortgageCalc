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

            if (settings.Contains("textboxMA"))
            {     
              textBoxMortgageAmount.Text = settings["textboxMA"] as string;
            }
            else
            {
               textBoxMortgageAmount.Text = "$150,000.00";
            }

            if (settings.Contains("textboxYears"))
            {
                textBoxMortgageTerm.Text = settings["textboxYears"] as string;
            }
            else
            {
                textBoxMortgageTerm.Text = "30";
            }

            if (settings.Contains("textboxIP"))
            {
                textBoxMortgageInterestRate.Text = settings["textboxIP"] as string;
            }
            else
            {
                textBoxMortgageInterestRate.Text = "7";
            }

            if (settings.Contains("textboxMP"))
            {
                textBoxMortgageMonthlyPayments.Text = settings["textboxMP"] as string;
            }
            else
            {
                textBoxMortgageMonthlyPayments.Text = "$798.36";
            }

            if (settings.Contains("textboxDPP"))
            {
               textBoxMortgageDownPaymentPercent.Text = settings["textboxDPP"] as string;
            }
            else
            {
                textBoxMortgageDownPaymentPercent.Text = "20";
            }

            if (settings.Contains("textblockDPA"))
            {
                textBlockMortgageDownPaymentAmount.Text = settings["textblockDPA"] as string;
            }
            else
            {
                textBlockMortgageDownPaymentAmount.Text = "$30,000.00";
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

            try
            {
                propertyPrice = double.Parse(this.textBoxMortgageAmount.Text, NumberStyles.Any);
                if (propertyPrice == 0) MessageBox.Show("Please check your purchase price and enter your correct purchase price");

            }
            catch (Exception)
            {
                propertyPrice = -1;
                MessageBox.Show("Purchase price is invalid. Please enter your correct one");
            }

            try
            {
                interestPerc = double.Parse(this.textBoxMortgageInterestRate.Text);
            }
            catch (Exception)
            {
                interestPerc = -1;
                MessageBox.Show("Interest rate is invalid. Please enter your correct one");
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
            
            downpaymentAmount = propertyPrice * (downpaymentPerc / 100);
            paymentNum = years * 12;

            paymentVal = (propertyPrice - propertyPrice * downpaymentPerc / 100) * (interestRate / (1 - Math.Pow((1 + interestRate), (-paymentNum))));

            CultureInfo cult = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = cult;
            
            // Add or update the settings
            if (settings.Contains("textboxMA"))
            {
                settings["textboxMA"] = propertyPrice.ToString("C");
            }
            else
            {
                settings.Add("textboxMA", propertyPrice.ToString("C"));
            }
            this.textBoxMortgageAmount.Text = propertyPrice.ToString("C");

            if (settings.Contains("textboxYears"))
            {
                settings["textboxYears"] = years.ToString();
            }
            else
            {
                settings.Add("textboxYears", years.ToString());
            }

            if (settings.Contains("textboxIP"))
            {
                settings["textboxIP"] = interestPerc.ToString();
            }
            else
            {
                settings.Add("textboxIP", years.ToString());
            }

            if (settings.Contains("textboxDPP"))
            {
                settings["textboxDPP"] = downpaymentPerc.ToString();
            }
            else
            {
                settings.Add("textboxDPP", downpaymentPerc.ToString());
            }

            if (settings.Contains("textblockDPA"))
            {
                settings["textblockDPA"] = downpaymentAmount.ToString("C");
            }
            else
            {
                settings.Add("textblockDPA", downpaymentAmount.ToString("C"));
            }
            this.textBlockMortgageDownPaymentAmount.Text = downpaymentAmount.ToString("C");

            if (settings.Contains("textboxMP"))
            {
                settings["textboxMP"] = paymentVal.ToString("C");
            }
            else
            {
                settings.Add("textboxMP", paymentVal.ToString("C"));
            }
            this.textBoxMortgageMonthlyPayments.Text = paymentVal.ToString("C");
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