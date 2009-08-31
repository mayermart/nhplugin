using System;
using JetBrains.Application.Progress;
using JetBrains.ReSharper.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;

namespace NHibernatePlugin.Analysis.CSharp
{
    public class CSharpAnalysisDaemonStageProcess : IDaemonStageProcess
    {
        private readonly IDaemonProcess m_DaemonProcess;
        
        public CSharpAnalysisDaemonStageProcess(IDaemonProcess daemonProcess) {
            m_DaemonProcess = daemonProcess;
        }

        /*
        public DaemonStageProcessResult Execute() {
            // Creating container to put highlightings to
            DaemonStageProcessResult result = new DaemonStageProcessResult();

            // Getting PSI (AST) for the file being highlighted
            PsiManager manager = PsiManager.GetInstance(m_DaemonProcess.Solution);
            IFile file = manager.GetPsiFile(m_DaemonProcess.ProjectFile);
            if (file == null) {
                return result;
            }

            // Running visitor against the PSI
            CSharpAnalysisElementProcessor elementProcessor = new CSharpAnalysisElementProcessor(m_DaemonProcess);
            file.ProcessDescendants(elementProcessor);

            // Checking if the daemon is interrupted by user activity
            if (m_DaemonProcess.InterruptFlag) {
                throw new ProcessCancelledException();
            }

            // Fill in the result
            result.FullyRehighlighted = true;
            result.Highlightings = elementProcessor.Highlightings.ToArray();

            return result;
        }
        */

        /// <summary>
        /// Executes the process.
        /// The process should check for <see cref="P:JetBrains.ReSharper.Daemon.IDaemonProcess.InterruptFlag"/> periodically (with intervals less than 100 ms)
        /// and throw <see cref="T:JetBrains.Application.Progress.ProcessCancelledException"/> if it is true.
        /// Failing to do so may cause the program to prevent user from typing while analysing the code.
        /// Stage results should be passed to <param name="commiter"/>. If DaemonStageResult is <c>null</c>, it means that no highlightings available
        /// </summary>
        /// <param name="commiter"></param>
        public void Execute(Action<DaemonStageResult> commiter) 
        {
            
            // Getting PSI (AST) for the file being highlighted
            PsiManager manager = PsiManager.GetInstance(this.m_DaemonProcess.Solution);
            IFile file = manager.GetPsiFile(this.m_DaemonProcess.ProjectFile);
            if (file == null)
            {
                return; // result;
            }

            // Running visitor against the PSI
            CSharpAnalysisElementProcessor elementProcessor = new CSharpAnalysisElementProcessor(m_DaemonProcess);
            file.ProcessDescendants(elementProcessor);

            // Checking if the daemon is interrupted by user activity
            if (this.m_DaemonProcess.InterruptFlag)
            {
                throw new ProcessCancelledException();
            }

            // Fill in the result
            //commiter.Invoke(this.m_DaemonProcess.);
            //result.FullyRehighlighted = true;
            //result.Highlightings = elementProcessor.Highlightings.ToArray();

            return; // result;
        }
    }
}