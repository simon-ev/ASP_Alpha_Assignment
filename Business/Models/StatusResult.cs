using Data.Models;
namespace Business.Models;


public class StatusResult<T> : ServiceResult
{
    public T? Result { get; set; }
}


