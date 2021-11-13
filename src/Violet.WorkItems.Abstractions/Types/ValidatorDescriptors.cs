using System;

namespace Violet.WorkItems.Types;

public record ValidatorDescriptor(string Type);

public record StringLengthValidatorDescriptor(int Min, int Max)
    : ValidatorDescriptor("StringLength");
public record MandatoryValidatorDescriptor()
    : ValidatorDescriptor("Mandatory");
