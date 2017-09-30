using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Code_Generator
{
    class Polynomial
    {
        public ArrayList C;
        public ArrayList P;
        public Polynomial(ArrayList polynomial, int degree)
        {
            P = new ArrayList();
            C = new ArrayList();
            C = polynomial;  
            for (int i = degree; i >= 0; i-- )
            {
                P.Add(i);
            }
        }
        public Polynomial()
        {
            P = new ArrayList();
            C = new ArrayList();
        }

        
    }
}
