namespace SeniorCQCAssignment.Automation.Models.Domain;

public sealed record User
(
    string Email,
    string Password,
    int SecurityQuestionId,
    string SecurityAnswer
);