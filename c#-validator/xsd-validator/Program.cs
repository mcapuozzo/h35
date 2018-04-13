using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Windows.Forms;

namespace xsd_validator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            String escapeSequence = "";

            while (escapeSequence != "q")
            {
                XDocument xmlFile = new XDocument();
                XDocument xsdFile = new XDocument();
                string message = "";

                Console.WriteLine("Press enter to select an XML file for validation.");
                Console.ReadKey();

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Select XML File";
                ofd.FilterIndex = 1;
                ofd.RestoreDirectory = false;
                ofd.Multiselect = false;
                ofd.InitialDirectory = @"C:\\";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader myStream = new StreamReader(ofd.FileName))
                    {
                        string buffer = myStream.ReadToEnd();
                        xmlFile = XDocument.Parse(buffer);
                    }
                }
                else
                {
                    return;
                }

                Console.WriteLine("Press enter to select an XML Schema Definition file to validate with.");
                Console.ReadKey();

                ofd.Title = "Select XSD File";
                ofd.Filter = "XSD File |*.XSD";
                ofd.FilterIndex = 1;
                ofd.RestoreDirectory = false;
                ofd.Multiselect = false;
                ofd.InitialDirectory = @"C:\\";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.Schemas.Add("", ofd.FileName);
                        settings.ValidationType = ValidationType.Schema;

                        xmlFile.Validate(settings.Schemas, (o, e) =>
                        {
                            message = e.Message;
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The XSD is invalid.\n" + e.Message + "\n");
                        Console.ReadKey();
                        return;
                    }

                    if (message == "")
                    {
                        message = "success.";
                    }

                    Console.WriteLine("Validation result: " + message + "\n" +
                        "Type \"q\", then [Enter] to escape, or any other key to perform an additional check.");
                }

                escapeSequence = Console.ReadLine().ToString();
            }

            return;
        }
    }
}
