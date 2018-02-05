using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Code_Generator
{
    class QRTemplate
    {
        private QRModule[,] template;
        private int baseD = 21;
        private int templateD;
        public QRTemplate(int version)
        {
            templateD = baseD+((version - 1) * 4);
            template = new QRModule[templateD, templateD];

            for(int row = 0; row<templateD; row++)
            {
                for (int column = 0; column < templateD; column++)
                {
                    template[row,column] = new QRModule();
                }
            }
            Console.WriteLine(templateD);
        }
    }

    class QRModule
    {
        public bool isAvailabe;
        public QRModule()
        {
            this.isAvailabe = false;
        }
    }
}
