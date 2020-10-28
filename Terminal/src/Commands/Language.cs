using CommandLine;
using Terminal.Properties;
using System;
using Utilities;
using System.Globalization;

namespace Command
{
    /// <summary>
    /// This command changes the language.
    /// </summary>
    [Verb("lang", HelpText = "cmdLang", ResourceType = typeof(Localization))]
    class BaseLanguage : BaseCommand
    {
        [Value(0, MetaName = "locale", HelpText = "langLocale", Required = true, ResourceType = typeof(Localization))]
        public string Locale { get; set; }

        public override void Execute()
        {
            base.Execute();
            try
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Locale);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Locale);
            }
            catch (CultureNotFoundException e)
            {
                Logger.PrintError(Localization.langLocaleNotSupported, e.InvalidCultureName);
                OnError();
            }
            catch (Exception e)
            {
                Logger.PrintError(e.Message);
                OnError();
            }
        }
    }

    class QLanguage : BaseLanguage, IQuiteable
    {
        public bool Quite { get; set; }
    }

    class NLanguage : BaseLanguage, INotQuiteable
    {
        public bool Quite { get => false; set => throw new NotImplementedException(); }
    }
}
