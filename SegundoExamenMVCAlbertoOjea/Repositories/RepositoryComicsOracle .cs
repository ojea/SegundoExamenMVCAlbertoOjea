using Microsoft.AspNetCore.Http.HttpResults;
using Oracle.ManagedDataAccess.Client;
using SegundoExamenMVCAlbertoOjea.Models;
using System.Collections.Generic;
using System.Data;

#region
//CREATE OR REPLACE PROCEDURE SP_INSERT_COMIC
//(P_NOMBRE COMICS.NOMBRE%TYPE, P_IMAGEN COMICS.IMAGEN%TYPE, P_DESCRIPCION COMICS.DESCRIPCION%TYPE)
//AS
//P_IDCOMIC COMICS.IDCOMIC%TYPE;
//BEGIN
//  SELECT MAX(IDCOMIC)+1 INTO P_IDCOMIC

//  FROM COMICS;
//INSERT INTO COMICS VALUES
//  (P_IDCOMIC, P_NOMBRE, P_IMAGEN, P_DESCRIPCION);
//COMMIT;
//END;
#endregion

namespace SegundoExamenMVCAlbertoOjea.Repositories
{
    public class RepositoryComicsOracle: IRepositoryComics
    {

        private DataTable tablaComics;
        private OracleConnection cn;
        private OracleCommand com;

        public RepositoryComicsOracle()
        {
            string connectionString =@"Data Source=LOCALHOST:1521/XE; Persist Security Info=True; User Id=SYSTEM; Password=oracle"; 
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            string sql = "select * from COMICS";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
            this.tablaComics = new DataTable();
            ad.Fill(this.tablaComics);
        }

        public void CreateComicProcedure(Comic comic)
        {
            OracleParameter paramNombre = new OracleParameter(":P_NOMBRE", comic.Nombre);
            this.com.Parameters.Add(paramNombre);

            OracleParameter paramImagen = new OracleParameter(":P_IMAGEN", comic.Imagen);
            this.com.Parameters.Add(paramImagen);

            OracleParameter paramDescripcion = new OracleParameter(":P_DESCRIPCION", comic.Descripcion);
            this.com.Parameters.Add(paramDescripcion);
            this.com.CommandText = "SP_INSERT_COMIC";
            this.com.CommandType = CommandType.StoredProcedure;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void Delete(int idComic)
        {
            string sql = "delete from comics where idcomic= :idcomic";
            OracleParameter pamIdComic = new OracleParameter(":idcomic", idComic);
            this.com.Parameters.Add(pamIdComic);
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();

        }

        public Comic GetComicId(int idComic)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() where datos.Field<int>("IDCOMIC") == idComic select datos;

            var row = consulta.First();
            Comic comic = new Comic
            {
                idComic = row.Field<int>("IDCOMIC"),
                Nombre = row.Field<string>("NOMBRE"),
                Imagen = row.Field<string>("IMAGEN"),
                Descripcion = row.Field<string>("DESCRIPCION"),
            };
            return comic;
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() select datos;
            List<Comic> comics = new List<Comic>(); 
            foreach(var row in consulta)
            {
                Comic comic = new Comic
                {

                    idComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION"),
                };
                comics.Add(comic);
            }
            return comics;
        }

        public void Insert(Comic comic)
        {
            string sql = "insert into comics values(:idcomic, :nombre, :imagen, :descripcion)";
            
            OracleParameter pamIdComic = new OracleParameter(":idcomic", comic.idComic);
            this.com.Parameters.Add(pamIdComic);

            OracleParameter pamNombre = new OracleParameter(":nombre", comic.Nombre);
            this.com.Parameters.Add(pamNombre);

            OracleParameter pamImagen = new OracleParameter(":imagen", comic.Imagen);
            this.com.Parameters.Add(pamImagen);

            OracleParameter pamDescripcion = new OracleParameter(":descripcion", comic.Descripcion);
            this.com.Parameters.Add(pamDescripcion);

            this.com.CommandType = CommandType.Text;    
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();


        }
    }
}
