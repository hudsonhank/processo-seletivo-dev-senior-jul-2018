using Core.Abstractions.Extension.FlakeGen;
using System.Linq;

namespace Core.Abstractions.Types
{
    public static class IdGenerator
    {
        public static long NewId => new Id64Generator().Take(1).FirstOrDefault();

        //public static long NewId
        //{
        //    get
        //    {
        //        return new Id64Generator().Take(1).FirstOrDefault();
        //    }
        //}

        public static long[] Gerar(int quantidade)
        {
            IIdGenerator<long> idGenerator = new Id64Generator();

            return idGenerator.Take(quantidade).ToArray();
        }
    }
}