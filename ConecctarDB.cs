using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace TPV_Bar
{
   public  class ConectarDB
    {
        MySqlConnection conexion;
        MySqlConnection conexion2;
        MySqlCommand comando;
        MySqlDataReader DR;

        //Lista de los camareros
        List<Camarero> ListaCamarero = new List<Camarero>();

        //Lista para guardar la facturacion de una comanda
        List<FacturacionBar> ListaFacturacion = new List<FacturacionBar>();

        //Lista para guardar los productos
        List<Producto> ListaProducto = new List<Producto>();

        //Lista para guardar los producto bajos en stock
        List<Producto> ListaProductoBajoStock = new List<Producto>();

        //Lista para guardar el total de las facturas de un camarero 
        List<FacturacionBar> ListaFacturacionTotal = new List<FacturacionBar>();

        List<FacturacionBar> ListaFecha = new List<FacturacionBar>();
        //Lista para actualizar los productos
        List<Producto> listaProductos = new List<Producto>();

        public ConectarDB()
        {
            conexion = new MySqlConnection();
            conexion2 = new MySqlConnection();
           // Bases de datos gestionadas por el profesor, no borrar
            conexion.ConnectionString = "server=freedb.tech;Database=freedbtech_antonio2020;uid=freedbtech_javier2020;pwd=javier2020";
            conexion2.ConnectionString = "server=freedb.tech;Database=freedbtech_CopiaSeguridad;uid=freedbtech_Dam2020;pwd=Pili2020";
        }
        public List<Camarero> ListarCamareros()
        {
            String cadenaSql = "select * from camarero";
            conexion.Open();
            comando = new MySqlCommand(cadenaSql, conexion);
            DR = comando.ExecuteReader();
            while (DR.Read())
            {
                Camarero tf = new Camarero();
                tf.Id_camarero = Convert.ToInt32(DR[0]);
                tf.Nombre = Convert.ToString(DR[1]);
                tf.MailCamarero = Convert.ToString(DR[2]);
                tf.Dni = Convert.ToString(DR[3]);
                ListaCamarero.Add(tf);
            }
            conexion.Close();
            return ListaCamarero;
        }



        internal DataTable mostrarCamareros()
        {
            //Método igual que el anterior pero guarda los datos directamente en una tabla
            conexion.Open();

            //Seleccionar las frutas
            string cadenaSql = "select * from camarero";
            MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
            DataTable dt = new DataTable();
            MySqlDataAdapter adap = new MySqlDataAdapter(comando);
            adap.Fill(dt);
            conexion.Close();
            return dt;
        }

        public List<FacturacionBar> ListarFacturaciones()
        {
            String cadenaSql = "select * from facturacionBar";
            conexion.Open();
            comando = new MySqlCommand(cadenaSql, conexion);
            DR = comando.ExecuteReader();
            while (DR.Read())
            {
                FacturacionBar tf = new FacturacionBar();
                tf.Fecha = Convert.ToDateTime(DR[0]);
                tf.Id = Convert.ToString(DR[1]);
                tf.Nombre = Convert.ToString(DR[2]);
                tf.PrecioUnitario = Convert.ToDouble(DR[3]);
                tf.IdCamarero = Convert.ToString(DR[4]);
                ListaFacturacion.Add(tf);
            }
            conexion.Close();
            return ListaFacturacion;
        }
        public List<Producto> ListarProducto()
        {
            String cadenaSql = "select * from producto";
            conexion.Open();
            comando = new MySqlCommand(cadenaSql, conexion);
            DR = comando.ExecuteReader();
            while (DR.Read())
            {
                Producto tf = new Producto();
                tf.Id_producto = Convert.ToInt32(DR[0]);
                tf.Nombre = Convert.ToString(DR[1]);
                tf.Id_proveedor = Convert.ToInt32(DR[2]);
                tf.Precio = Convert.ToDouble(DR[3]);
                tf.Stock_actual = Convert.ToInt32(DR[4]);
                tf.Stock_minimo = Convert.ToInt32(DR[5]);
                tf.Imagenes = (byte[])DR[6];
                ListaProducto.Add(tf);
            }
            conexion.Close();
            return ListaProducto;
        }

        internal int ActualizarAlmacen(List<FacturacionBar> listaFacturaciones)
        {
            conexion.Open();
            int resultado = 0;
            for (int i = 0; i < listaFacturaciones.Count; i++)
            {
                String cadenaSql = "update producto set Stock_actual=(Stock_actual-1) where Id_producto=?id";
                MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
                comando.Parameters.Add("?id", MySqlDbType.VarChar).Value = listaFacturaciones[i].Id;
                resultado = comando.ExecuteNonQuery();

            }
            conexion.Close();
            return resultado;
        }

        internal int comprar(List<FacturacionBar> listaFacturaciones, string idCamarero)
        {
            conexion.Open();
            int resultado = 0;
            for (int i = 0; i < listaFacturaciones.Count; i++)
            {
                String cadenaSql = "insert into facturacionBar values(?fecha,?id,?nombre,?precioUnitario,?idCamarero)";
                MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
                comando.Parameters.Add("?fecha", MySqlDbType.DateTime).Value = DateTime.Now;
                comando.Parameters.Add("?id", MySqlDbType.VarChar).Value = listaFacturaciones[i].Id;
                comando.Parameters.Add("?nombre", MySqlDbType.VarChar).Value = listaFacturaciones[i].Nombre;
                comando.Parameters.Add("?precioUnitario", MySqlDbType.Double).Value = listaFacturaciones[i].PrecioUnitario;
                comando.Parameters.Add("?idCamarero", MySqlDbType.VarChar).Value = idCamarero;
                resultado = comando.ExecuteNonQuery();

            }
            conexion.Close();
            return resultado;
        }

        internal List<Producto> comprobarStock()
        {
            String cadenaSql = "select * from producto where Stock_actual<Sock_minimo";
            comando = new MySqlCommand(cadenaSql, conexion);
            DR = comando.ExecuteReader();
            while (DR.Read())
            {
                Producto tf = new Producto();
                tf.Id_producto = Convert.ToInt32(DR[0]);
                tf.Nombre = Convert.ToString(DR[1]);
                tf.Id_proveedor = Convert.ToInt32(DR[2]);
                tf.Precio = Convert.ToDouble(DR[3]);
                tf.Stock_actual = Convert.ToInt32(DR[4]);
                tf.Stock_minimo = Convert.ToInt32(DR[5]);
                tf.Imagenes = (byte[])DR[6];
                ListaProductoBajoStock.Add(tf);
            }
            conexion.Close();
            return ListaProductoBajoStock;
        }

        internal List<FacturacionBar> listarTotal(string camarero)
        {
            String cadenaSql = "select * from facturacionBar where idCamarero=?camarero";
            conexion.Open();
            comando = new MySqlCommand(cadenaSql, conexion);
            comando.Parameters.Add("?camarero", MySqlDbType.VarChar).Value = camarero;
            DR = comando.ExecuteReader();
            while (DR.Read())
            {
                FacturacionBar tF = new FacturacionBar();
                tF.Fecha = Convert.ToDateTime(DR[0]);
                tF.Id = Convert.ToString(DR[1]);
                tF.Nombre = Convert.ToString(DR[2]);
                tF.PrecioUnitario = Math.Round(Convert.ToDouble(DR[3]), 2);
                tF.IdCamarero = Convert.ToString(DR[4]);

                ListaFacturacionTotal.Add(tF);
            }
            conexion.Close();
            return ListaFacturacionTotal;
        }

        internal int insertarTabla2(string imagen, string id, string nombre, string proveedor, double precio,int stock_actual, int stock_minimo)
        {
            //Lo que está en Moodle
            conexion.Open();
            FileStream fs = new FileStream(imagen, FileMode.Open, FileAccess.Read);
            //tamaño del fichero
            long tamanio = fs.Length;

            //definir el array de datos binarios en función a la longitud del fichero
            BinaryReader br = new BinaryReader(fs);

            byte[] bloque = br.ReadBytes((int)fs.Length);//se castea
            fs.Read(bloque, 0, Convert.ToInt32(tamanio));

            MySqlCommand consultainsertar = new MySqlCommand("insert into  producto values(?id_producto,?nombre,?proveedor,?precio,?stock_actual,?stock_minimo,?imagen)", conexion);
            consultainsertar.Parameters.Add("?id_producto", MySqlDbType.Int32).Value =id ;
            consultainsertar.Parameters.Add("?nombre", MySqlDbType.VarChar).Value = nombre;
            consultainsertar.Parameters.Add("?precio", MySqlDbType.Double).Value = precio;
            consultainsertar.Parameters.Add("?proveedor", MySqlDbType.VarChar).Value = proveedor;
            consultainsertar.Parameters.Add("?stock_actual", MySqlDbType.Int32).Value = stock_actual;
            consultainsertar.Parameters.Add("?stock_minimo", MySqlDbType.Int32).Value = stock_minimo;
            consultainsertar.Parameters.Add("?imagen", MySqlDbType.Blob).Value = bloque;
            int codigo = consultainsertar.ExecuteNonQuery();
            conexion.Close();
            return codigo;
        }

       

        internal List<FacturacionBar> buscarFecha(String camarero)
        {
            String cadenaSql = "select * from facturacionBar where idCamarero=?camarero";
            conexion.Open();
            comando = new MySqlCommand(cadenaSql, conexion);
            comando.Parameters.Add("?camarero", MySqlDbType.VarChar).Value = camarero;
            DR = comando.ExecuteReader();
            while (DR.Read())
            {
                FacturacionBar tF = new FacturacionBar();
                tF.Fecha = Convert.ToDateTime(DR[0]);
                tF.Id = Convert.ToString(DR[1]);
                tF.Nombre = Convert.ToString(DR[2]);
                tF.PrecioUnitario = Math.Round(Convert.ToDouble(DR[3]), 2);
                tF.IdCamarero = Convert.ToString(DR[4]);

                ListaFecha.Add(tF);
            }
            conexion.Close();
            return ListaFecha;
        }



        internal int ActualizarStock(String[]campos)
        {
            conexion.Open();
            int resultado = 0;
           
                String cadenaSql = "update producto set Stock_actual=Stock_actual+?unidadesRecibidas where Id_producto=?IdProducto";
                MySqlCommand comando = new MySqlCommand(cadenaSql, conexion);
                comando.Parameters.Add("?unidadesRecibidas", MySqlDbType.Int32).Value = campos[2];
                comando.Parameters.Add("?IdProducto", MySqlDbType.Int32).Value = campos[0];
                resultado = comando.ExecuteNonQuery();
  
            conexion.Close();
            return resultado;
        }

        internal int modificarPrecio(string precio, String id)
        {
            conexion.Open();
            String cadenaSql = "update producto set precio=?NuevoPrecio where Id_producto=?id";
            comando = new MySqlCommand(cadenaSql, conexion);
            comando.Parameters.Add("?NuevoPrecio", MySqlDbType.Double).Value = Convert.ToDouble(precio);
            comando.Parameters.Add("?id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            int resultado = comando.ExecuteNonQuery();
            conexion.Close();
            return resultado;
        }
        internal int modificarProveedor(string proveedor, String id)
        {
            conexion.Open();
            String cadenaSql = "update producto set Id_proveedor=?NuevoProveedor where Id_producto=?id";
            comando = new MySqlCommand(cadenaSql, conexion);
            comando.Parameters.Add("?NuevoProveedor", MySqlDbType.VarChar).Value = proveedor;
            comando.Parameters.Add("?id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            int resultado = comando.ExecuteNonQuery();
            conexion.Close();
            return resultado;
        }
        internal int modificarStockMinimo(string minimo, string id)
        {
            conexion.Open();
            String cadenaSql = "update producto set Stock_minimo=?NuevoStock where Id_producto=?id";
            comando = new MySqlCommand(cadenaSql, conexion);
            comando.Parameters.Add("?NuevoStock", MySqlDbType.Int32).Value = Convert.ToDouble(minimo);
            comando.Parameters.Add("?id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            int resultado = comando.ExecuteNonQuery();
            conexion.Close();
            return resultado;
        }

        internal int modificarImagen(byte[]foto, string id)
        {
            conexion.Open();
            String cadenaSql = "update producto set imagenes=?imagen where Id_producto=?id";
            comando = new MySqlCommand(cadenaSql, conexion);
            comando.Parameters.Add("?imagen", MySqlDbType.Blob).Value = foto;
            comando.Parameters.Add("?id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            int resultado = comando.ExecuteNonQuery();
            conexion.Close();
            return resultado;
        }

        internal int modificarEmail(string email, object id)
        {
            conexion.Open();
            String cadenaSql = "update camarero set email=?email where Id_camarero=?id"; // Nombre de las columnas???
            comando = new MySqlCommand(cadenaSql, conexion);
            comando.Parameters.Add("?email", MySqlDbType.VarChar).Value = email;
            comando.Parameters.Add("?id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
            int resultado = comando.ExecuteNonQuery();
            conexion.Close();
            return resultado;
        }

        internal int insertarCamarero( int Id_camarero, string nombre, string mailCamarero,string dni)
        {
           
            conexion.Open();
            MySqlCommand consultainsertar = new MySqlCommand("insert into  camarero values(?id_camarero,?nombre,?mailCamarero,?dni)", conexion);
            consultainsertar.Parameters.Add("?id_camerero", MySqlDbType.Int32).Value = Id_camarero;
            consultainsertar.Parameters.Add("?nombre", MySqlDbType.VarChar).Value = nombre;
            consultainsertar.Parameters.Add("?mailCamarero", MySqlDbType.VarChar).Value = mailCamarero;
            consultainsertar.Parameters.Add("?dni", MySqlDbType.VarChar).Value = dni;
            int codigo = consultainsertar.ExecuteNonQuery();
            conexion.Close();
            return codigo;
        }


        internal void importar(String fichero)
        {
            //Importar desde fichero .sql a nueva db en la conexion 2
            MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexion2; //Añadir, que no está en los apuntes
            MySqlBackup mb = new MySqlBackup(comando);
            conexion2.Open(); mb.ImportFromFile(fichero);
            conexion2.Close();

        }


        internal void exportar(String fichero)
        {
            //Exportar la db de la primera conexion a un fichero .sql
            MySqlCommand comando= new MySqlCommand();
            comando.Connection = conexion; //Añadir, que no está en los apuntes
            conexion.Open();
            MySqlBackup mb = new MySqlBackup(comando);
            mb.ExportToFile(fichero);
            conexion.Close();
        }
        

    }
}
