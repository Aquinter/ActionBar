﻿using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V4.App;
using Android.Util;
using DrawerSample;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.Graphics;
//barcode
using ZXing;
using ZXing.Mobile;
//xml parsing
using System.Xml;
using System.IO;
//queue
using System.Collections.Generic;
using System.Globalization;
using IngHackaton;
using Android.Content.PM;
using Android.Telephony;
using Java.Util;

using Aping;

namespace com.ingenious.android
{
    [Activity(Label = "ING Mobile Banking", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Activity1 : Activity
    {
        private DrawerLayout drawerLayout;
        private MyActionBarDrawerToggle drawerToggle;
        private ListView drawerListView;
        private String[] drawerList = { "Mobile Banking", "Info", "Settings" };
        private List<Payment> paymentList = new List<Payment>();
        private ApIng myAping = new ApIng("DA02vQ9zQJTy0aDnSp0Do2mc8LTY8o1a", "2904561Y", "1/01/1980");

        /*
         * OnCreate is called when app is started (equivalent to main in ordinary C#)
         */
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            
            // Set drawer adapter to create list from drawerList array
            drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawerListView = FindViewById<ListView>(Resource.Id.left_drawer);
            drawerListView.Adapter = new ArrayAdapter<string>(this, Resource.Layout.DrawerListItem, drawerList);
            drawerListView.ItemClick += (sender, args) => SelectItem(args.Position);

            drawerToggle = new MyActionBarDrawerToggle(this, drawerLayout, Resource.Drawable.ic_drawer, Resource.String.drawer_open, Resource.String.drawer_close);
            drawerToggle.DrawerClosed += delegate
            {
                InvalidateOptionsMenu();
            };
            drawerToggle.DrawerOpened += delegate
            {
                InvalidateOptionsMenu();
            };

            drawerLayout.SetDrawerListener(drawerToggle);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            FrameLayout frame = FindViewById<FrameLayout>(Resource.Id.content_frame);
            frame.RemoveAllViews();
            LayoutInflater.Inflate(Resource.Layout.mobilebanking, frame);

            addMobileBankingTabs();
        }

        public void addMobileBankingTabs()
        {
            ActionBar.RemoveAllTabs();

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            AddTabToActionBar(Resource.String.title_section1);
            AddTabToActionBar(Resource.String.title_section3);
        }

        public void removeAllTabs()
        {
            ActionBar.RemoveAllTabs();

            ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
        }

        void AddTabToActionBar(int labelResourceId)
        {
            Android.App.ActionBar.Tab tab = ActionBar.NewTab()
                                         .SetText(labelResourceId);
            tab.TabSelected += TabOnTabSelected;
            ActionBar.AddTab(tab);
        }

        void TabOnTabSelected(object sender, Android.App.ActionBar.TabEventArgs tabEventArgs)
        {
            Android.App.ActionBar.Tab tab = (Android.App.ActionBar.Tab)sender;
            Toast.MakeText(this, String.Format("The tab {0} has been selected.", tab.Text), ToastLength.Short).Show();

            FrameLayout frame = FindViewById<FrameLayout>(Resource.Id.content_frame);
            frame.RemoveAllViews();

            switch (tab.Position)
            {
                case 0:
                    //LayoutInflater.Inflate(Resource.Layout.accounts, frame);
                    LayoutInflater.Inflate(Resource.Layout.List, frame);
                    //removeAllTabs();
                    ListView contentListView = FindViewById<ListView>(Resource.Id.activity_list);
                    contentListView.Adapter = new AccountListAdapter(this);
                    break;
                case 1:
                    LayoutInflater.Inflate(Resource.Layout.transfers, frame);
                    Button btn = FindViewById<Button>(Resource.Id.button1);
                    btn.Click += async delegate
                    {
                        MobileBarcodeScanner scanner = new MobileBarcodeScanner(this);
                        var result = await scanner.Scan();
                        readXML(result.Text);
                    };
                    Button btn2 = FindViewById<Button>(Resource.Id.button2);
                    btn2.Click += delegate
                    {
                        toastQueue();
                    };
                    break;
                default:
                    break;
            }
        }

        public void readXML(string xmlString)
        {
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {

                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Confirm payment?");
                LayoutInflater inflater = this.LayoutInflater;
                View view = inflater.Inflate(Resource.Layout.PaymentDialog, null);

                
                /*
                TextView[] txt = new TextView[8];
                txt[0] = view.FindViewById<TextView>(Resource.Id.duedate);
                txt[1] = view.FindViewById<TextView>(Resource.Id.amount);
                txt[2] = view.FindViewById<TextView>(Resource.Id.firmname);
                txt[3] = view.FindViewById<TextView>(Resource.Id.firmaddress1);
                txt[4] = view.FindViewById<TextView>(Resource.Id.firmaddress2);
                txt[5] = view.FindViewById<TextView>(Resource.Id.iban);
                txt[6] = view.FindViewById<TextView>(Resource.Id.bic);
                txt[7] = view.FindViewById<TextView>(Resource.Id.reference);
                */
                Dictionary<String, String> paymentAttribs = new Dictionary<String, String>(20);

                reader.ReadToFollowing("payment");

                for (int i = 0; i < reader.AttributeCount; i++)
                {
                    reader.MoveToNextAttribute();
                    paymentAttribs.Add(reader.Name, reader.Value);
                }

                Payment newPayment = parseXmlToPayment(paymentAttribs);
                /*
                if (newPayment.amount > 1000)
                {
                    showLargePaymentPopUp(newPayment);
                }
                else
                {
                    paymentList.Add(newPayment);
                }
                */
                builder.SetView(view)
                    .SetPositiveButton("OK", (s1, ev) =>
                    {
                        paymentList.Add(newPayment);

                        // execute payment

                        MoneyTransfer toTransfer = new MoneyTransfer();
                        toTransfer.from = new Aping.From { productNumber = "14650100911708338200" };
                        toTransfer.to = new Aping.To { productNumber = newPayment.iban, titular = newPayment.firmName };
                        toTransfer.currency = newPayment.currency;
                        toTransfer.operationDate = newPayment.dueDateStr;
                        toTransfer.concept = newPayment.reference;
                        toTransfer.amount = (double)newPayment.amount;

                        ConfirmationOfTransfer fullPayment = myAping.EasyTransfer(toTransfer, "1,1");

                        myAping.LogOut();
                        //end payment execution


                    })
                    .SetNegativeButton("Cancel", (s1, ev) =>
                    {

                    });

                if (newPayment.amount <= 0.0m)
                {

                    AlertDialog.Builder builder3 = new AlertDialog.Builder(this);
                    builder3.SetTitle("Enter amount");
                    LayoutInflater inflater3 = this.LayoutInflater;
                    View view3 = inflater.Inflate(Resource.Layout.setamount, null);

                    builder3.SetView(view3)
                    .SetPositiveButton("OK", (s3, ev3) =>
                    {
                        newPayment.amount = Convert.ToDecimal(view3.FindViewById<EditText>(Resource.Id.amountValue).Text);
                        //paymentList.Add(newPayment);
                        setAmountInfo(view, newPayment);
                        builder.Show();
                    })
                    .SetNegativeButton("Cancel", (s1, ev) =>
                    {

                    });

                    builder3.Show();

                }
                else
                {
                    setAmountInfo(view, newPayment);
                    builder.Show();
                }

                
                
                /*
                txt[0].Text = newPayment.dueDate.ToString();
                txt[1].Text = newPayment.currency + " " + newPayment.amount.ToString();
                txt[2].Text = newPayment.firmName;
                txt[3].Text = newPayment.address.toString();
                txt[4].Text = "";
                txt[5].Text = newPayment.iban;
                txt[6].Text = newPayment.bic;
                txt[7].Text = newPayment.reference;
                 * */
                //DeviceId devID = new DeviceId();
                //view.FindViewById<TextView>(Resource.Id.textView31).Text = devID.getDeviceId();
                //txt[8].Text = devID.getDeviceId();
            }
        }

        public void setAmountInfo(View view, Payment newPayment)
        {
            view.FindViewById<TextView>(Resource.Id.duedate).Text = newPayment.dueDate.ToString();
            view.FindViewById<TextView>(Resource.Id.amount).Text = newPayment.currency + " " + newPayment.amount.ToString();
            view.FindViewById<TextView>(Resource.Id.firmname).Text = newPayment.firmName;
            view.FindViewById<TextView>(Resource.Id.firmaddress1).Text = newPayment.address.toString();
            view.FindViewById<TextView>(Resource.Id.firmaddress2).Text = "";
            view.FindViewById<TextView>(Resource.Id.iban).Text = newPayment.iban;
            view.FindViewById<TextView>(Resource.Id.bic).Text = newPayment.bic;
            view.FindViewById<TextView>(Resource.Id.reference).Text = newPayment.reference;

            //view.FindViewById<TextView>(Resource.Id.textView31).Text = ((TelephonyManager)this.GetSystemService(TelephonyService)).DeviceId.ToString();
        }

        private Payment parseXmlToPayment(Dictionary<String, String> list)
        {
            string dueDate = "";
            string currency = "";
            string amount = "";
            string firmName = "";
            string firmStreet = "";
            string firmNumber = "";
            string firmCity = "";
            string firmZipCode = "";
            string firmCountry = "";
            string firmBus = "";
            string iban = "";
            string bic = "";
            string reference = "";
            string referenceType = "";
            string id = "";

            list.TryGetValue("duedate", out dueDate);
            list.TryGetValue("currency", out currency);
            list.TryGetValue("amount", out amount);
            list.TryGetValue("name", out firmName);
            list.TryGetValue("street", out firmStreet);
            list.TryGetValue("number", out firmNumber);
            list.TryGetValue("bus", out firmBus);
            list.TryGetValue("zip", out firmZipCode);
            list.TryGetValue("city", out firmCity);
            list.TryGetValue("country", out firmCountry);
            list.TryGetValue("iban", out iban);
            list.TryGetValue("bic", out bic);
            list.TryGetValue("ref", out reference);
            list.TryGetValue("type", out referenceType);
            list.TryGetValue("id", out id);
            
            return new Payment(Convert.ToDateTime(dueDate, new CultureInfo("ru-RU")), currency, Convert.ToDecimal(amount), firmName, new IngHackaton.Address(firmStreet, Convert.ToInt32(firmNumber), Convert.ToInt32(firmBus), Convert.ToInt32(firmZipCode), firmCity, firmCountry),iban, bic, reference, referenceType);
        }

        private void showLargePaymentPopUp(Payment payment)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            AlertDialog alertDialog = builder.Create();
            alertDialog.SetTitle("Warning");
            alertDialog.SetMessage("Are you sure you sure you want to enlist a payment of >€1000?");
            alertDialog.SetIcon(Android.Resource.Drawable.IcDialogInfo);
            alertDialog.SetButton("Yes", (s1, ev) =>
            {
                AlertDialog.Builder builder2 = new AlertDialog.Builder(this);
                AlertDialog alertDialog2 = builder.Create();
                alertDialog2.SetTitle("Payment enlisted");
                //alertDialog2.SetMessage(editTxt.Text);
                alertDialog2.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                alertDialog2.SetButton("OK", (s2, ev2) =>
                {
                    //do something
                });
                alertDialog2.Show();

                paymentList.Add(payment);
            });
            alertDialog.Show();
        }

        public void toastQueue()
        {
            String str = "";
            int listCount = paymentList.Count;
            Payment lastPayment = paymentList[listCount - 1];

            str += "Items: " + listCount + "; Last item:\n";
            str += "Firm name: " + lastPayment.firmName + "\n";
            str += "Amount: " + lastPayment.currency + " " + lastPayment.amount + "\n";
            str += "Duedate: " + lastPayment.dueDate.ToString();

            Toast.MakeText(this, str, ToastLength.Long).Show();
        }

        /*
         * Some needed overrides for the navigation drawer
         */

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            drawerToggle.OnConfigurationChanged(newConfig);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (drawerToggle.OnOptionsItemSelected(item))
                return true;
            
            return base.OnOptionsItemSelected(item);
        }

        /*
         * Called when a navigation drawer listitem is clicked
         */

        private void SelectItem(int position)
        {
            
            FrameLayout frame = FindViewById<FrameLayout>(Resource.Id.content_frame);
            frame.RemoveAllViews();

            switch (position)
            {
                case 0:
                    LayoutInflater.Inflate(Resource.Layout.mobilebanking, frame);
                    addMobileBankingTabs();
                    break;
                case 1:
                    LayoutInflater.Inflate(Resource.Layout.List, frame);
                    removeAllTabs();
                    ListView contentListView = FindViewById<ListView>(Resource.Id.activity_list);
                    contentListView.Adapter = new InfoListAdapter(this);
                    break;
                case 2:
                    LayoutInflater.Inflate(Resource.Layout.List, frame);
                    removeAllTabs();
                    ListView contentListView2 = FindViewById<ListView>(Resource.Id.activity_list);
                    contentListView2.Adapter = new SettingsListAdapter(this);
                    break;
                default:
                    break;
            }            

            drawerListView.SetItemChecked(position, true);
            drawerLayout.CloseDrawer(drawerListView);
        }
    }
}