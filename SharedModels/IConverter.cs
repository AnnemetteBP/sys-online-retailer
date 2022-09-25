using System.Collections.Generic;

namespace SharedModels.Converters
{
    public interface IConverter<T,U>
    {
        T Convert(U model);
        U Convert(T model);
        IEnumerable<T> ConvertAll(IEnumerable<U> models);
        IEnumerable<U> ConvertAll(IEnumerable<T> models);
    }
}
