using Spirebyte.Services.Issues.Core.Enums;
using System;

namespace Spirebyte.Services.Issues.Core.Attributes
{
    public class HistoryFieldTypeAttribute : Attribute
    {
        public FieldTypes FieldType { get; }

        public HistoryFieldTypeAttribute(FieldTypes fieldType)
        {
            FieldType = fieldType;
        }
    }
}
