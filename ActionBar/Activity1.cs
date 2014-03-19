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

namespace ActionBar
{
    [Activity(Label = "ActionBar", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        private DrawerLayout drawerLayout;
        private MyActionBarDrawerToggle drawerToggle;
        private ListView drawerListView;
        private String[] drawerList = { "Mobile Banking", "Branches", "Contact", "Appointment", "Info" };
        private Queue<Payment> queue = new Queue<Payment>();
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
            AddTabToActionBar(Resource.String.title_section2);
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
                    LayoutInflater.Inflate(Resource.Layout.accounts, frame);
                    break;
                case 1:
                    LayoutInflater.Inflate(Resource.Layout.credits, frame);
                    break;
                case 2:
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
                TextView[] txt = new TextView[8];
                String[] txtStr = new String[12];
                txt[0] = FindViewById<TextView>(Resource.Id.duedate);
                txt[1] = FindViewById<TextView>(Resource.Id.amount);
                txt[2] = FindViewById<TextView>(Resource.Id.firmname);
                txt[3] = FindViewById<TextView>(Resource.Id.firmaddress1);
                txt[4] = FindViewById<TextView>(Resource.Id.firmaddress2);
                txt[5] = FindViewById<TextView>(Resource.Id.iban);
                txt[6] = FindViewById<TextView>(Resource.Id.bic);
                txt[7] = FindViewById<TextView>(Resource.Id.reference);

                reader.ReadToFollowing("payment");
                reader.MoveToFirstAttribute();      //duedate
                txt[0].Text = reader.Value;
                txtStr[0] = reader.Value;
                reader.MoveToNextAttribute();       //currency
                txt[1].Text = reader.Value;
                txtStr[1] = reader.Value;
                reader.MoveToNextAttribute();       //amount
                txt[1].Text += " " + reader.Value;
                txtStr[2] = reader.Value;
                reader.MoveToNextAttribute();       //firmname
                txt[2].Text = reader.Value;
                txtStr[3] = reader.Value;
                reader.MoveToNextAttribute();       //firm address (street)
                txt[3].Text = reader.Value;
                txtStr[4] = reader.Value;
                reader.MoveToNextAttribute();       //firm address (number)
                txt[3].Text += " " + reader.Value;
                txtStr[5] = reader.Value;
                reader.MoveToNextAttribute();       //firm zipcode
                txt[4].Text = reader.Value;
                txtStr[6] = reader.Value;
                reader.MoveToNextAttribute();       //firm city
                txt[4].Text += " " + reader.Value;
                txtStr[7] = reader.Value;
                reader.MoveToNextAttribute();       //iban
                txt[5].Text = reader.Value;
                txtStr[8] = reader.Value;
                reader.MoveToNextAttribute();       //bic
                txt[6].Text = reader.Value;
                txtStr[9] = reader.Value;
                reader.MoveToNextAttribute();       //reference
                txt[7].Text = reader.Value;
                txtStr[10] = reader.Value;
                reader.MoveToNextAttribute();
                txtStr[11] = reader.Value;

                Payment newPayment = new Payment();

                try
                {
                    newPayment = new Payment(Convert.ToDateTime(txtStr[0], new CultureInfo("ru-RU")), txtStr[1], Convert.ToDecimal(txtStr[2]), txtStr[3], txtStr[4] + " " + txtStr[5] + " " + txtStr[6] + " " + txtStr[7], txtStr[8], txtStr[9], txtStr[10], txtStr[11]);
                }
                catch (Exception e)
                {
                    Toast.MakeText(this, e.Message, ToastLength.Long).Show();
                }

                queue.Enqueue(newPayment);
            }
        }

        public void toastQueue()
        {
            String str = "";
            int queueCount = queue.Count;

            str += "Items: " + queueCount + "\n";
            str += "Last item:\n";
            str += "Firm name: " + queue.Peek().firmName + "\n";
            str += "Amount: " + queue.Peek().currency + " " + queue.Peek().amount + "\n";
            str += "Duedate: " + queue.Peek().dueDate.ToString();

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
                    LayoutInflater.Inflate(Resource.Layout.branches, frame);
                    removeAllTabs();
                    break;
                case 2:
                    LayoutInflater.Inflate(Resource.Layout.contact, frame);
                    removeAllTabs();
                    break;
                case 3:
                    LayoutInflater.Inflate(Resource.Layout.appointment, frame);
                    removeAllTabs();
                    break;
                case 4:
                    LayoutInflater.Inflate(Resource.Layout.List, frame);
                    removeAllTabs();
                    ListView contentListView = FindViewById<ListView>(Resource.Id.activity_list);
                    contentListView.Adapter = new InfoListAdapter(this);
                    break;
                default:
                    break;
            }
            
            //LayoutInflater.Inflate(res, frame);
            

            drawerListView.SetItemChecked(position, true);
            drawerLayout.CloseDrawer(drawerListView);
        }
    }
}

