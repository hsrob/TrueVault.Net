using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TrueVault.Net.Dto;
using TrueVault.Net.Dto.Inner;
using TrueVault.Net.Models;
using TrueVault.Net.Models.Inner;

namespace TrueVault.Net
{
	public class AutoMapperConfig
	{
		public static void Configure()
		{
			Mapper.Initialize(c =>
			{
				c.SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
				c.DestinationMemberNamingConvention = new PascalCaseNamingConvention();
			});

			Mapper.CreateMap<TrueVaultResponseDto, TrueVaultResponse>();
			Mapper.CreateMap<InnerErrorDto, InnerError>();
			Mapper.CreateMap<ErrorResponseDto, ErrorResponse>();
			Mapper.CreateMap<DocumentSuccessResponseDto, DocumentSuccessResponse>();
			Mapper.CreateMap<DocumentResponseDto, DocumentResponse>();
			Mapper.CreateMap<MultiDocumentResponseDto, MultiDocumentResponse>();
		}
	}
}
