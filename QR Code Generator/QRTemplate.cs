using System;
using System.Collections.Generic;
using System.Drawing;
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
        private byte[,] finderPattern;
        public QRTemplate(int version)
        {
             
            finderPattern = new byte[7,7];
            for (int i = 0; i < 7; i++) {
                finderPattern[0, i] = 0;
                finderPattern[6, i] = 0;
                finderPattern[i, 0] = 0;
                finderPattern[i, 6] = 0;
            }
            for (int i = 1; i <= 5; i++)
            {
                finderPattern[1, i] = 255;
                finderPattern[5, i] = 255;
                finderPattern[i, 1] = 255;
                finderPattern[i, 5] = 255;
            }

            
           

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
        public void save(String url)
        {
            Bitmap b = new Bitmap(templateD, templateD);
            for (int row = 0; row < templateD; row++)
            {
                for (int col = 0; col < templateD; col++)
                {
                    byte c = template[col, row].value;
                    b.SetPixel(row, col, Color.FromArgb(255, c, c, c));
                }

            }
            b.Save(url);
        }
    }

    class QRModule
    {
        public bool isAvailabe;
        public byte value;
        public QRModule()
        {
            this.isAvailabe = false;
            this.value = 0;
        }
    }
}
