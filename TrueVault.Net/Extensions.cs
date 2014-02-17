using System;
using System.Text;
using AutoMapper;
using ServiceStack.Text;
using TrueVault.Net.Dto;
using TrueVault.Net.Models;

namespace TrueVault.Net
{
    internal static class Extensions
    {
        public static TResponse MapResponseDto<TDto, TResponse>(this string responseString)
            where TDto : TrueVaultResponseDto where TResponse : TrueVaultResponse
        {
            return Mapper.Map<TResponse>(JsonSerializer.DeserializeFromString<TDto>(responseString));
        }

        public static T MapDocumentResponse<T>(this string documentResponseString) where T : class, new()
        {
            return
                JsonSerializer.DeserializeFromString<T>(
                    Encoding.ASCII.GetString(Convert.FromBase64String(documentResponseString)));
        }
    }
}