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

            // Placing The Finder Pattern inside the template 
            int rowstart = 0, columnstart = 0;
            int finderrow = 0, findercolumn = 0;
            QRModule tmp = null;
            for (int row = rowstart; row < 7; row++)
            {
                for(int column=columnstart; column<7; column++)
                {
                    tmp = new QRModule();
                    tmp.isAvailabe = false;
                    tmp.value = finderPattern[finderrow, findercolumn];
                    
                    template[row, column] = tmp;
                    // adding Separator 
                    if (column==6)
                    {
                        tmp = new QRModule();
                        tmp.isAvailabe = false;
                        tmp.value = 255;
                        template[row, column + 1] = tmp;
                    }
                    if (row == 6)
                    {
                        tmp = new QRModule();
                        tmp.isAvailabe = false;
                        tmp.value = 255;
                        template[row+1, column] = tmp;
                    }
                    template[row + 1, column + 1] = tmp;
                    findercolumn++;
                }
                findercolumn = 0;
                finderrow++;
            }

            // ([(((V-1)*4)+21) - 7], 0)
            rowstart =  ((((2-1)*4)+21)-7) ; columnstart = 0;
            finderrow = 0; findercolumn = 0;

            for (int row = rowstart; (row- rowstart) < 7; row++)
            {
                
                for (int column = columnstart; column < 7; column++)
                {
                    tmp = new QRModule();
                    tmp.isAvailabe = false;
                    tmp.value = finderPattern[finderrow, findercolumn];
                    template[row, column] = tmp;
                    // adding Separator 
                    if (column == 6)
                    {
                        tmp = new QRModule();
                        tmp.isAvailabe = false;
                        tmp.value = 255;
                        template[row, column + 1] = tmp;
                    }
                    if ( (row - rowstart) == 0)
                    {
                        tmp = new QRModule();
                        tmp.isAvailabe = false;
                        tmp.value = 255;
                        template[row-1, column] = tmp;
                        template[row - 1, column + 1] = tmp;
                    }
                    


                    findercolumn++;
                }
                findercolumn = 0;
                finderrow++;
            }

            // ( 0, [(((V-1)*4)+21) - 7])
            rowstart = 0; columnstart = ((((2 - 1) * 4) + 21) - 7);
            finderrow = 0; findercolumn = 0;
            for (int row = rowstart; row < 7; row++)
            {
                for (int column = columnstart; (column- columnstart)< 7; column++)
                {
                    tmp = new QRModule();
                    tmp.isAvailabe = false;
                    tmp.value = finderPattern[finderrow, findercolumn];
                    template[row, column] = tmp;
                    // adding Separator 
                    if ((column - columnstart) == 0)
                    {
                        tmp = new QRModule();
                        tmp.isAvailabe = false;
                        tmp.value = 255;
                        template[row, column - 1] = tmp;
                    }
                    if (row == 6)
                    {
                        tmp = new QRModule();
                        tmp.isAvailabe = false;
                        tmp.value = 255;
                        template[row + 1, column] = tmp;
                        template[row + 1, column - 1] = tmp;
                    }
                    findercolumn++;
                }
                findercolumn = 0;
                finderrow++;
            }


        }
        public void save(String url )
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
