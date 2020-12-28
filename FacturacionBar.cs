using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPV_Bar
{
  public  class FacturacionBar
    {
        DateTime fecha;
        string id;
        string nombre;
        double precioUnitario;
        string idCamarero;

        public DateTime Fecha
        {
            get
            {
                return fecha;
            }

            set
            {
                fecha = value;
            }
        }

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
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

        public double PrecioUnitario
        {
            get
            {
                return precioUnitario;
            }

            set
            {
                precioUnitario = value;
            }
        }

        public string IdCamarero
        {
            get
            {
                return idCamarero;
            }

            set
            {
                idCamarero = value;
            }
        }

        public FacturacionBar()
        {
        }

        public FacturacionBar(DateTime fecha, string id, string nombre, double precioUnitario, string idCamarero)
        {
            this.Fecha = fecha;
            this.Id = id;
            this.Nombre = nombre;
            this.PrecioUnitario = precioUnitario;
            this.IdCamarero = idCamarero;
        }
    } 
        
    }
