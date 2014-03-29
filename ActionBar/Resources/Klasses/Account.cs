using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

namespace com.ingenious.android
{
    class Account
    {
        public String owner;
        public String name;
        public String iban;
        public String bic;
        public Decimal balance;
        public String currency;
        //public List<Payment> payments;

        public Account()
        {
            this.name = "";
            this.owner = "";
            this.iban = "";
            this.bic = "";
            this.balance = 0;
            this.currency = "";
        }

        public Account(String owner, String iban, String bic, String currency)
        {
            this.name = "";
            this.owner = owner;
            this.iban = iban;
            this.bic = bic;
            this.currency = currency;
        }

        public Account(String owner, String iban, String bic, Decimal balance, String currency)
        {
            this.name = "";
            this.owner = owner;
            this.iban = iban;
            this.bic = bic;
            this.balance = balance;
            this.currency = currency;
        }

        public Account(String name, String owner, String iban, String bic, String currency)
        {
            this.name = name;
            this.owner = owner;
            this.iban = iban;
            this.bic = bic;
            this.currency = currency;
        }

        public Account(String name, String owner, String iban, String bic, Decimal balance, String currency)
        {
            this.name = name;
            this.owner = owner;
            this.iban = iban;
            this.bic = bic;
            this.balance = balance;
            this.currency = currency;
        }
    }
}