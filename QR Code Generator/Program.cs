using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Code_Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            DataEncoding DataObject = new DataEncoding("AREE SARCHIL");
            string encodedData = DataObject.Encode();
            int msgSize = 0;
            string tmp = "";

            ArrayList msgPoly = new ArrayList();

            for (int i = 0; i < encodedData.Length; i=i+8 )
            {
                tmp = encodedData.Substring(i, 8);
               // Console.WriteLine(Convert.ToInt32(tmp, 2));
                msgPoly.Add(Convert.ToInt32(tmp, 2));
                msgSize++;
            }
            Polynomial polynomial1 = new Polynomial(msgPoly, 27);

            ArrayList genPoly = new ArrayList();
            genPoly.Add(0);
            genPoly.Add(120);
            genPoly.Add(104);
            genPoly.Add(107);
            genPoly.Add(109);
            genPoly.Add(102);
            genPoly.Add(161);
            genPoly.Add(76);
            genPoly.Add(3);
            genPoly.Add(91);
            genPoly.Add(191);
            genPoly.Add(147);
            genPoly.Add(169);
            genPoly.Add(182);
            genPoly.Add(194);
            genPoly.Add(225);
            genPoly.Add(120);
            Polynomial polynomial2 = new Polynomial(genPoly, 16);
          

            ErrorCorrection err = new ErrorCorrection();
            Polynomial correctionCodeWords = err.Divide(polynomial1, polynomial2);

            for (int i = 0; i < correctionCodeWords.C.Count; i++ )
            {
                Console.Write((int)correctionCodeWords.C[i]+" , " );
            }
            

        }
    }
}
