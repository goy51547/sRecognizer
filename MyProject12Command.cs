using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using System.Speech;
using System.Speech.Recognition;

namespace MyProject12
{
    [System.Runtime.InteropServices.Guid("1da357d0-27c3-4774-95fb-566bdab2e6a7")]
    public class MyProject12Command : Command
    {
        public MyProject12Command()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static MyProject12Command Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "sRecognizer"; }
        }

        static bool commandc = false;
        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("Recognizer starts running.", EnglishName);

            if (mode == RunMode.Interactive)
            {
                SpeechRecognizer recognizer = new SpeechRecognizer();
                Choices bChoices = new Choices(new GrammarBuilder[] { "command", "move", "rectangle", "circle", "text", "box", "group" });
                bChoices.Add(new GrammarBuilder[] { "point", "poly line", "arc", "split" ,"polygon","copy"});
                Grammar CoGrammar = new Grammar((GrammarBuilder)bChoices);
                CoGrammar.Name = "commands";
                recognizer.LoadGrammar(CoGrammar);
                recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognizedHandler);
            }
            return Result.Success;
        }
        static void SpeechRecognizedHandler(object sender, SpeechRecognizedEventArgs e)
        {
            String result = e.Result.Text.ToString();
            if (commandc)
            {
                if (IsValidCommandName(result))
                {
                    RhinoApp.RunScript(result, false);
                }
                commandc = false;
                return;
            }
            else if (result == "command")
            {
                commandc = true;
                return;
            }

            else return;
        }
    }
}
