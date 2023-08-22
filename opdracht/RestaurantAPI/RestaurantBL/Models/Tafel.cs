using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBL.Models
{
    public class Tafel
    {
        public int Id { get => _id; 
            set
            {
                if (value <= 0) throw new RestaurantException("Tafel Id moet groter dan 0 zijn.");  
                _id = value;
            }
        }
        public int Plaatsen
        {
            get => plaatsen;
            set
            {
                if (value <= 0) throw new TafelException("Plaaten moet hoger zijn dan 0.");
                plaatsen = value;
            }
        }

        //public int TafelNr { get; set; }

        private int plaatsen;
        private int _id;

        public Tafel(int id, int plaatsen)
        {
            Id = id;
            Plaatsen = plaatsen;

        }

        public Tafel(int plaatsen)
        {
            Plaatsen = plaatsen;
        }

        public override bool Equals(object? obj)
        {
            Tafel t2 = obj as Tafel;
            if (t2 == null)
                return false;
            else
                return this.Id == t2.Id;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }


    }
}
