using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.Util;

namespace NHibernatePlugin.LanguageService
{
    public class MappingFileTypePresenter : ITypePresenter
    {
        public static readonly MappingFileTypePresenter Instance = new MappingFileTypePresenter();

        public string GetPresentableName(IType type) {
            Logger.LogMessage("MappingFileTypePresenter.GetPresentableName {0}", type);
            return GetTypePresenter(CSharpLanguageService.CSHARP).GetPresentableName(type);
        }

        public string GetLongPresentableName(IType type) {
            Logger.LogMessage("MappingFileTypePresenter.GetLongPresentableName {0}", type);
            return GetTypePresenter(CSharpLanguageService.CSHARP).GetLongPresentableName(type);

        }

        /// <summary>
        /// Gets the unresolved scalar type presentation.
        /// </summary>
        /// <param name="name">The name of the unresolved scalar type presentation.</param>
        /// <param name="typeArguments">The type arguments.</param>
        /// <returns>Some string message that is displayed</returns>
        public string GetUnresolvedScalarTypePresentation(string name, ICollection<IType> typeArguments) 
        {
            string fulltypestring = String.Empty;
            foreach (IType type in typeArguments) 
            {
                fulltypestring += GetTypePresenter(CSharpLanguageService.CSHARP).GetLongPresentableName(type);
            }
            Logger.LogMessage("MappingFileTypePresenter.GetUnresolvedScalarTypePresentation {0}", fulltypestring);
            return fulltypestring;
        }

        private static ITypePresenter GetTypePresenter(PsiLanguageType language) {
            JetBrains.ReSharper.Psi.LanguageService languageService = LanguageServiceManager.Instance.GetLanguageService(language);
            if (languageService != null) {
                return languageService.TypePresenter;
            }
            return null;
        }
    }
}