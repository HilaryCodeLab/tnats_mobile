using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using tnats_mobile.Droid;
using tnats_mobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(LogClass))]
namespace tnats_mobile.Droid
{
    public class LogClass : ILogClass
    {

        public LogClass() { }

        public void AddInfo(string tag, string message)
        {
            Log.Info(tag, message);
        }
        public void AddError(string tag, string message)
        {
            Log.Error(tag, message);
        }
        public void AddWarning(string tag, string message)
        {
            Log.Warn(tag, message);
        }
        public void AddDebug(string tag, string message)
        {
            Log.Debug(tag, message);
        }
    }
}