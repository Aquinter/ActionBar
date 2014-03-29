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
    class InfoListAdapter : BaseAdapter
    {
        List<InfoListItem> infoListItemList = getDataForListView();
        Context c;

        public InfoListAdapter(Context c)
        {
            this.c = c;
        }

        public override int Count
        {
            get {
					return infoListItemList.Count;
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
            //if(position==0)
            //{
                LayoutInflater inflater = (LayoutInflater)c.GetSystemService(Context.LayoutInflaterService);
                convertView = inflater.Inflate(Resource.Layout.ListItem, parent,false);
            //}

            TextView mainInfoText = convertView.FindViewById<TextView>(Resource.Id.text1);
            TextView extraInfoText = convertView.FindViewById<TextView>(Resource.Id.text2);
            ImageView iconView = convertView.FindViewById<ImageView>(Resource.Id.imageView1);

            InfoListItem item = infoListItemList[position];

            mainInfoText.Text = item.mainInfo;
            extraInfoText.Text = item.extraInfo;
            iconView.SetImageResource(item.imgSource);

            convertView.FindViewById(Resource.Id.primary_target).Click += delegate
            {
                Toast.MakeText(c, Resource.String.touched_primary_message, ToastLength.Short).Show();
                TextView infoTxt = convertView.FindViewById<TextView>(Resource.Id.text2);

                if (infoTxt.Text.StartsWith("+"))
                {
                    //start phone client
                    Intent intent = new Intent(Intent.ActionDial);
                    intent.SetData(Android.Net.Uri.Parse("tel:"+infoTxt.Text));
                    c.StartActivity(intent); 
                }
                else if (infoTxt.Text.StartsWith("ING Belgium"))
                {
                    String url = "http://www.facebook.com/ingbanquebelgique";
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse(url));
                    c.StartActivity(intent);
                }
                else if (infoTxt.Text.StartsWith("@INGBelgique"))
                {
                    String url = "http://www.twitter.com/INGBelgique";
                    Intent intent = new Intent(Intent.ActionView);
                    intent.SetData(Android.Net.Uri.Parse(url));
                    c.StartActivity(intent);
                }
                else
                {
                    //start mail client
                    Intent intent = new Intent(Intent.ActionSend);
                    intent.SetType("plain/text");
                    intent.PutExtra(Intent.ExtraEmail, new String[] { infoTxt.Text });
                    intent.PutExtra(Intent.ExtraSubject, "subject");
                    intent.PutExtra(Intent.ExtraText, "mail body");
                    c.StartActivity(Intent.CreateChooser(intent, ""));
                }
            };            

            return convertView;
        }

        public static List<InfoListItem> getDataForListView()
        {
            List<InfoListItem> infoListItemList = new List<InfoListItem>(6);
            infoListItemList.Add(new InfoListItem("General help or advice", "+3224646004", Resource.Drawable.ic_action_call));
            infoListItemList.Add(new InfoListItem("Mail ING Belgium", "info@ing.be", Resource.Drawable.ic_action_email));
            infoListItemList.Add(new InfoListItem("Facebook", "ING Belgium", Resource.Drawable.ic_action_facebook));
            infoListItemList.Add(new InfoListItem("Twitter", "@INGBelgique", Resource.Drawable.ic_action_twitter));
            infoListItemList.Add(new InfoListItem("Card Stop", "+3270344344", Resource.Drawable.ic_action_call));
            infoListItemList.Add(new InfoListItem("Claims or accidents", "+3225500600", Resource.Drawable.ic_action_call));

            return infoListItemList;
        }
    }
}