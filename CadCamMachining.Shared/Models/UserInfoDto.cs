using System.Collections.Generic;

namespace CadCamMachining.Shared.Models;

public class UserInfoDto
{
    public bool IsAuthenticated { get; set; }
    public string UserName { get; set; }
    public Dictionary<string, string> ExposedClaims { get; set; }
}