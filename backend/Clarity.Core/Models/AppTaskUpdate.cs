using Clarity.Core.Enums;

namespace Clarity.Core.Models;

public record AppTaskUpdate(
    string ? Title,
    string? Description,
    AppTaskStatus? Status);