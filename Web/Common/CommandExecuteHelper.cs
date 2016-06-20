using System;

using ECommon.Components;
using ECommon.Serializing;
using ENode.Commanding;

namespace Web.Common
{
    public class CommandExecuteHelper
    {
        private static IJsonSerializer _jsonSerializer = ObjectContainer.Resolve<IJsonSerializer>();

        public static string CreateErrJsonStr(ICommand command)
        {
            var type = command.GetType();
            if (type == null)
            {
                return null;
            }
            var body = _jsonSerializer.Serialize(command);
            return _jsonSerializer.Serialize(new ErrJsonModel() { AssemblyQualifiedName = type.AssemblyQualifiedName, Body = body });
        }

        public static ICommand CreateCommandObjByErrJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            var jsonModel = _jsonSerializer.Deserialize<ErrJsonModel>(json);
            Type type = Type.GetType(jsonModel.AssemblyQualifiedName);
            if (type == null)
            {
                return null;
            }
            return _jsonSerializer.Deserialize(jsonModel.Body, type) as ICommand;
        }
    }
}