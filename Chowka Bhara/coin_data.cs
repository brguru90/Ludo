using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chowka_Bhara
{
    public class coin_data
    {
        public int coin_val, i, j,x,y;
        public bool safe;
    }
    public class player
    {
        public coin_data[] coin=new coin_data[4];
        public player()
        {
            for (int i = 0; i < 4; i++)
                coin[i] = new coin_data();
        }
    }
}

