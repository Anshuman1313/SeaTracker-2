namespace Assiginment.Services
{
    public class CommonService: ICommonService
    {
       public string GenerateTempPassword()
        {
            var random = new Random();
            var number = random.Next(1000, 9999); // 4-digit random number
            return $"SeaTech@{number}";
        }
    }
}
