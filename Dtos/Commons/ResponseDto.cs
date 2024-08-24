using System;

namespace PixerAPI.Dtos.Commons;

public class ResponseDto<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}
