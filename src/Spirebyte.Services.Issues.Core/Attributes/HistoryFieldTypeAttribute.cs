using System;
using System.Collections.Generic;
using System.Text;
using Spirebyte.Services.Issues.Core.Enums;

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
