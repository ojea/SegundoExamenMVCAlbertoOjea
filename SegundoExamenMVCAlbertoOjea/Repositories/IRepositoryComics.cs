using SegundoExamenMVCAlbertoOjea.Models;

namespace SegundoExamenMVCAlbertoOjea.Repositories
{
    public interface IRepositoryComics
    {
        List<Comic> GetComics();

        void Insert(Comic comic);
        void CreateComicProcedure(Comic comic);
        void Delete(int idComic);
        Comic GetComicId(int idComic);
    }


}
