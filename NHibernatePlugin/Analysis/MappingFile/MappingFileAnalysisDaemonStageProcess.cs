using JetBrains.Application.Progress;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace NHibernatePlugin.Analysis.MappingFile
{
    public class MappingFileAnalysisDaemonStageProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess m_DaemonProcess;

        public MappingFileAnalysisDaemonStageProcess(IDaemonProcess daemonProcess) {
            m_DaemonProcess = daemonProcess;
        }

        /*
        public DaemonStageProcessResult Execute() {
            Logger.LogMessage("NHibernatePlugin: MappingFileAnalysisDaemonStageProcess.Execute called");
            DaemonStageProcessResult result = new DaemonStageProcessResult();

            IFile file = PsiManager.PsiFile(m_DaemonProcess.ProjectFile);
            if (file == null) {
                Logger.LogMessage("   NO PSI FILE !!! {0}", m_DaemonProcess.ProjectFile.Name);
                return result;
            }

            MappingFileAnalysisElementProcessor elementProcessor = new MappingFileAnalysisElementProcessor(m_DaemonProcess);
            file.ProcessDescendants(elementProcessor);

            if (m_DaemonProcess.InterruptFlag) {
                throw new ProcessCancelledException();
            }

            result.FullyRehighlighted = true;
            result.Highlightings = elementProcessor.Highlightings.ToArray();

            return result;
        }*/

        #region IDaemonStageProcess Members

        /// <summary>
        /// Executes the process.
        /// The process should check for <see cref="P:JetBrains.ReSharper.Daemon.IDaemonProcess.InterruptFlag"/> periodically (with intervals less than 100 ms)
        /// and throw <see cref="T:JetBrains.Application.Progress.ProcessCancelledException"/> if it is true.
        /// Failing to do so may cause the program to prevent user from typing while analysing the code.
        /// Stage results should be passed to <param name="commiter"/>. If DaemonStageResult is <c>null</c>, it means that no highlightings available
        /// </summary>
        /// <param name="commiter"></param>
        public void Execute(System.Action<DaemonStageResult> commiter)
        {
            Logger.LogMessage("NHibernatePlugin: MappingFileAnalysisDaemonStageProcess.Execute called");
            //DaemonStageProcessResult result = new DaemonStageProcessResult();

            IFile file = PsiManager.PsiFile(m_DaemonProcess.ProjectFile);
            if (file == null)
            {
                Logger.LogMessage("   NO PSI FILE !!! {0}", m_DaemonProcess.ProjectFile.Name);
                return; // result;
            }

            MappingFileAnalysisElementProcessor elementProcessor = new MappingFileAnalysisElementProcessor(m_DaemonProcess);
            file.ProcessDescendants(elementProcessor);

            if (m_DaemonProcess.InterruptFlag)
            {
                throw new ProcessCancelledException();
            }

            /*result.FullyRehighlighted = true;
            result.Highlightings = elementProcessor.Highlightings.ToArray();

            return result;*/
            return;
        }

        #endregion
    }
}