using System.Collections.Generic;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Caches;
using JetBrains.ReSharper.Psi.Util;
using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.Util;
using NHibernatePlugin.Helper;
using NHibernatePlugin.LanguageService.Parser;
using NHibernatePlugin.LanguageService.Psi;

namespace NHibernatePlugin.LanguageService
{
    public static class PsiUtils
    {
        public static IAccessor GetPropertySetter(ITypeElement typeElement, string propertyName) {
            if (typeElement == null) {
                return null;
            }
            foreach (IProperty property in typeElement.Properties) {
                if ((property.ShortName == propertyName) && (property.Setter != null)) {
                    return property.Setter;
                }
            }
            IList<IDeclaredType> superTypes = typeElement.GetSuperTypes();
            foreach (IDeclaredType superType in superTypes) {
                IAccessor accessor = GetPropertySetter(superType.GetTypeElement(), propertyName);
                if (accessor != null) {
                    return accessor;
                }
            }
            return null;
        }
        
        public static IAccessor GetPropertyGetter(ITypeElement typeElement, string propertyName) {
            if (typeElement == null) {
                return null;
            }
            foreach (IProperty property in typeElement.Properties) {
                if ((property.ShortName == propertyName) && (property.Getter != null)) {
                    return property.Getter;
                }
            }
            IList<IDeclaredType> superTypes = typeElement.GetSuperTypes();
            foreach (IDeclaredType superType in superTypes) {
                IAccessor accessor = GetPropertySetter(superType.GetTypeElement(), propertyName);
                if (accessor != null) {
                    return accessor;
                }
            }
            return null;
        }
        
        public static IProperty GetProperty(ITypeElement typeElement, string propertyName) {
            if (typeElement == null) {
                return null;
            }
            foreach (IProperty property in typeElement.Properties) {
                if (property.ShortName == propertyName) {
                    return property;
                }
            }
            IList<IDeclaredType> superTypes = typeElement.GetSuperTypes();
            foreach (IDeclaredType superType in superTypes) {
                IProperty property = GetProperty(superType.GetTypeElement(), propertyName);
                if (property != null) {
                    return property;
                }
            }
            return null;
        }

        public static IProperty GetProperty(ISolution solution, NameAttribute nameAttribute, string attributeName, IXmlTag containingElement) {
            string propertyName = nameAttribute.UnquotedValue;
            Logger.LogMessage("GetProperty {0} ({1})", propertyName, attributeName);

            ITypeElement typeElement = GetTypeElement(solution, containingElement, attributeName);
            if (typeElement == null) {
                Logger.LogMessage("  type not found");
                return null;
            }
            Logger.LogMessage("  in type {0}", typeElement.ShortName);
            return GetProperty(typeElement, propertyName);
        }

        private static ITypeElement GetTypeElement(ISolution solution, IXmlTag classTag, string attributeName) {
            IXmlAttribute nameAttribute = classTag.GetAttribute(attributeName);
            if ((nameAttribute == null) || (nameAttribute.UnquotedValue == null)) {
                return null;
            }
            string className = nameAttribute.UnquotedValue;

            IXmlAttribute namespaceAttribute = classTag.GetAttribute("namespace");
            string @namespace = namespaceAttribute == null ? "" : namespaceAttribute.UnquotedValue;

            IXmlAttribute assemblyAttribute = classTag.GetAttribute("assembly");
            string assembly = assemblyAttribute == null ? "" : assemblyAttribute.UnquotedValue;

            return GetTypeElement(solution, className, assembly, @namespace);
        }

        public static IField GetField(ITypeElement typeElement, string fieldName, AccessMethod access) {
            if (typeElement == null) {
                return null;
            }
            foreach (IField field in GetFields(typeElement)) {
                if (field.ShortName == access.Name(fieldName)) {
                    return field;
                }
            }
            IList<IDeclaredType> superTypes = typeElement.GetSuperTypes();
            foreach (IDeclaredType superType in superTypes) {
                IField field = GetField(superType.GetTypeElement(), fieldName, access);
                if (field != null) {
                    return field;
                }
            }
            return null;
        }

        private static IEnumerable<IField> GetFields(ITypeElement typeElement) {
            IList<IField> result = new List<IField>();
            IEnumerable<TypeMemberInstance> members = MiscUtil.GetAllClassMembers(typeElement);
            foreach (TypeMemberInstance member in members) {
                IField field = member.Element as IField;
                if (field != null) {
                    result.Add(field);
                }
            }
            return result;
        }

        public static ITypeElement GetTypeElement(ISolution solution, string fullQualifiedTypeName, string assembly, string @namespace) {
            TypeNameParser typeNameParser = new TypeNameParser(fullQualifiedTypeName, assembly, @namespace);
            if (string.IsNullOrEmpty(typeNameParser.TypeName)) {
                return null;
            }

            PsiManager psiManager = PsiManager.GetInstance(solution);
            IDeclarationsCache declarationsCache = psiManager.GetDeclarationsCache(DeclarationsCacheScope.SolutionScope(solution, true), true);
            ITypeElement typeElement = GetTypeElement(typeNameParser.TypeName, declarationsCache);
            if (typeElement != null) {
                return typeElement;
            }
            return GetTypeElement(typeNameParser.QualifiedTypeName, declarationsCache);
        }

        private static ITypeElement GetTypeElement(string className, IDeclarationsCache declarationsCache) {
            ITypeElement typeElement = declarationsCache.GetTypeElementByCLRName(className);
            if (typeElement == null) {
                IDeclaredElement[] declaredElements = declarationsCache.GetElementsByShortName(className);
                if (declaredElements.Length == 1) {
                    return (ITypeElement)declaredElements[0];
                }
            }
            return typeElement;
        }

        public static ITypeElement GetTypeElement(ISolution solution, string className) {
            return GetTypeElement(solution, className, "", "");
        }

        public static IField GetField(ISolution solution, NameAttribute nameAttribute, string attributeName, IXmlTag containingElement) {
            string fieldName = nameAttribute.UnquotedValue;
            Logger.LogMessage("GetField {0} ({1})", fieldName, attributeName);

            AccessMethod access = null;
            XmlTag xmlTag = nameAttribute.GetContainingElement<XmlTag>(true);
            if (xmlTag != null) {
                IXmlAttribute accessAttribute = xmlTag.GetAttribute("access");
                if (accessAttribute != null) {
                    Logger.LogMessage("  access attribute found: {0}", accessAttribute.UnquotedValue);
                    access = new AccessMethod(accessAttribute.UnquotedValue);
                }
            }
            if (access == null) {
                HibernateMappingTag hibernateMappingTag = Parent<HibernateMappingTag>(nameAttribute, Keyword.HibernateMapping);
                if (hibernateMappingTag != null) {
                    IXmlAttribute accessAttribute = hibernateMappingTag.GetDefaultAccessAttribute();
                    if (accessAttribute != null) {
                        Logger.LogMessage("  default access attribute found: {0}", accessAttribute.UnquotedValue);
                        access = new AccessMethod(accessAttribute.UnquotedValue);
                    }
                }
            }
            if (access == null) {
                access = new AccessMethod("");
            }

            ITypeElement typeElement = GetTypeElement(solution, containingElement, attributeName);
            if (typeElement == null) {
                return null;
            }
            IField field = GetField(typeElement, fieldName, access);
            if (field == null) {
                return null;
            }
            Logger.LogMessage("  field found: {0}", field.ShortName);
            return field;
        }

        public static ComponentTag ParentComponent(NameAttribute nameAttribute) {
            return Parent<ComponentTag>(nameAttribute, Keyword.Component);
        }

        public static CompositeElementTag ParentComposite(NameAttribute nameAttribute) {
            return Parent<CompositeElementTag>(nameAttribute, Keyword.CompositeElement);
        }

        public static NestedCompositeElementTag ParentNestedComposite(NameAttribute nameAttribute) {
            return Parent<NestedCompositeElementTag>(nameAttribute, Keyword.NestedCompositeElement);
        }

        public static SubclassTag ParentSubclass(NameAttribute nameAttribute) {
            return Parent<SubclassTag>(nameAttribute, Keyword.Subclass);
        }

        private static T Parent<T>(NameAttribute nameAttribute, string keyword) where T : IXmlTag {
            T subclassTag = nameAttribute.GetContainingElement<T>(false);
            if (subclassTag != null) {
                if (nameAttribute.ContainerName == keyword) {
                    subclassTag = subclassTag.GetContainingElement<T>(false);
                }
            }
            return subclassTag;
        }
    }
}