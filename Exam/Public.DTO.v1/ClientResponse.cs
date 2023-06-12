using Newtonsoft.Json;
using Public.DTO.v1.Error;

namespace Public.DTO.v1;

public class ClientResponse<TValue>
{
    public static ClientResponse<TValue> NoContentSuccessful()
    {
        return new ClientResponse<TValue>
        {
            IsSuccessful = true
        };
    }
    
    public static ClientResponse<TValue> Successful(string valueJson)
    {
        return new ClientResponse<TValue>
        {
            IsSuccessful = true,
            Value = JsonConvert.DeserializeObject<TValue>(valueJson)
        };
    }
    
    public static ClientResponse<TValue> Error(string errorResponse)
    {
        return new ClientResponse<TValue>
        {
            IsSuccessful = false,
            ErrorMessage = JsonConvert.DeserializeObject<Message>(errorResponse)
        };
    }
    
    public bool IsSuccessful { get; set; }

    public TValue? Value { get; set; }
    
    public Message? ErrorMessage { get; set; }
    
}