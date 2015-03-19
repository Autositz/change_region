/*
 * Created by SharpDevelop.
 * User: Autositz
 * Date: 19/03/2015
 * Time: 02:35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Globalization;
using Microsoft.Win32;
using System.Threading;

namespace change_region
{
    class Program
    {
        static Keys oldkeys;
        
        public static void Main(string[] args)
        {
            string appid = "0"; // appid
            const int waittime = 30; // seconds to wait after launch
            oldkeys = new Keys();
            
            Console.WriteLine("Press number for app to launch");
            Console.WriteLine("1: Dark Souls (211420)");
            Console.WriteLine("0: Enter appid to launch");
            ConsoleKeyInfo keypress = Console.ReadKey(true);
            
            switch (keypress.Key.ToString()) {
                case "D1":
                    appid = "211420";
                    break;
                case "D0":
                    Console.Write("Please enter appid: ");
                    appid = Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("No selection made. Exiting...");
                    break;
            }
            
            if (appid != "0")
            {
                // change to english
                ChangeLocale("en");
                
                // launch dark souls
                System.Diagnostics.Process.Start("steam://run/" + appid);
                Console.WriteLine("App launched with steam://run/" + appid + " - waiting " + waittime + " seconds to undo changes");
                
                // wait one minute then set it back to normal
                Thread.Sleep((waittime * 1000));
                
                // back to normal
                ChangeLocale();
            }
            
            
        }
        
        private static bool ChangeLocale()
        {
            return ChangeLocale("");
        }
        
        private static bool ChangeLocale(string locale)
        {
            bool bRet = true;
            
            try {
                // open registry to change values
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true);
                
                // store old values
                if (locale != "") {
                    oldkeys.Locale = key.GetValue("Locale").ToString();
//                    oldkeys.LocaleName = key.GetValue("LocaleName").ToString();
//                    oldkeys.sLanguage = key.GetValue("sLanguage").ToString();
//                    oldkeys.iCountry = key.GetValue("iCountry").ToString();
                }
                
                switch (locale) {
                    case "en":
                        // England
                        key.SetValue("Locale", "00000809", RegistryValueKind.String);
//                        key.SetValue("LocaleName", "en-GB", RegistryValueKind.String);
//                        key.SetValue("sLanguage", "ENG", RegistryValueKind.String);
//                        key.SetValue("iCountry", "44", RegistryValueKind.String);
                        break;
                    default:
                        key.SetValue("Locale", oldkeys.Locale, RegistryValueKind.String);
                        // Deutschland
//                        key.SetValue("Locale", "00000407", RegistryValueKind.String);
//                        key.SetValue("LocaleName", "de-DE", RegistryValueKind.String);
//                        key.SetValue("sLanguage", "DEU", RegistryValueKind.String);
//                        key.SetValue("iCountry", "49", RegistryValueKind.String);
                        break;
                }
            } catch (Exception ex) {
                bRet = false;
                Console.WriteLine("ERROR: " + ex.Message);
            }
            
            return bRet;
        }
    }
    
    public class Keys
    {
        public string Locale { get; set; }
        public string LocaleName { get; set; }
        public string sLanguage { get; set; }
        public string iCountry { get; set; }
        
        public Keys()
        {
            
        }
    }
}