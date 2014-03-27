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
using Android.Graphics.Drawables;

namespace ActionBar
{
    class InfoListItem
    {
        public String mainInfo;
        public String extraInfo;
        public int imgSource;

        public InfoListItem(String main, String extra, int img)
        {
            this.mainInfo = main;
            this.extraInfo = extra;
            imgSource = img;
        }
    }
}