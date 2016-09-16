using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace Mono
{
    public class CecilImporter
    {
        public delegate TResult Func<TIn, TResult>(TIn param);

        public CecilImporter(ModuleDefinition targetModule)
        {
            this.TargetModule = targetModule;
        }

        public FieldDefinition CreateImportedField(TypeDefinition declaringType, FieldDefinition targetField)
        {
            FieldDefinition definition = new FieldDefinition(targetField.Name, targetField.Attributes, this.ImportTypeReference(declaringType, targetField.FieldType));
            if (definition.HasConstant)
            {
                definition.Constant = targetField.Constant;
            }
            return definition;
        }

        public MethodDefinition CreateImportedMethod(TypeDefinition declaringType, MethodDefinition targetMethod)
        {
            MethodDefinition definition = new MethodDefinition(targetMethod.Name, targetMethod.Attributes, this.ImportTypeReference(declaringType, targetMethod.ReturnType));
            if (targetMethod.HasParameters)
            {
                foreach (ParameterDefinition definition2 in targetMethod.Parameters)
                {
                    ParameterDefinition item = new ParameterDefinition(definition2.Name, definition2.Attributes, this.ImportTypeReference(declaringType, definition2.ParameterType));
                    definition.Parameters.Add(item);
                }
            }
            return definition;
        }

        public MethodBody CreateImportedMethodBody(TypeDefinition declaringType, MethodDefinition newMethod, MethodBody originalBody)
        {
            MethodBody body = new MethodBody(newMethod);
            ILProcessor iLProcessor = body.GetILProcessor();
            if (originalBody.HasVariables)
            {
                foreach (VariableDefinition definition in originalBody.Variables)
                {
                    body.Variables.Add(new VariableDefinition(this.ImportTypeReference(declaringType, definition.VariableType)));
                }
            }
            for (int i = 0; i < originalBody.Instructions.Count; i++)
            {
                Instruction instruction = originalBody.Instructions[i];
                object operand = instruction.Operand;
                if (operand != null)
                {
                    if (operand is TypeReference)
                    {
                        iLProcessor.Emit(instruction.OpCode, this.ImportTypeReference(declaringType, operand as TypeReference));
                    }
                    else if (operand is MethodReference)
                    {
                        iLProcessor.Emit(instruction.OpCode, this.ImportMethodReference(declaringType, operand as MethodReference));
                    }
                    else if (operand is FieldReference)
                    {
                        iLProcessor.Emit(instruction.OpCode, this.ImportFieldReference(declaringType, operand as FieldReference));
                    }
                    else
                    {
                        iLProcessor.Append(instruction);
                    }
                }
                else
                {
                    iLProcessor.Append(instruction);
                }
            }
            if (originalBody.HasExceptionHandlers)
            {
                foreach (ExceptionHandler handler in originalBody.ExceptionHandlers)
                {
                    ExceptionHandler item = new ExceptionHandler(handler.HandlerType)
                    {
                        TryStart = this.GetInstructionByOffset(body, handler.TryStart.Offset),
                        TryEnd = this.GetInstructionByOffset(body, handler.TryEnd.Offset),
                        HandlerStart = this.GetInstructionByOffset(body, handler.HandlerStart.Offset),
                        HandlerEnd = this.GetInstructionByOffset(body, handler.HandlerEnd.Offset)
                    };
                    if (item.FilterStart != null)
                    {
                        item.FilterStart = this.GetInstructionByOffset(body, handler.FilterStart.Offset);
                    }
                    if (handler.CatchType != null)
                    {
                        item.CatchType = this.ImportTypeReference(declaringType, handler.CatchType);
                    }
                    body.ExceptionHandlers.Add(item);
                }
            }
            return body;
        }

        public TypeDefinition CreateImportedType(TypeDefinition targetType, string ns = "")
        {
            TypeDefinition declaringType = null;
            if (ns == "")
            {
                declaringType = new TypeDefinition(targetType.Namespace, targetType.Name, targetType.Attributes);
            }
            else
            {
                declaringType = new TypeDefinition(ns, targetType.Name, targetType.Attributes);
            }

            if (targetType.HasFields)
            {
                foreach (FieldDefinition definition2 in targetType.Fields)
                {
                    declaringType.Fields.Add(this.CreateImportedField(declaringType, definition2));
                }
            }
            if (targetType.HasMethods)
            {
                foreach (MethodDefinition definition3 in targetType.Methods)
                {
                    declaringType.Methods.Add(this.CreateImportedMethod(declaringType, definition3));
                }
                for (int i = 0; i < declaringType.Methods.Count; i++)
                {
                    if (targetType.Methods[i].HasBody)
                    {
                        declaringType.Methods[i].Body = this.CreateImportedMethodBody(declaringType, declaringType.Methods[i], targetType.Methods[i].Body);
                    }
                }
            }
            if (targetType.BaseType != null)
            {
                declaringType.BaseType = this.ImportTypeReference(declaringType, targetType.BaseType);
            }
            if (targetType.HasInterfaces)
            {
                foreach (TypeReference reference in targetType.Interfaces)
                {
                    declaringType.Interfaces.Add(this.ImportTypeReference(declaringType, reference));
                }
            }
            return declaringType;
        }

        private Instruction GetInstructionByOffset(MethodBody body, int offset)
        {
            return Extensions.FirstOrDefault<Instruction>(body.Instructions, i => i.Offset == offset);
        }

        private FieldReference ImportFieldReference(TypeDefinition declaringType, FieldReference targetFieldRef)
        {
            Func<FieldDefinition, bool> condition = null;
            if (declaringType.HasFields)
            {
                if (condition == null)
                {
                    condition = f => f.Name == targetFieldRef.Name;
                }
                FieldDefinition definition = Extensions.FirstOrDefault<FieldDefinition>(declaringType.Fields, condition);
                if (definition != null)
                {
                    return definition;
                }
            }
            return this.TargetModule.Import(targetFieldRef);
        }

        private MethodReference ImportMethodReference(TypeDefinition declaringType, MethodReference targetMethodRef)
        {
            Func<MethodDefinition, bool> condition = null;
            if (declaringType.HasMethods)
            {
                if (condition == null)
                {
                    condition = m => m.Name == targetMethodRef.Name;
                }
                MethodDefinition definition = Extensions.FirstOrDefault<MethodDefinition>(declaringType.Methods, condition);
                if (definition != null)
                {
                    return definition;
                }
            }
            return this.TargetModule.Import(targetMethodRef);
        }

        private TypeReference ImportTypeReference(TypeDefinition declaringType, TypeReference targetTypeRef)
        {
            if (declaringType.FullName == targetTypeRef.FullName)
            {
                return declaringType;
            }
            return this.TargetModule.Import(targetTypeRef);
        }

        public ModuleDefinition TargetModule { get; private set; }
    }

    public static class Extensions
    {
        public static T FirstOrDefault<T>(Collection<T> array, CecilImporter.Func<T, bool> condition)
        {
            foreach (T local in array)
            {
                if (condition(local))
                {
                    return local;
                }
            }
            return default(T);
        }
    }
}