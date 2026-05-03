using System;

namespace Documentation;

[AttributeUsage(AttributeTargets.Method)]
public class ApiMethodAttribute : Attribute
{
}

public class ApiDescriptionAttribute(string description) : Attribute
{
	public string Description { get; } = description;
}

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class ApiIntValidationAttribute(int minValue, int maxValue) : Attribute
{
	public int? MaxValue { get; } = maxValue;
	public int? MinValue { get; } = minValue;
}

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class ApiRequiredAttribute(bool required = true) : Attribute
{
	public bool Required { get; } = required;
}