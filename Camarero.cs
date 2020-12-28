using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPV_Bar
{
   public class Camarero
    {
            int id_camarero;
            string nombre;
            string mailCamarero;
            string dni;

        public int Id_camarero
        {
            get
            {
                return id_camarero;
            }

            set
            {
                id_camarero = value;
            }
        }

        public string Nombre
        {
            get
            {
                return nombre;
            }

            set
            {
                nombre = value;
            }
        }

        public string MailCamarero
        {
            get
            {
                return mailCamarero;
            }

            set
            {
                mailCamarero = value;
            }
        }

        public string Dni
        {
            get
            {
                return dni;
            }

            set
            {
                dni = value;
            }
        }

        public Camarero()
        {
        }

        public Camarero(int id_camarero, string nombre, string mailCamarero, string dni)
        {
            this.Id_camarero = id_camarero;
            this.Nombre = nombre;
            this.MailCamarero = mailCamarero;
            this.Dni = dni;
        }
    }
}
