
using Data.Models;

namespace Data.Models;

public class ClientResult : ServiceResult
{
    public IEnumerable<Client>? Result { get; set; }
}
