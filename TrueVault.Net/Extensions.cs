using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ServiceStack.Text;
using TrueVault.Net.Dto;
using TrueVault.Net.Models;

namespace TrueVault.Net
{
	static class Extensions
	{
		public static TResponse MapResponseDto<TDto, TResponse>(this string responseString) where TDto : TrueVaultResponseDto where TResponse : TrueVaultResponse
		{
			return Mapper.Map<TResponse>(JsonSerializer.DeserializeFromString<TDto>(responseString));
		}

		public static T MapDocumentResponse<T>(this string documentResponseString) where T : class, new()
		{
			return JsonSerializer.DeserializeFromString<T>(Encoding.ASCII.GetString(Convert.FromBase64String(documentResponseString)));
		}
	}
}
