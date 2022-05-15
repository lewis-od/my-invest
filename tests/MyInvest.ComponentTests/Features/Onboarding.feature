Feature: Onboarding
Users signing up for the platform

Scenario: Sign up
	Given my username is test-user
	When I sign up
	Then I get assigned a user ID
	And I have 0 investment accounts
