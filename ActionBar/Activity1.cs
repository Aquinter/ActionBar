using System;

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
        //private ApIng myAping = new ApIng("DA02vQ9zQJTy0aDnSp0Do2mc8LTY8o1a", "2904561Y", "1/01/1980");
        public MoneyTransfer toTransfer;
        private string pinCode = "1";

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
                        //toastQueue();
                        /*
                        MoneyTransfer toTransfer = new MoneyTransfer();
                        toTransfer.from = new Aping.From { productNumber = "14650100911708338200" };
                        toTransfer.to = new Aping.To { productNumber = "14650100932025956187", titular = "PEPE PEREZ PEREZ" };
                        toTransfer.currency = "EUR";
                        toTransfer.operationDate = "30/03/2014";
                        toTransfer.concept = "this is a first test";
                        toTransfer.amount = 100.5d;
                        //UpdateTransfer toTest6 = myAping.requestUpdateTransfer(ToTest5.id, toTransfer);
                        //ConfirmationOfTransfer toTest7 = myAping.requestConfirmationOfTransfer("1,1", ToTest5.id);
                        ConfirmationOfTransfer fullPayment = myAping.EasyTransfer(toTransfer, "1,1");
                        //MessageBox.Show(fullPayment.ToString());
                        Toast.MakeText(this, fullPayment.ToString(), ToastLength.Long).Show();
                        myAping.LogOut();
                         * */
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
                        //Comfirm payment dialog



                        //paymentList.Add(newPayment);

                        // execute payment

                        
                        toTransfer = new MoneyTransfer();
                        toTransfer.from = new Aping.From { productNumber = "14650100911708338200" };
                        toTransfer.to = new Aping.To { productNumber = newPayment.iban, titular = newPayment.firmName };
                        toTransfer.currency = newPayment.currency;
                        toTransfer.operationDate = "30/03/2014";
                        toTransfer.concept = newPayment.reference;
                        toTransfer.amount = (double)newPayment.amount;
                        
                        /*
                         * 
                         * PIN code confirmation
                         * 
                         */
                        
                        AlertDialog.Builder builder4 = new AlertDialog.Builder(this);
                        builder4.SetTitle("Enter PIN");
                        LayoutInflater inflater4 = this.LayoutInflater;
                        View view4 = inflater4.Inflate(Resource.Layout.setamount, null);

                        TextView nameTxt = view4.FindViewById<TextView>(Resource.Id.textView3);
                        EditText pinTxt = view4.FindViewById<EditText>(Resource.Id.amountValue);
                        nameTxt.Text = "Pin: ";

                        builder4.SetView(view4)
                        .SetPositiveButton("OK", (s4, ev4) =>
                        {
                            // Enter pin dialog

                            if (pinTxt.Text.Equals(pinCode))
                            {
                                int auth = performAuthentication();
                                bool performPayment = auth >= 0 && auth < 3;

                                try
                                {
                                    if (performPayment)
                                    {
                                        ApIng myAping = new ApIng("DA02vQ9zQJTy0aDnSp0Do2mc8LTY8o1a", "2904561Y", "1/01/1980");

                                        ConfirmationOfTransfer fullPayment = myAping.EasyTransfer(toTransfer, "1,1");
                                        Toast.MakeText(this, "performPayment: " + performPayment.ToString() + "\n" + fullPayment.ToString() + "\nAuth:" + auth.ToString(), ToastLength.Long).Show();
                                        myAping.LogOut();
                                    }
                                    else
                                        Toast.MakeText(this, "performPayment: " + performPayment.ToString() + "\n No payment done\nAuth:" + auth.ToString(), ToastLength.Long).Show();
                                     
                                }
                                catch (Exception e)
                                {
                                    Toast.MakeText(this, e.Message.ToString(), ToastLength.Long).Show();
                                }
                            }
                             
                        })
                        .SetNegativeButton("Cancel", (s5, ev5) =>
                        {

                        });

                        builder4.Show();
                        
                        
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
                        // Enter amount dialog

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
            }

            
        }

        public int performAuthentication()
        {
            /*-1 = undefined error
             * 0 = Home perimeter
             * 1 = Public perimeter, FR passed
             * 2 = Public perimeter, FR failed
             * 3 = Forbidden perimeter
             * 4 = perimeter error
             * 5 = no perimeters defined
             * 6 = PIN wrong
             */

            

            return checkPerimeter();
        }

        public int checkPerimeter()
        {
            /*-1 = undefined error
             * 0 = Home perimeter
             * 1 = Public perimeter, FR passed
             * 2 = Public perimeter, FR failed
             * 3 = Forbidden perimeter
             * 4 = perimeter error
             * 5 = no perimeters defined
             */

            double lat = 50.2058;
            double lon = 8.02521;

            Coordinate coord = new Coordinate(lat, lon);
            Perimeter perimeter = new Perimeter(coord, 200, 5000);

            int perimeterRating = perimeter.getZoneRating(coord); //should return 0;

            switch (perimeterRating)
            {
                case 0:
                    // Home perimeter
                    return 0;
                case 1:
                    // Public perimeter
                    return faceRecognition() ? 1 : 2;
                case 2:
                    // Forbidden perimeter
                    return 3;
                case 3:
                    // Perimeter error
                    return 4;
                case 4:
                    // No perimeter defined, send to settings, no payment
                    return 5;
                default:
                    return -1;
            }
        }

        public bool faceRecognition()
        {
            return true;
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