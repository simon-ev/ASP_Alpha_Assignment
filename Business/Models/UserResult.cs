using System.Collections.Generic;
using Data.Models;

namespace Data.Models;

public class UserResult
{
    public bool Succeeded { get; set; }
    public int StatusCode { get; set; }
    public List<User>? Result { get; set; }
    public string? Error { get; set; }
}
