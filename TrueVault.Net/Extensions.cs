using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using ServiceStack.Text;
using TrueVault.Net.Dto;
using TrueVault.Net.Dto.Schema;
using TrueVault.Net.Models;
using TrueVault.Net.Models.Schema;

namespace TrueVault.Net
{
    internal static class Extensions
    {
        public static TResponse AutoMapResponseDto<TDto, TResponse>(this string responseString)
            where TDto : TrueVaultResponseDto where TResponse : TrueVaultResponse
        {
            return Mapper.Map<TResponse>(JsonSerializer.DeserializeFromString<TDto>(responseString));
        }

        public static T MapBase64StringResponse<T>(this string respString) where T : class, new()
        {
            return
                JsonSerializer.DeserializeFromString<T>(
                    Encoding.ASCII.GetString(Convert.FromBase64String(respString)));
        }
        
        public static T MapStringResponse<T>(this string respString) where T : class, new()
        {
            return
                JsonSerializer.DeserializeFromString<T>(respString);
        }

        public static Schema MapToSchema(this SchemaDto schemaDto)
        {
            return new Schema(schemaDto.id, schemaDto.name, Mapper.Map<SchemaField[]>(schemaDto.fields));
        }

        public static SchemaSaveSuccessResponse MapToSchemaSaveSuccessResponse(this string schemaSaveSuccessResponseString)
        {
            var respDto = schemaSaveSuccessResponseString.MapStringResponse<SchemaSaveSuccessResponseDto>();
            return new SchemaSaveSuccessResponse(respDto.result, respDto.transaction_id,
                new SchemaSaveResponse(respDto.schema.vault_id, respDto.schema.id, respDto.schema.name,
                    Mapper.Map<SchemaField[]>(respDto.schema.fields)));
        }
        
        public static SchemaGetResponse MapToSchemaGetResponse(this string schemaGetResponseString)
        {
            var respDto = schemaGetResponseString.MapStringResponse<SchemaGetResponseDto>();
            return respDto.MapToSchemaGetResponse();
        }

        public static SchemaGetResponse MapToSchemaGetResponse(this SchemaGetResponseDto schemaGetResponseDto)
        {
            return new SchemaGetResponse(schemaGetResponseDto.result, schemaGetResponseDto.transaction_id, schemaGetResponseDto.schema.MapToSchema());
        }
        
        public static SchemaGetListResponse MapToSchemaGetListResponse(this string schemaGetListResponseString)
        {
            var respDto = schemaGetListResponseString.MapStringResponse<SchemaGetListResponseDto>();
            return respDto.MapToSchemaGetListResponse();
        }

        public static SchemaGetListResponse MapToSchemaGetListResponse(this SchemaGetListResponseDto schemaGetListResponseDto)
        {
            return new SchemaGetListResponse(schemaGetListResponseDto.result, schemaGetListResponseDto.transaction_id, schemaGetListResponseDto.schemas.Select(sd => sd.MapToSchema()));
        }
    }

    public static class Utils
    {
        public static MemberExpression GetMemberExpression<T>(Expression<Func<T, object>> expr)
        {
            var member = expr.Body as MemberExpression;
            var unary = expr.Body as UnaryExpression;
            return member ?? (unary != null ? unary.Operand as MemberExpression : null);
        }
    }
}