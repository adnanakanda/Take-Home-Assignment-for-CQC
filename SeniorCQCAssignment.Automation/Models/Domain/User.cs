namespace SeniorCQCAssignment.Automation.Models.Domain;

public sealed record User
(
    string Email,
    string Password,
    string? SecurityQuestion = null,
    string? SecurityAnswer = null
);