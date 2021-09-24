using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fabryka_wzorzec
{
    class Program
    { }

        enum OS_TYPE
        {
            Windows,
            OsX
        }

        // Przykład GUIFactory 

        abstract class GUIFactory
        {
            /// <summary>
            /// getFactory returns concrete Factory,
            ///instead parameter abstract Factory can get OS_Type from outer method
            /// </summary>
            /// <param name="type">Operating System</param>
            /// <returns></returns>
            public static GUIFactory getFactory(OS_TYPE type)
            {
                switch (type)
                {
                    case OS_TYPE.Windows:
                        return new WinFactory();

                    case OS_TYPE.OsX:
                        return new OSXFactory();
                    default:
                        throw new NotImplementedException();
                }
            }

            public abstract Button createButton();
        }

        class WinFactory : GUIFactory
        {
            public override Button createButton()
            {
                return new WinButton();
            }
        }

        class OSXFactory : GUIFactory
        {
            public override Button createButton()
            {
                return new OSXButton();
            }
        }

        abstract class Button
        {
            public abstract void paint();
        }

        class WinButton : Button
        {
            public override void paint()
            {
                Console.WriteLine("Przycisk WinButton");
            }
        }

        class OSXButton : Button
        {
            public override void paint()
            {
                Console.WriteLine("Przycisk OSXButton");
            }
        }

        public class Application
        {
            public static void Main(String[] args)
            {
                GUIFactory factory = GUIFactory.getFactory(OS_TYPE.OsX);
                Button button = factory.createButton();
                button.paint();
                Console.ReadLine();
            }
            // Wyświetlony zostanie tekst:
            //   "Przycisk WinButton"
            // lub:
            //   "Przycisk OSXButton"
        }
     
    }

