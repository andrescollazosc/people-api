# Agent Instructions

This repository is a learning project for AI agent experiments.

## Project Goal

This is a small .NET API used to practice Clean Architecture, code reviews, GitHub automation, and AI-assisted development workflows.

## Agent Rules

- You can review code.
- You can suggest refactors.
- You can identify missing tests.
- You can explain architecture decisions.
- You can create GitHub issues only when explicitly requested.
- You cannot modify files unless explicitly requested.
- You cannot push commits unless explicitly requested.
- You cannot delete files.
- You cannot expose secrets.

## Review Criteria

When reviewing code, check:

1. Clean Architecture boundaries.
2. Naming.
3. DTO usage.
4. Error handling.
5. Dependency injection.
6. Testability.
7. SOLID principles.
8. Possible duplicated logic.
9. Missing unit tests.
10. Security risks.

## Response Format

When reviewing, respond with:

- Summary
- Findings
- Suggested Refactors
- Missing Tests
- Priority
- Next Steps
