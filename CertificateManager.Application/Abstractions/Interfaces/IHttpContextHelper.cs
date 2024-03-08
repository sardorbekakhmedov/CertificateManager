namespace Certificate.Application.Abstractions.Interfaces;

public interface IHttpContextHelper
{
    void AddResponseToHeaderData(string key, string value);
}