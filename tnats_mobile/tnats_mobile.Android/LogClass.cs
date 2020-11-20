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
        /// <summary>
        /// CONSTRUCTOR CLASS
        /// </summary>
        public LogClass() { }

        /// <summary>
        /// METHOD THAT INSERT THE LOG AS INFO TYPE
        /// </summary>
        /// <param name="tag">REFERENCE</param>
        /// <param name="message">LOG MESSAGE</param>
        public void AddInfo(string tag, string message)
        {
            Log.Info(tag, message);
        }

        /// <summary>
        /// METHOD THAT INSERT THE LOG AS ERROR TYPE
        /// </summary>
        /// <param name="tag">REFERENCE</param>
        /// <param name="message">LOG MESSAGE</param>
        public void AddError(string tag, string message)
        {
            Log.Error(tag, message);
        }

        /// <summary>
        /// METHOD THAT INSERT THE LOG AS WARNING TYPE
        /// </summary>
        /// <param name="tag">REFERENCE</param>
        /// <param name="message">LOG MESSAGE</param>
        public void AddWarning(string tag, string message)
        {
            Log.Warn(tag, message);
        }

        /// <summary>
        /// METHOD THAT INSERT THE LOG AS DEBUG TYPE
        /// </summary>
        /// <param name="tag">REFERENCE</param>
        /// <param name="message">LOG MESSAGE</param>
        public void AddDebug(string tag, string message)
        {
            Log.Debug(tag, message);
        }
    }
}