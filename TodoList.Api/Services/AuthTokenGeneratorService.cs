namespace TodoList.Api.Services;

public class AuthTokenGeneratorService
{
    private readonly long _length;
    private readonly Random _random;
    
    public AuthTokenGeneratorService(long length)
    {
        _length = length;
        _random = new Random();
    }

    public string Generate()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        return new string(Enumerable
            .Repeat(chars, (int)_length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}