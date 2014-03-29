using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace IngHackaton
{
    public class Payment
    {
        public DateTime dueDate;
        public string dueDateStr;
        public Decimal amount;
        public string currency;
        public string firmName;
        public Address address;
        public string iban;
        public string bic;
        public string reference;
        public string referenceStructured;

        public Payment()
        {
        }

        public Payment(DateTime dueDate, string currency, Decimal amount, string firmName, Address address, string iban, string bic, string reference, string referenceStructured)
        {
            this.dueDate = dueDate;
            this.dueDateStr = dueDate.ToString().Substring(0, dueDate.ToString().IndexOf(" "));
            this.amount = amount;
            this.currency = currency;
            this.firmName = firmName;
            this.address = address;
            this.iban = iban;
            this.bic = bic;
            this.reference = reference;
            this.referenceStructured = referenceStructured;
        }
        public Payment(DateTime dueDate, string currency, Decimal amount, string iban, string bic, string reference, string referenceStructured)
        {
            this.dueDate = dueDate;
            this.dueDateStr = dueDate.ToString().Substring(0, dueDate.ToString().IndexOf(" "));
            this.amount = amount;
            this.currency = currency;
            this.firmName = "";
            this.address = null;
            this.iban = iban;
            this.bic = bic;
            this.reference = reference;
            this.referenceStructured = referenceStructured;
        }
        
        // Getters
        public DateTime getDueDate()
        {
            return dueDate;
        }
        public Decimal getAmount()
        {
            return amount;
        }
        public string getCurrency()
        {
            return currency;
        }
        public string getFirmName()
        {
            return firmName;
        }
        public Address getAddress()
        {
            return address;
        }
        public string getIban()
        {
            return iban;
        }
        public string getBic()
        {
            return bic;
        }
        public string getReference()
        {
            return reference;
        }
        public string getReferenceStructured()
        {
            return referenceStructured;
        }
    }
}