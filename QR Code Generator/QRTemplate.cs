using System;
using System.Collections;
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
        private byte[,] alignmentPattern;

        public QRTemplate(int version , string data)
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

            alignmentPattern = new byte[5,5];
            for (int i = 0; i < 5; i++)
            {
                alignmentPattern[0, i] = 0;
                alignmentPattern[4, i] = 0;
                alignmentPattern[i, 0] = 0;
                alignmentPattern[i, 4] = 0;
            }

            for (int i = 1; i < 4; i++)
            {
                alignmentPattern[1, i] = 255;
                alignmentPattern[3, i] = 255;
                alignmentPattern[i, 1] = 255;
                alignmentPattern[i, 3] = 255;
                
            }
            

            templateD = baseD+((version - 1) * 4);
            template = new QRModule[templateD, templateD];
            
            for(int row = 0; row<templateD; row++)
            {
                for (int column = 0; column < templateD; column++)
                {
                    QRModule p = new QRModule();
                    p.isAvailabe = true;
                    template[row,column] = p;
                }
            }

            // add Timing Patterns
            addTimingPatterns();

            // Placing The Finder Pattern inside the template 
            add_FinderPatterns_Separators();
            // place the alignment pattern
            addAlignmentPattern();

            // adding the dark module
            addDarkModule();

            // placing the data bits
            placeDta(data);

            // datamask
            dataMask();
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
        public static void save(QRModule[,] template,String url)
        {
            Bitmap b = new Bitmap(template.GetLength(0), template.GetLength(0) );
           
            for (int row = 0; row < template.GetLength(0); row++)
            {
                for (int col = 0; col < template.GetLength(0); col++)
                {
                    byte c = template[col, row].value;
                    b.SetPixel(row, col, Color.FromArgb(255, c, c, c));
                }

            }
            b.Save(url);
        }

        public void add_FinderPatterns_Separators()
        {
            int rowstart = 0, columnstart = 0;
            int finderrow = 0, findercolumn = 0;
            QRModule tmp = null;
            for (int row = rowstart; row < 7; row++)
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
                        
                        template[row, column + 2].isAvailabe = false;// reserve informationarea
                    }
                    if (row == 6)
                    {
                        tmp = new QRModule();
                        tmp.isAvailabe = false;
                        tmp.value = 255;
                        template[row + 1, column] = tmp;
                        
                        template[row + 2, column].isAvailabe = false; // reserve informationarea
                    }
                    template[row + 1, column + 1] = tmp;

                    template[row + 2, column + 2].isAvailabe = false; // reserve informationarea
                    findercolumn++;

                }
                findercolumn = 0;
                finderrow++;
            }

            // ([(((V-1)*4)+21) - 7], 0)
            rowstart = ((((2 - 1) * 4) + 21) - 7); columnstart = 0;
            finderrow = 0; findercolumn = 0;

            for (int row = rowstart; (row - rowstart) < 7; row++)
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
                        template[row, column + 2].isAvailabe = false; // reserved area
                    }

                    if ((row - rowstart) == 0)
                    {
                        tmp = new QRModule();
                        tmp.isAvailabe = false;
                        tmp.value = 255;
                        template[row - 1, column] = tmp;
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
                for (int column = columnstart; (column - columnstart) < 7; column++)
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
                        template[row + 2, column - 1].isAvailabe = false; // reserved area
                        template[row + 2, column].isAvailabe = false;// reserved area
                    }
                   
                    findercolumn++;
                }
                findercolumn = 0;
                finderrow++;
            }

        }

        public void addAlignmentPattern()
        {
            int alignmentstart = 18; //only valid location for alignment pattern is  18,18 in version 2 QR code 
            int alignmentrow = 0, alignmentcolumn = 0;
            QRModule tmp = null;
            for (int row = alignmentstart - 2; (row - (alignmentstart - 2)) < 5; row++)
            {

                for (int column = alignmentstart - 2; (column - (alignmentstart - 2)) < 5; column++)
                {
                    tmp = new QRModule();
                    tmp.isAvailabe = false;
                    tmp.value = alignmentPattern[alignmentrow, alignmentcolumn];

                    template[row, column] = tmp;
                    alignmentcolumn++;
                }
                alignmentcolumn = 0;
                alignmentrow++;
            }

        }

        public void addTimingPatterns()
        {
            bool isblack = true;
            QRModule tmp = null;
            for (int i = 0; i < templateD; i++)
            {
                if (template[6, i].isAvailabe)
                {
                    tmp = new QRModule();
                    tmp.isAvailabe = false;
                    if (isblack)
                    {
                        tmp.value = 0;
                        isblack = false;
                    }
                    else
                    {
                        tmp.value = 255;
                        isblack = true;
                    }
                    template[6, i] = tmp;
                    template[i, 6] = tmp;
                }

            }
        }
           
        public void addDarkModule()
        {
            template[((4 * 2) + 9), 8].isAvailabe = false;
            template[((4 * 2) + 9), 8].value = 0;
        }
        
        public void placeDta(string data)
        {
            bool up = true;
            int dataindex = 0;
            byte bitzero = 255;
            byte bitone = 0;
            for (int column = templateD-1; column > 6; column = column - 2)
            {
                if (up)
                {
                    for (int row =templateD-1; row>=0; row--)
                    {
                        if (template[row,column].isAvailabe)
                        {  
                            template[row, column].value = (data[dataindex] == '0')? bitzero : bitone;
                            dataindex++;
                        }
                        if (template[row, column-1].isAvailabe)
                        {
                            template[row, column-1].value = (data[dataindex] == '0') ? bitzero : bitone;
                            dataindex++;
                        }
                    }
                    up = false;
                }
                else
                {
                    for (int row = 0; row< templateD; row++)
                    {
                        if (template[row, column].isAvailabe)
                        {
                            template[row, column].value = (data[dataindex] == '0') ? bitzero : bitone;
                            dataindex++;
                        }
                        if (template[row, column - 1].isAvailabe)
                        {
                            template[row, column - 1].value = (data[dataindex] == '0') ? bitzero : bitone;
                            dataindex++;
                        }
                    }
                    up = true;
                }
            }

            for (int column = 5; column > 0; column = column - 2)
            {
                if (up)
                {
                    for (int row = templateD - 1; row >= 0; row--)
                    {
                        if (template[row, column].isAvailabe)
                        {
                            template[row, column].value = (data[dataindex] == '0') ? bitzero : bitone;
                            dataindex++;
                        }
                        if (template[row, column - 1].isAvailabe)
                        {
                            template[row, column - 1].value = (data[dataindex] == '0') ? bitzero : bitone;
                            dataindex++;
                        }
                    }
                    up = false;
                }
                else
                {
                    for (int row = 0; row < templateD; row++)
                    {
                        if (template[row, column].isAvailabe)
                        {
                            template[row, column].value = (data[dataindex] == '0') ? bitzero : bitone;
                            dataindex++;
                        }
                        if (template[row, column - 1].isAvailabe)
                        {
                            template[row, column - 1].value = (data[dataindex] == '0') ? bitzero : bitone;
                            dataindex++;
                        }
                    }
                    up = true;
                }
            }


        }

        public void dataMask()
        {
            List<QRModule[,]> masktemplates = new List<QRModule[,]>();

            masktemplates.Add(template);  //(row + column) mod 2 == 0
            //masktemplates.Add(template);  //(row) mod 2 == 0
            //masktemplates.Add(template);  //(column) mod 3 == 0
            //masktemplates.Add(template);  //(row + column) mod 3 == 0
            //masktemplates.Add(template);  //( floor(row / 2) + floor(column / 3) ) mod 2 == 0
            //masktemplates.Add(template);  //((row * column) mod 2) + ((row * column) mod 3) == 0
            //masktemplates.Add(template);  //( ((row * column) mod 2) + ((row * column) mod 3) ) mod 2 == 0
            //masktemplates.Add(template);  //( ((row + column) mod 2) + ((row * column) mod 3) ) mod 2 == 0

            QRModule[,] tmp = null;
            //masktemplates.Count
            for (int i = 0; i<masktemplates.Count; i++)
            {
                tmp = (QRModule[,])masktemplates[i];
                switch (i)
                {
                    case 0:
                        for (int row = 0; row<templateD; row++)
                        {
                            for (int column =0; column<templateD; column++)
                            {
                                if ( (row+column)%2==0 && tmp[row, column].isAvailabe)
                                {
                                    tmp[row, column].value = (tmp[row, column].value==0)? (byte)255: (byte)0;
                                }
                            }
                        }
                        masktemplates[i] = tmp;
                        break;
                    case 1:
                        for (int row = 0; row < templateD; row++)
                        {
                            for (int column = 0; column < templateD; column++)
                            {
                                if ((row ) % 2 == 0 && tmp[row, column].isAvailabe)
                                {
                                    tmp[row, column].value = (tmp[row, column].value == 0) ? (byte)255 : (byte)0;
                                }
                            }
                        }
                        masktemplates[i] = tmp;
                        break;
                    case 2:
                        for (int row = 0; row < templateD; row++)
                        {
                            for (int column = 0; column < templateD; column++)
                            {
                                if ((column) % 3 == 0 && tmp[row, column].isAvailabe)
                                {
                                    tmp[row, column].value = (tmp[row, column].value == 0) ? (byte)255 : (byte)0;
                                }
                            }
                        }
                        masktemplates[i] = tmp;
                        break;
                    case 3:
                        for (int row = 0; row < templateD; row++)
                        {
                            for (int column = 0; column < templateD; column++)
                            {
                                if ((row + column) % 3 == 0 && tmp[row, column].isAvailabe)
                                {
                                    tmp[row, column].value = (tmp[row, column].value == 0) ? (byte)255 : (byte)0;
                                }
                            }
                        }
                        masktemplates[i] = tmp;
                        break;
                    case 4:
                        for (int row = 0; row < templateD; row++)
                        {
                            for (int column = 0; column < templateD; column++)
                            {
                                if ( (Math.Floor((double)row / 2) + Math.Floor((double)column / 3)) % 2 == 0 && tmp[row, column].isAvailabe)
                                {
                                    tmp[row, column].value = (tmp[row, column].value == 0) ? (byte)255 : (byte)0;
                                }
                            }
                        }
                        masktemplates[i] = tmp;
                        break;
                    case 5:
                        for (int row = 0; row < templateD; row++)
                        {
                            for (int column = 0; column < templateD; column++)
                            {
                                if (((row * column) % 2) +((row * column) % 3) == 0 && tmp[row, column].isAvailabe)
                                {
                                    tmp[row, column].value = (tmp[row, column].value == 0) ? (byte)255 : (byte)0;
                                }
                            }
                        }
                        masktemplates[i] = tmp;
                        break;
                    case 6:
                        for (int row = 0; row < templateD; row++)
                        {
                            for (int column = 0; column < templateD; column++)
                            {
                                if ((((row * column) % 2) +((row * column) % 3) )% 2 == 0 && tmp[row, column].isAvailabe)
                                {
                                    tmp[row, column].value = (tmp[row, column].value == 0) ? (byte)255 : (byte)0;
                                }
                            }
                        }
                        masktemplates[i] = tmp;
                        break;
                    case 7:
                        for (int row = 0; row < templateD; row++)
                        {
                            for (int column = 0; column < templateD; column++)
                            {
                                if ((((row + column) % 2) +((row * column) % 3) ) % 2 == 0 && tmp[row, column].isAvailabe)
                                {
                                    tmp[row, column].value = (tmp[row, column].value == 0) ? (byte)255 : (byte)0;
                                }
                            }
                        }
                        masktemplates[i] = tmp;
                        break;
                }
            }
            for (int i = 0; i < masktemplates.Count; i++)
            {
                tmp = (QRModule[,])masktemplates[i];
                QRTemplate.save(tmp , "D://img_"+i+".png" );
            }

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
