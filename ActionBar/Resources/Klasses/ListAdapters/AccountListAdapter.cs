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

namespace com.ingenious.android
{
    class AccountListAdapter : BaseAdapter
    {

        List<Account> accountList = getDataForListView();
        Context c;

        public AccountListAdapter(Context c)
        {
            this.c = c;
        }

        public override int Count
        {
            get {
					return accountList.Count;
				}
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)c.GetSystemService(Context.LayoutInflaterService);
            convertView = inflater.Inflate(Resource.Layout.accountview, parent, false);

            TextView nameTxt = convertView.FindViewById<TextView>(Resource.Id.nameTxt);
            TextView ibanTxt = convertView.FindViewById<TextView>(Resource.Id.ibanTxt);
            TextView ownerTxt = convertView.FindViewById<TextView>(Resource.Id.ownerTxt);
            TextView amountTxt = convertView.FindViewById<TextView>(Resource.Id.amountTxt);


            Account account = accountList[position];

            nameTxt.Text = account.name;
            ibanTxt.Text = account.iban;
            ownerTxt.Text = account.owner;
            amountTxt.Text = account.balance.ToString();

            return convertView;
        }

        public static List<Account> getDataForListView()
        {
            Account testAccount = new Account("Account name 1", "Luis Garavito", "BE58290017912079", "GEBABEBB", new Decimal(412.20), "EUR");
            Account testAccount2 = new Account("2nd account", "Pedro Lopez", "BE58290017912079", "GEBABEBB", new Decimal(323236.02), "EUR");

            List<Account> accountListItemList = new List<Account>(2);
            accountListItemList.Add(testAccount);
            accountListItemList.Add(testAccount2);

            return accountListItemList;
        }

    }
}