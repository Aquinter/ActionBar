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
    class SettingsListAdapter : BaseAdapter
    {
        List<InfoListItem> infoListItemList = getDataForListView();
        Context c;

        public SettingsListAdapter(Context c)
        {
            this.c = c;
        }

        public override int Count
        {
            get
            {
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
            convertView = inflater.Inflate(Resource.Layout.ListItem, parent, false);
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
                AlertDialog.Builder builder = new AlertDialog.Builder(this.c);
                AlertDialog alertDialog = builder.Create();
                alertDialog.SetTitle(mainInfoText.Text);
                alertDialog.SetMessage("To be implemented");
                alertDialog.SetIcon(Android.Resource.Drawable.IcDialogAlert);
                alertDialog.SetButton("OK", (s2, ev2) =>
                {
                    //do something
                });
                alertDialog.Show();
            };

            return convertView;
        }

        public static List<InfoListItem> getDataForListView()
        {
            List<InfoListItem> infoListItemList = new List<InfoListItem>(3);
            infoListItemList.Add(new InfoListItem("Settings test", "subtext 1", 0));
            infoListItemList.Add(new InfoListItem("2nd setting", "subtext 2", 0));
            infoListItemList.Add(new InfoListItem("Another test", "subtext 3", 0));

            return infoListItemList;
        }
    }
}