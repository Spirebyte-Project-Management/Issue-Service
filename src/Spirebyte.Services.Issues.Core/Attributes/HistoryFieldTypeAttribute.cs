using System;
using Spirebyte.Services.Issues.Core.Enums;

namespace Spirebyte.Services.Issues.Core.Attributes;

public class HistoryFieldTypeAttribute : Attribute
{
    public HistoryFieldTypeAttribute(FieldTypes fieldType)
    {
        FieldType = fieldType;
    }

    public FieldTypes FieldType { get; }
}