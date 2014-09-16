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
    public class LowerToPascalProfile : Profile
    {
        public override string ProfileName
        {
            get { return "LowerToPascal"; }
        }

        protected override void Configure()
        {
            SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
            DestinationMemberNamingConvention = new PascalCaseNamingConvention();
            CreateMap<TrueVaultResponseDto, TrueVaultResponse>();
            CreateMap<InnerErrorDto, InnerError>();
            CreateMap<ErrorResponseDto, ErrorResponse>();
            #region JSON Store
            CreateMap<DocumentSaveSuccessResponseDto, DocumentSaveSuccessResponse>();
            CreateMap<DocumentGetResponseDto, DocumentResponse>();
            CreateMap<MultiDocumentGetResponseDto, MultiDocumentResponse>();
            #endregion

            CreateMap<SchemaFieldDto, SchemaField>();
            CreateMap<SchemaDto, Schema>();
            CreateMap<SchemaGetResponseDto, SchemaGetResponse>();
            CreateMap<SchemaGetListResponseDto, SchemaGetListResponse>();
            CreateMap<SchemaSaveResponseDto, SchemaSaveResponse>();
            CreateMap<SchemaSaveSuccessResponseDto, SchemaSaveSuccessResponse>();
        }
    }

    public class PascalToLowerProfile : Profile
    {
        public override string ProfileName
        {
            get { return "PascalToLower"; }
        }

        protected override void Configure()
        {
            SourceMemberNamingConvention = new PascalCaseNamingConvention();
            DestinationMemberNamingConvention = new LowerUnderscoreNamingConvention();
            CreateMap<SchemaField, SchemaFieldDto>();
            CreateMap<Schema, SchemaDto>();
        }
    }

    public class AutoMapperConfig
    {

        public static void Configure()
        {
            Mapper.Initialize(c =>
            {
                c.AddProfile<PascalToLowerProfile>();
                c.AddProfile<LowerToPascalProfile>();
            });
        }
    }
}