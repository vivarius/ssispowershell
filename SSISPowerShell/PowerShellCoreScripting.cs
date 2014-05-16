using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using SSISPowerShellTask110.SSIS;
using VariableDispenser = Microsoft.SqlServer.Dts.Runtime.VariableDispenser;

namespace SSISPowerShellTask110
{
    internal class PowerShellCoreScripting
    {
        public static string FormatPowerShellScript(string scriptText)
        {
            string formatText = scriptText.Trim();

            foreach (var match in Regex.Matches(formatText, @"\@(.*?)\]"))
            {
                formatText = formatText.Replace(match.ToString(), string.Format("'\"+{0}+\"'", match));
            }

            return string.Format("\"{0}\"", formatText);
        }

        public static string RunScript(string scriptText, VariableDispenser variableDispenser)
        {
            Collection<PSObject> results;
            using (Runspace runspace = RunspaceFactory.CreateRunspace())
            {
                runspace.Open();

                using (Pipeline pipeline = runspace.CreatePipeline())
                {
                    pipeline.Commands.AddScript(EvaluateExpression(FormatPowerShellScript(scriptText), variableDispenser).ToString());
                    pipeline.Commands.Add("Out-String");

                    results = pipeline.Invoke();
                }
                runspace.Close();
            }

            var stringBuilder = new StringBuilder();

            if (results != null)
                foreach (PSObject obj in results)
                {
                    stringBuilder.AppendLine(obj.ToString());
                }

            return stringBuilder.ToString();
        }

        private static object EvaluateExpression(string mappedParam, VariableDispenser variableDispenser)
        {
            object variableObject;

            try
            {
                var expressionEvaluatorClass = new ExpressionEvaluator
                {
                    Expression = mappedParam
                };

                expressionEvaluatorClass.Evaluate(DtsConvert.GetExtendedInterface(variableDispenser), out variableObject, false);
            }
            catch (Exception) // for already initialized values
            {
                variableObject = mappedParam;
            }

            return variableObject;
        }

        public static void GetNeededVariables(VariableDispenser variableDispenser, ref Variables variables, IDTSComponentEvents componentEvents, SSISPowerShellTask ssisPowerShellTask)
        {
            bool refire = false;
            var lockForReadWrite = new List<string>();

            {
                var regex = new Regex(@"\@(.*?)\]");
                componentEvents.FireInformation(0, "SSISPowerShell", String.Format("Search variable to LockForRead "), string.Empty, 0, ref refire);

                foreach (var match in regex.Matches(ssisPowerShellTask.PowerShellScript))
                {
                    componentEvents.FireInformation(0, "SSISPowerShell", String.Format("Match : {0} ", match), string.Empty, 0, ref refire);
                    var mappedParams = match.ToString().Split(new[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                    componentEvents.FireInformation(0, "SSISPowerShell", String.Format("mappedParams : {0} ", mappedParams), string.Empty, 0, ref refire);
                    LockForRead(variableDispenser, ref variables, lockForReadWrite, mappedParams, componentEvents);
                }
            }

            {
                var mappedParams = ssisPowerShellTask.OutputVariableName.Split(new[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                LockForWrite(variableDispenser, ref variables, lockForReadWrite, mappedParams, componentEvents);
            }
        }

        public static string GetVariableFromNamespaceContext(string ssisVariable)
        {
            return ssisVariable.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("[", string.Empty).Replace("]", string.Empty).Replace("@", string.Empty);
        }

        private static void LockForRead(VariableDispenser variableDispenser, ref Variables variables, List<string> lockForReadWrite, IEnumerable<string> mappedParams, IDTSComponentEvents componentEvents)
        {
            bool refire = false;

            foreach (string t in mappedParams)
            {
                try
                {
                    string param = t.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("[", string.Empty).Replace("]", string.Empty).Replace("@", string.Empty);
                    if (!IsVariableInLockForReadOrWrite(lockForReadWrite, param))
                    {
                        componentEvents.FireInformation(0, "SSISPowerShell", String.Format("LockForRead: {0} ", param), string.Empty, 0, ref refire);
                        variableDispenser.LockOneForRead(param, ref variables);
                    }
                }
                catch (Exception)
                {
                    componentEvents.FireInformation(0, "SSISPowerShell", String.Format("Unable to LockForRead: {0} ", t), string.Empty, 0, ref refire);
                }
            }
        }

        private static void LockForWrite(VariableDispenser variableDispenser, ref Variables variables, List<string> lockForReadWrite, IEnumerable<string> mappedParams, IDTSComponentEvents componentEvents)
        {
            bool refire = false;

            foreach (string t in mappedParams)
            {
                try
                {
                    string param = t.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("[", string.Empty).Replace("]", string.Empty).Replace("@", string.Empty);
                    if (!IsVariableInLockForReadOrWrite(lockForReadWrite, param))
                    {
                        componentEvents.FireInformation(0, "SSISPowerShell", String.Format("LockForWrite: {0} ", param), string.Empty, 0, ref refire);
                        variableDispenser.LockOneForWrite(param, ref variables);
                    }
                }
                catch (Exception)
                {
                    componentEvents.FireInformation(0, "SSISPowerShell", String.Format("Unable to LockForWrite: {0} ", t), string.Empty, 0, ref refire);
                }
            }
        }

        private static bool IsVariableInLockForReadOrWrite(List<string> lockForReadWrite, string variable)
        {
            bool retVal = lockForReadWrite.Contains(variable);

            if (!retVal)
            {
                lockForReadWrite.Add(variable);
            }

            return retVal;
        }
    }
}
