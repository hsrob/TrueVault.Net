using System;
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

        public static Schema MapToSchema(this SchemaDto schemaDto)
        {
            return new Schema(schemaDto.id, schemaDto.name, Mapper.Map<SchemaField[]>(schemaDto.fields));
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