using System;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using NHibernatePlugin.LanguageService;

namespace NHibernatePlugin.Analysis.MappingFile
{
    [LanguageSpecificImplementation(MappingFileLanguageService.MAPPING_FILE_LANGUAGEID, typeof(ILanguageSpecificDaemonBehavior))]
    public class MappingFileDaemonBehaviour : ILanguageSpecificDaemonBehavior
    {
        /*public ErrorStripeRequest InitialErrorStripe(IProjectFile file) {
            if (PsiSupportManager.Instance.ShouldBuildPsi(file) &&
                (ProjectFileLanguageServiceManager.Instance.GetPsiLanguageType(file) == MappingFileLanguageService.MAPPING_FILE)) {
                return ErrorStripeRequest.STRIPE_AND_ERRORS;
            }
            return ErrorStripeRequest.NONE;
        }*/

        public ErrorStripeRequest InitialErrorStripe(IProjectFile file) {
            return ErrorStripeRequest.STRIPE_AND_ERRORS;
        }

        public bool CanShowErrorBox {
            get { return true; }
        }

        public bool RunInSolutionAnalysis {
            get { return false; }
        }
    }
}