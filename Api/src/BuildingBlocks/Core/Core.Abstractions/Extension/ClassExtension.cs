using Core.Abstractions.Attribute;
using Core.Abstractions.Extension.FlakeGen;
using Core.Abstractions.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.Abstractions.Extension
{
    public static class ClassExtension
    {        
        //public static IEnumerable<PropertyInfo> BuscarPropriedadesPorAtributoDoValor<TCustomAttribute>(object valor)
        //{
        //    return valor.GetPropertiesByCustomAttributes<object, TCustomAttribute>();
        //}

        public static IEnumerable<PropertyInfo> GetPropertiesByCustomAttributes<T, TCustomAttribute>(this T valor)
        {
            return typeof(T).GetProperties()
                .Where(c => c.CustomAttributes != null && c.CustomAttributes.Any(a => a.AttributeType == typeof(TCustomAttribute))).ToList();
        }

        public static PropertyInfo GetPropertieByCustomAttribute<T, TCustomAttribute>(this T valor)
        {
            return typeof(T).GetProperties()
            .Where(c => c.CustomAttributes != null && c.CustomAttributes.Any(a => a.AttributeType == typeof(TCustomAttribute))).FirstOrDefault();
        }

        public static System.Attribute GetCustomAttributesByValor<T, TCustomAttribute>(this T valor)
        {
            System.Attribute attr = null;
            foreach (var property in typeof(T).GetProperties())
            {
                attr = System.Attribute.GetCustomAttributes(property).SingleOrDefault(a => a.GetType() == typeof(TCustomAttribute));
                if (attr != null)
                    return attr;
            }
            return attr;
        }

        public static object GetValue<T>(this T valor, string memberName)
        {
            return typeof(T).GetProperty(memberName).GetValue(valor);
        }

        public static object GetId(this object classe, string propertyName = "Id")
        {
            return classe.GetType().GetProperty(propertyName).GetValue(classe);
        }

        public static Int64 NewId(this object classe, string propertyName = "Id")
        {
            if (classe != null)
            {
                var id = classe.GetType().GetProperty(propertyName);
                if (id != null)
                {

                    long gerado = IdGenerator.NewId;
                    id.SetValue(classe, gerado);
                    return gerado;
                    //if (Convert.ToInt64(id.GetValue(classe)) <= 0)
                    //{                        
                        
                    //}
                }
            }
            return 0;
        }

        public static void NewIdEntity<T>(T entity)
        {
            entity.NewId();
            var props = entity.GetType().GetProperties().ToList().Where(x => x.GetCustomAttribute<EntityPaiAttribute>() != null).ToList();
            object propertyValor;
            if (props != null)
            {
                foreach (var property in props)
                {
                    propertyValor = property.GetValue(entity);
                    if (propertyValor as IList != null && (propertyValor as IList).Count > 0)
                    {
                        foreach (var item in (propertyValor as IList))
                        {
                            NewIdEntity(item);
                        }
                    }
                    else
                    {
                        if (propertyValor != null)
                        {
                            propertyValor.NewId();
                            //ApplyEntityID(propertyValor);
                        }
                    }
                }
            }
        }

        public static PropertyInfo GetProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName);
        }
    }
}