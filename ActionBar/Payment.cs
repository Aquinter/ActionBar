using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ActionBar
{
    class Payment
    {
        public DateTime dueDate;
        public Decimal amount;
        public String currency;
        public String firmName;
        public String address;
        public String IBAN;
        public String BIC;
        public String reference;
        public String referenceType;

        public Payment()
        {
        }

        public Payment(DateTime dueDate, String currency, Decimal amount, String firmName, String address, String IBAN, String BIC, String reference, String referenceType)
        {
            this.dueDate = dueDate;
            this.amount = amount;
            this.currency = currency;
            this.firmName = firmName;
            this.address = address;
            this.IBAN = IBAN;
            this.BIC = BIC;
            this.reference = reference;
            this.referenceType = referenceType;
        }
    }
}