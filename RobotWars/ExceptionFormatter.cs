///<summary>
/// Author: DJ van Wyk
/// Date: July 2010
/// Copyright: Nobody, because I have been shamelessly copying the same to most companies I have worked for
/// 
/// Pretty formatting for exceptions
///</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
namespace RobotWars
{
    public static class ExceptionFormatter
    {
        private static string GetInnerExceptions(Exception ex)
        {
            string ExceptionMessage = String.Format("/*Inner Exception" + Environment.NewLine +
                "Datetime: " + Environment.NewLine + "\t{0}" + Environment.NewLine +
                "Workstation: " + Environment.NewLine + "\t{1}" + Environment.NewLine +
                "Windows User: " + Environment.NewLine + "\t{3}" + Environment.NewLine +
                "Application User: " + Environment.NewLine + "\t{4}" + Environment.NewLine +
                "Exception Type: " + Environment.NewLine + "\t{6}" + Environment.NewLine +
                "Exception Message: " + Environment.NewLine + "\t{2}" + Environment.NewLine +
                "Source: " + Environment.NewLine + "\t{7}" + Environment.NewLine +
                "Target Site: " + Environment.NewLine + "\t{8}" + Environment.NewLine +
                "StackTrace: " + Environment.NewLine + "{5}" + Environment.NewLine,
                DateTime.Now.ToString("dd-MMM-yyyy H:mm:ss.fff"),
                Environment.MachineName,
                ex.Message,
                Environment.UserDomainName + "\\" + Environment.UserName,
                System.Threading.Thread.CurrentPrincipal.Identity.Name,
                ex.StackTrace,
                ex.GetType().ToString(),
                ex.Source,
                ex.TargetSite);

            foreach (System.Collections.DictionaryEntry dictItem in ex.Data)
            {
                ExceptionMessage = String.Concat(ExceptionMessage, String.Format("\nInner Exception Additional Data:\n\tKey: {0}\n\tValue: {1}\n", dictItem.Key, dictItem.Value));
            }

            if (ex.InnerException != null)
            {
                ExceptionMessage = String.Concat(ExceptionMessage, GetInnerExceptions(ex.InnerException));
            }
            return ExceptionMessage;
        }

        public static string FormatException(Exception ex)
        {
            string version = "?.?.?.?";
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in assemblies)
            {
                if (asm.GetName().Name.ToLower() == "orderwise.oms.console")
                {
                    version = String.Format("{0}.{1}.{2}.{3}",
                        Convert.ToString(asm.GetName().Version.Major),
                        Convert.ToString(asm.GetName().Version.Minor),
                        Convert.ToString(asm.GetName().Version.Build),
                        Convert.ToString(asm.GetName().Version.Revision));
                    break;
                } // if
            } // foreach 

            if (version == "?.?.?.?")
            {
                Assembly caller = Assembly.GetCallingAssembly();
                if (caller != null)
                    version = String.Format("{0}.{1}.{2}.{3}",
                        Convert.ToString(caller.GetName().Version.Major),
                        Convert.ToString(caller.GetName().Version.Minor),
                        Convert.ToString(caller.GetName().Version.Build),
                        Convert.ToString(caller.GetName().Version.Revision));
            } // if  

            string ExceptionMessage = String.Format(Environment.NewLine + "/*---Start-------------" + Environment.NewLine +
                "Datetime: " + Environment.NewLine +
                "\t{0}" + Environment.NewLine +
                "Workstation: " + Environment.NewLine +
                "\t{1}" + Environment.NewLine +
                "Windows User: " + Environment.NewLine +
                "\t{2}" + Environment.NewLine +
                "Application User: " + Environment.NewLine +
                "\t{3}" + Environment.NewLine +
                "Version: " + Environment.NewLine +
                "\t{4}" + Environment.NewLine +
                "Exception Type: " + Environment.NewLine +
                "\t{5}" + Environment.NewLine +
                "Exception Message: " + Environment.NewLine +
                "\t{6}" + Environment.NewLine +
                "Source: " + Environment.NewLine +
                "\t{7}" + Environment.NewLine +
                "Target Site: " + Environment.NewLine +
                "\t{8}" + Environment.NewLine +
                "StackTrace: " + Environment.NewLine +
                "{9}" + Environment.NewLine,
                DateTime.Now.ToString("dd-MMM-yyyy H:mm:ss.fff"),
                Environment.MachineName,
                Environment.UserDomainName + "\\" + Environment.UserName,
                System.Threading.Thread.CurrentPrincipal.Identity.Name,
                version,
                ex.GetType().ToString(),
                ex.Message,
                ex.Source == null ? "Unavailable" : ex.Source,
                ex.TargetSite == null ? "Unavailable" : ex.TargetSite.Name,
                ex.StackTrace == null ? "Unavailable" : ex.StackTrace);

            if (ex.InnerException != null)
            {
                ExceptionMessage = String.Concat(ExceptionMessage, GetInnerExceptions(ex.InnerException));
            }

            foreach (System.Collections.DictionaryEntry dictItem in ex.Data)
            {
                ExceptionMessage = String.Concat(ExceptionMessage, String.Format(Environment.NewLine + "Additional Data:" + Environment.NewLine + "\tKey: {0}" + Environment.NewLine + "\tValue: {1}" + Environment.NewLine, dictItem.Key, dictItem.Value));
            }

            ExceptionMessage = String.Concat(ExceptionMessage, Environment.NewLine + "-----End---------------*/" + Environment.NewLine);
            return ExceptionMessage;
        }

        public static string FormatExceptionMessage(Exception exception)
        {
            string ExceptionMessage = exception.Message;

            if (exception.InnerException != null)
            {
                ExceptionMessage = String.Concat(ExceptionMessage, Environment.NewLine, "\t", FormatExceptionMessage(exception.InnerException));
            }

            return ExceptionMessage;
        }
    }
}