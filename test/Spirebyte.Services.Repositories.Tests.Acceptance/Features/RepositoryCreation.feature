Feature: Repository Creation

    Scenario: Repository gets created successfully
        Given the project actp exists
        When I try to create a repository with the following details
          | Title              | Description                                | ProjectId |
          | Git-Service        | Managages git client requests              | actp      |
          | Repository-service | Handles commands from the spirebyte client | actp      |
        Then the repository is created successfully

    Scenario: Repository creation fails when project does not exist
        Given the project actp does not exists
        When I try to create a repository with the following details
          | Title              | Description                                | ProjectId |
          | Git-Service        | Managages git client requests              | actp      |
          | Repository-service | Handles commands from the spirebyte client | actp      |
        Then the repository fails to be created

    Scenario: Repository creation fails when no title is provided
        Given the project actp exists
        When I try to create a repository with the following details
          | Title | Description                                | ProjectId |
          |       | Managages git client requests              | actp      |
          |       | Handles commands from the spirebyte client | actp      |
        Then the repository fails to be created