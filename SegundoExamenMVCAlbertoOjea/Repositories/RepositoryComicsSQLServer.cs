using Microsoft.AspNetCore.Http.HttpResults;
using SegundoExamenMVCAlbertoOjea.Models;
using System.Data;
using System.Data.SqlClient;

#region
//CREATE OR ALTER PROCEDURE SP_INSERT_COMIC
//(@NOMBRE NVARCHAR(50), @IMAGEN NVARCHAR(1000), @DESCRIPCION NVARCHAR(50))

//AS
//    DECLARE @IDCOMIC INT
//	SELECT @IDCOMIC = MAX(IDCOMIC) +1

//    FROM COMICS
//	INSERT INTO COMICS VALUES
//	(@IDCOMIC, @NOMBRE, @IMAGEN, @DESCRIPCION)
//GO
#endregion

namespace SegundoExamenMVCAlbertoOjea.Repositories
{
    public class RepositoryComicsSQLServer : IRepositoryComics
    {
        private DataTable tablaComics;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryComicsSQLServer()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=COMICS;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.tablaComics = new DataTable();
            string sql = "select * from COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(this.tablaComics);
        }

        public void CreateComicProcedure(Comic comic)
        {
            this.com.Parameters.AddWithValue("@NOMBRE", comic.Nombre);
            this.com.Parameters.AddWithValue("@IMAGEN", comic.Nombre);
            this.com.Parameters.AddWithValue("@DESCRIPCION", comic.Nombre);

            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERT_COMIC";
            this.cn.Open(); 
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void Delete(int idComic)
        {
            string sql = "delete from COMICS where idcomic = @idcomic";
            this.com.Parameters.AddWithValue("@idcomic", idComic);
            this.com.CommandType = CommandType.Text;    
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
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach(var row in consulta)
            {
                Comic cm = new Comic
                {
                    idComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION"),

                };
                comics.Add(cm);
            }
            return comics;
        }

        public void Insert(Comic comic)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            int idComic = consulta.Max(comic => comic.Field<int>("IDCOMIC")) + 1;
            string sql = "insert into COMICS values(@idcomic, @nombre, @imagen, @descripcion)";

            this.com.Parameters.AddWithValue("@idcomic", comic.idComic);
            this.com.Parameters.AddWithValue("@nombre", comic.Nombre);
            this.com.Parameters.AddWithValue("@imagen", comic.Nombre);
            this.com.Parameters.AddWithValue("@descripcion", comic.Descripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql; 
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
