using System;

namespace Red.WorkPoint.ApiClient
{
    /// <summary>
    /// Entity with customizable fields.
    /// </summary>
    public interface ICustomizableEntity : IEntity
    {
        string StringValue1 { get; set; }
        string StringValue2 { get; set; }
        string StringValue3 { get; set; }
        string StringValue4 { get; set; }
        string StringValue5 { get; set; }

        decimal? DecimalValue1 { get; set; }
        decimal? DecimalValue2 { get; set; }
        decimal? DecimalValue3 { get; set; }
        decimal? DecimalValue4 { get; set; }
        decimal? DecimalValue5 { get; set; }

        DateTime? DateValue1 { get; set; }
        DateTime? DateValue2 { get; set; }
        DateTime? DateValue3 { get; set; }
        DateTime? DateValue4 { get; set; }
        DateTime? DateValue5 { get; set; }

        Guid? LookupValue1Id { get; set; }
        LookupValue LookupValue1 { get; set; }
        Guid? LookupValue2Id { get; set; }
        LookupValue LookupValue2 { get; set; }
        Guid? LookupValue3Id { get; set; }
        LookupValue LookupValue3 { get; set; }
        Guid? LookupValue4Id { get; set; }
        LookupValue LookupValue4 { get; set; }
        Guid? LookupValue5Id { get; set; }
        LookupValue LookupValue5 { get; set; }
        
        int? IntegerValue1 { get; set; }
        int? IntegerValue2 { get; set; }
        int? IntegerValue3 { get; set; }
        int? IntegerValue4 { get; set; }
        int? IntegerValue5 { get; set; }

    }
}