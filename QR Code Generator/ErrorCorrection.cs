using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Code_Generator
{
    class ErrorCorrection
    {

       public Polynomial Divide(Polynomial A , Polynomial B){
           int n = B.C.Count; // error correction code word

           int degreeofA = (int)A.P[0];
           for (int i = 0; i < A.P.Count; i++)
           {
               A.P[i] = (int)A.P[i] + n;
           }
           for (int i = 0; i < B.P.Count; i++)
           {
               B.P[i] = (int)B.P[i] + degreeofA;
           }
           //3x^2+x-1 A Ca , Pa
           //X+1 B  Cb Pb
           // result  C  Cc Pc
           // Pc0 = Pa0-Pb0
           // Cc0 = Ca0/Cb0
           // Tc = Cc0*Cb
           // Tp = Pc0+Pb
           // Ca = Ca-Tc
           // Pa = Pa discard first index
           Polynomial Pc = new Polynomial();
           Polynomial T = new Polynomial();
           //(int)Pc.P[Pc.P.Count - 1] != 0
           logAntilogTable table =new  logAntilogTable();
           int loop = A.C.Count;
           while (loop > 0) 
           {
               if(A.C.Count==n){
                   A.C.Add(0);
               }
               //int Pci = (int)A.P[0] - (int)B.P[0];
               int Pci = 0; // was here first
               // int Cci = (int)A.C[0] / (int)B.C[0];
               //Console.WriteLine("Key:" + (int)A.C[0]);
               int Cci = 0;
               if ((int)A.C[0] != 0)
               {
                   Cci = (int)table.int2pow[(int)A.C[0]];
               }
               
               Console.Write("Calculated Value:");
               //Pc.P.Add(Pci);
               //Pc.C.Add(Cci);
               for (int j = 0; j < B.C.Count; j++)
               {
                   Console.Write((int)table.pow2int[((Cci + (int)B.C[j]) % 255)] + " ");

                   T.C.Add((int)table.pow2int[((Cci + (int)B.C[j]) % 255)]);

                   T.P.Add(Pci + (int)B.P[j]);
               }
               Console.WriteLine();

               Console.Write("MSG Polynomial  :");
               for (int j = 0; j < A.C.Count; j++)
               {

                   if (j >= T.C.Count) // added this if it must xored with all A elements
                   {
             
                       Console.Write((int)A.C[j] + " "); // the loop must go all the way 
                       A.C[j] = (int)A.C[j] ^ 0;
                   }
                   else
                   {
                       Console.Write((int)A.C[j] +" "); // the loop must go all the way 
                       A.C[j] = (int)A.C[j] ^ (int)T.C[j];
                   }
                  
               }

               Console.WriteLine("\n______________________________________________________");

              
                A.C.RemoveAt(0); // it was like this
                A.P.RemoveAt(0);
              

               if(loop==1){
                   Pc = A;
               }
               T = new Polynomial();
               loop--;
           } 
           return Pc;

       }

    }
}
