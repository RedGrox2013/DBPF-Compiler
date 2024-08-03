using DBPF_Compiler.ArgScript;
using System.Reflection;

namespace DBPF_Compiler.Commands
{
    internal class ConfigsCommand : ConsoleCommand
    {
        public override void ParseLine(Line line)
        {
            if (line.ArgumentCount == 1)
            {
                PrintProperties();
                return;
            }

            if (line[1].Equals("get", StringComparison.OrdinalIgnoreCase))
            {
                var prop = typeof(ConfigManager).GetProperty(line[2],
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (prop == null)
                    PrintError(line[2] + " not found");
                else
                    PrintProperty(prop);

                return;
            }

            if (!line[1].Equals("set", StringComparison.OrdinalIgnoreCase))
            {
                PrintError($"\"{line[0]} {line[1]}\": unknown command");
                return;
            }

            PropertyInfo? property = typeof(ConfigManager).GetProperty(line[2],
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
            if (property == null)
            {
                PrintError(line[2] + " not found");
                return;
            }

            if (property.PropertyType == typeof(string))
                property.SetValue(ConfigManager.Instance, line[3]);

            if (line.HasFlag("l"))
                PrintProperties();
        }

        private void PrintProperties()
        {
            foreach (var prop in typeof(ConfigManager).GetProperties(
                    BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.GetCustomAttribute<ConfigsCommandIgnoreAttribute>() == null)
                    PrintProperty(prop);
            }
        }

        private void PrintProperty(PropertyInfo prop)
        {
            Write(prop.Name);
            Write(" = ");
            WriteLine(prop.GetValue(ConfigManager.Instance));
        }

        public override string? GetDescription(DescriptionMode mode = DescriptionMode.Basic)
        {
            if (mode == DescriptionMode.Basic)
                return "Use this command to manage program configurations.";
            if (mode == DescriptionMode.Complete)
                return @"Use this command to manage program configurations.
Usage:  configs <get/set> <property-name> <value> [-l]
<property-name> name of the property
                whose value needs to be viewed/changed
<value>         (only for ""set"") the value to set for the property
-l              use this flag to get a list of properties.
                You can also call the command without arguments
                to get a list of properties.";

            return base.GetDescription(mode);
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigsCommandIgnoreAttribute : Attribute { }
}
