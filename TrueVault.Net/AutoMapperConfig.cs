using AutoMapper;
using TrueVault.Net.Dto;
using TrueVault.Net.Dto.Inner;
using TrueVault.Net.Dto.JsonStore;
using TrueVault.Net.Dto.Schema;
using TrueVault.Net.Models;
using TrueVault.Net.Models.Inner;
using TrueVault.Net.Models.JsonStore;
using TrueVault.Net.Models.Schema;

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
            #region JSON Store
            Mapper.CreateMap<DocumentSuccessResponseDto, DocumentSuccessResponse>();
            Mapper.CreateMap<DocumentResponseDto, DocumentResponse>();
            Mapper.CreateMap<MultiDocumentResponseDto, MultiDocumentResponse>();
            #endregion
            #region Schemas
            Mapper.CreateMap<SchemaFieldDto, SchemaField>().ReverseMap();
            Mapper.CreateMap<SchemaDto, Schema>().ReverseMap();
            Mapper.CreateMap<SchemaCreateResponseDto, SchemaResponse>();
            Mapper.CreateMap<SchemaCreateSuccessResponseDto, SchemaSuccessResponse>();
            #endregion
        }
    }
}