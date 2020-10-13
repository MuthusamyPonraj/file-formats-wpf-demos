#region Copyright Syncfusion Inc. 2001-2020.
// Copyright Syncfusion Inc. 2001-2020. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.Licensing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace syncfusion.demoscommon.wpf
{
    public static class AssemblyResolver
    {
        static AssemblyResolver()
        {

        }

        public static void Init()
        {
            ErrorLogging.ClearPreviousLogs();
            ErrorLogging.LogError("ErrorLogging Initialized");
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (!args.Name.Contains(".resources"))
            {
                if (args.Name.ToLower().StartsWith("syncfusion"))
                {
                    var assemblylocation = ConfigurationManager.AppSettings["assemblylocation"];
                    if (!string.IsNullOrEmpty(assemblylocation))
                    {
                        string assemblyName;
                        if (args.Name.Contains(","))
                        {
                            assemblyName = args.Name.Substring(0, args.Name.IndexOf(","));
                        }
                        else
                        {
                            assemblyName = args.Name;
                        }
                        assemblyName = assemblylocation + assemblyName + ".dll";
                        if (File.Exists(assemblyName))
                            return Assembly.LoadFrom(assemblyName);
                        ErrorLogging.LogError("Could not locate the assembly " + assemblyName + ", Please contact Syncfusion support.");
                    }
                    else
                    {
                        ErrorLogging.LogError("Assembly location details missing in app.config file, Please contact Syncfusion support." + args.Name);
                    }
                }
            }
            return null;
        }
    }

    public static class LicenseKeyLocator
    {
        public static void FindandRegisterLicenseKey()
        {
            SyncfusionLicenseProvider.RegisterLicense(FindLicenseKey());
        }

        /// <summary>
        /// Helper method to find a syncfusion license key.
        /// </summary>
        /// <returns>License Key</returns>
        private static string FindLicenseKey()
        {
            int levelsToCheck = 12;
            string filePath = @"SyncfusionLicense.txt";

            string rootPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().CodeBase.Replace(@"file:///", ""));

            for (int n = 0; n < levelsToCheck; n++)
            {
                string fileDataPath = System.IO.Path.Combine(rootPath, filePath);
                if (System.IO.File.Exists(fileDataPath))
                    return File.ReadAllText(fileDataPath, Encoding.UTF8);
                DirectoryInfo rootDirectory = Directory.GetParent(rootPath);
                if (rootDirectory == null)
                    break;
                rootPath = rootDirectory.FullName;
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// Logs the errors in ErrorLog.txt
    /// </summary>
    public static class ErrorLogging
    {
        /// <summary>
        /// Method to Clear previous logs
        /// </summary>
        internal static void ClearPreviousLogs()
        {
            string path = Directory.GetCurrentDirectory() + @"\Errorlog.txt";
            if (File.Exists(path))
                File.Delete(path);
        }

        /// <summary>
        /// Method to take care of error logging operations
        /// </summary>
        /// <param name="error"></param>
        public static void LogError(object error)
        {
            string path = Directory.GetCurrentDirectory() + @"\Errorlog.txt";
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            using (StreamWriter fileWriter = File.AppendText(path))
            {
                fileWriter.WriteLine($"{ DateTime.Now.ToLongTimeString()} :");
                fileWriter.Write(error.ToString() + "\n");
                fileWriter.Close();
            }
        }
    }
}
