using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Code_Generator
{
    class DataEncoding
    {
        private String ModeIndicator = "0010";
        private String input;
        private String paddByte236 = "11101100";
        private String paddByte17 = "00010001";
        private int TotalCodeWords = 28;
        public Dictionary<String, int> AlphaNumericTable = new Dictionary<string,int>();
        public DataEncoding(String inpt)
        {
            input = inpt;
            AlphaNumericTable.Add("0", 0);
            AlphaNumericTable.Add("1", 1);
            AlphaNumericTable.Add("2", 2);
            AlphaNumericTable.Add("3", 3);
            AlphaNumericTable.Add("4", 4);
            AlphaNumericTable.Add("5", 5);
            AlphaNumericTable.Add("6", 6);
            AlphaNumericTable.Add("7", 7);
            AlphaNumericTable.Add("8", 8);
            AlphaNumericTable.Add("9", 9);
            AlphaNumericTable.Add("A", 10);
            AlphaNumericTable.Add("B", 11);
            AlphaNumericTable.Add("C", 12);
            AlphaNumericTable.Add("D", 13);
            AlphaNumericTable.Add("E", 14);
            AlphaNumericTable.Add("F", 15);
            AlphaNumericTable.Add("G", 16);
            AlphaNumericTable.Add("H", 17);
            AlphaNumericTable.Add("I", 18);
            AlphaNumericTable.Add("J", 19);
            AlphaNumericTable.Add("K", 20);
            AlphaNumericTable.Add("L", 21);
            AlphaNumericTable.Add("M", 22);
            AlphaNumericTable.Add("N", 23);
            AlphaNumericTable.Add("O", 24);
            AlphaNumericTable.Add("P", 25);
            AlphaNumericTable.Add("Q", 26);
            AlphaNumericTable.Add("R", 27);
            AlphaNumericTable.Add("S", 28);
            AlphaNumericTable.Add("T", 29);
            AlphaNumericTable.Add("U", 30);
            AlphaNumericTable.Add("V", 31);
            AlphaNumericTable.Add("W", 32);
            AlphaNumericTable.Add("X", 33);
            AlphaNumericTable.Add("Y", 34);
            AlphaNumericTable.Add("Z", 35);
            AlphaNumericTable.Add(" ", 36);
            AlphaNumericTable.Add("$", 37);
            AlphaNumericTable.Add("%", 38);
            AlphaNumericTable.Add("*", 39);
            AlphaNumericTable.Add("+", 40);
            AlphaNumericTable.Add("-", 41);
            AlphaNumericTable.Add(".", 42);
            AlphaNumericTable.Add("/", 43);
            AlphaNumericTable.Add(":", 44);
        }

        public String Encode(){
            String tempLength = Convert.ToString(input.Length, 2);
            String CountIndicatorBinary = paddZeros(tempLength, (9 - tempLength.Length) , 'L');
           
            String binaryResult = "";
            if (input.Length % 2 == 0)
            {
                int n1, n2,n3;
                string tmp = "";
                for (int i = 0; i < input.Length - 1; i = i+2 )
                {
                   n1 = AlphaNumericTable[ input[i]+"" ];
                   n2 = AlphaNumericTable[input[i+1] + ""];
                   n3 = (n1 * 45) + n2;
                   tmp = Convert.ToString(n3, 2);
                   binaryResult += paddZeros(tmp,(11-tmp.Length),'L' ) ;
                    
                }

            }
            else
            {
                int n1, n2, n3;
                string tmp = "";
                for (int i = 0; i < input.Length - 2; i = i + 2)
                {
                    n1 = AlphaNumericTable[input[i] + ""];
                    n2 = AlphaNumericTable[input[i + 1] + ""];
                    n3 = (n1 * 45) + n2;
                    tmp = Convert.ToString(n3, 2);
                    binaryResult += paddZeros(tmp, (11 - tmp.Length), 'L');
                }
                n1 = AlphaNumericTable[input[input.Length-1] + ""];
                tmp = Convert.ToString(n1, 2);
                binaryResult += paddZeros(tmp, (6 - tmp.Length), 'L');
                //Console.WriteLine(binaryResult);
            }
            binaryResult = ModeIndicator + CountIndicatorBinary + binaryResult;
            if ((TotalCodeWords * 8) >= binaryResult.Length && ((TotalCodeWords * 8) - binaryResult.Length) >= 4)
            {
                binaryResult = paddZeros(binaryResult, 4, 'R');
                
            }
            else
            {
                binaryResult = paddZeros(binaryResult, ((TotalCodeWords * 8) - binaryResult.Length), 'R');
                
            }

            if (binaryResult.Length%8 !=0)
            {
                while (binaryResult.Length % 8 != 0)
                {
                    binaryResult += "0";
                }
            }

            if (binaryResult.Length < (TotalCodeWords * 8) )
            {
                int remainingBytes = ((TotalCodeWords * 8) - binaryResult.Length )/8;
                bool add236 = true;
                
                for (int i = 0; i < remainingBytes; i++ )
                {
                    if (add236)
                    {
                        binaryResult += paddByte236;
                        add236 = false;
                    }
                    else
                    {
                        binaryResult += paddByte17;
                        add236 = true;
                    }
                }
            }

            return binaryResult;
        }

        private String paddZeros(String input , int zeroCount , char mod)
        {
            String res = "";
            for (int i = 0; i < zeroCount; i++ )
            {
                res += "0";
            }
            if(mod=='L'){
                res = res + input;  
            }else if(mod=='R'){
                res = input + res;
            }
            return res;
        }
            
    }
}
