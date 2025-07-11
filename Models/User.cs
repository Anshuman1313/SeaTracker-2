using System;
using System.Collections.Generic;

namespace Assiginment.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;
    //enum emply admin 

    public bool? IsActive { get; set; } = true;
    //by defalult user true when delete false
    public DateTime? CreatedAt { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<Leaf> Leaves { get; set; } = new List<Leaf>();
}

public class ListResponseModel<T>
{
    public List<T> data { get; set; }

    public int totalItems { get; set; }
    public string msg { get; set; }
    public string code { get; set; }
}
public class ResponseModel<T>
{
    public T? data { get; set; }

    public string msg { get; set; }
    public int code { get; set; }
}
