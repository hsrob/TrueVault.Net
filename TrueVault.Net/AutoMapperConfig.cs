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
            Mapper.CreateMap<DocumentSaveSuccessResponseDto, DocumentSaveSuccessResponse>();
            Mapper.CreateMap<DocumentGetResponseDto, DocumentResponse>();
            Mapper.CreateMap<MultiDocumentGetResponseDto, MultiDocumentResponse>();
            #endregion
            #region Schemas
            Mapper.CreateMap<SchemaFieldDto, SchemaField>().ReverseMap();
            //Important: SchemaDto -> Schema is not Automapped, and must be mapped manually. This is because Schema's Id has an internal setter.
            Mapper.CreateMap<Schema, SchemaDto>();
            Mapper.CreateMap<SchemaGetResponseDto, SchemaGetResponse>();
            Mapper.CreateMap<SchemaGetListResponseDto, SchemaGetListResponse>();
            Mapper.CreateMap<SchemaSaveResponseDto, SchemaSaveResponse>();
            Mapper.CreateMap<SchemaSaveSuccessResponseDto, SchemaSaveSuccessResponse>();
            #endregion
        }
    }
}